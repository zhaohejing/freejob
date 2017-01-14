using HuaTong.General.Utility;
using Job.Application;
using Job.Application.IService;
using Job.Application.Service;
using Job.Core;
using Job.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Security;
using static Job.Application.Service.BaseService;

namespace Job.Controllers {
    /// <summary>
    /// 账户操作
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController {
        public string AppId { get; set; } = System.Configuration.ConfigurationManager.AppSettings["AppId"];
        public string AppSecret { get; set; } = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
        /// <summary>
        /// ctor
        /// </summary>
        public AccountController() {
        }
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginViewModel model) {
            var t = _userService.GetUserInfo(model.OpenId);
            if (t == null) {
                var res = _userService.RegisterWeChat(model.OpenId);
                if (res == ErrEnum.Ok) {
                    t = _userService.GetUserInfo(model.OpenId);
                }
            }
            var token = $"{t.Id}-{t.WeChatToken}-{GetTimeStamp()}";
            RedisHelper.SetStringKey(t.WeChatToken, token, TimeSpan.FromDays(3));
            return Json(new { Result = 1, ErrorMsg = "登陆成功", Data = t });
        }
        /// <summary>
        /// 个人用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RegisterUser(UserInfo user) {
            if (CurrentUser.HadRegisterUser > 0) {
                return Json(new { Result = 0, ErrorMsg = "该账户已注册个人信息,请登录" });
            }
            var res = _userService.RegisterUserInfo(user, CurrentUser);
            if (res <= 0) {
                return Json(new { Result = -1, ErrorMsg = "注册失败,请稍后重试" });
            }
            return Json(new { Result = 1, ErrorMsg = "注册成功" });
        }
        /// <summary>
        /// 公司账户注册
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]

        public IHttpActionResult RegisterCompany(CompanyInfo company) {
            if (CurrentUser.HadRegisterCompany > 0) {
                return Json(new { Result = 0, ErrorMsg = "该账户已注册公司信息,请登录" });
            }
            var res = _userService.RegisterCompanyInfo(company, CurrentUser);
            if (res <= 0) {
                return Json(new { Result = 0, ErrorMsg = "注册失败,请稍后重试" });
            }
            return Json(new { Result = 1, ErrorMsg = "注册成功" });
        }
        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendMessgae(MessageModel model) {
            var res = _userService.SendMessage(model);
            //if (res==ErrEnum.SendMessageOk) {
            return Json(new { Result = 1, ErrorMsg = "发送成功", Data = res });
            //}
            //return Json(new { Result = 0, ErrorMsg = "发送失败" });
        }
        /// <summary>
        /// 验证手机验证码是否正确
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VilidateMessage(MessageModel model) {
            var res = _userService.VilidateCode(model.Mobile, model.Code);
            return Json(new { Result = res ? 1 : 0, ErrorMsg = res ? "验证通过" : "验证失败" });
        }

        private static string GetTimeStamp() {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 获取redis中的公司名称
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RedisCompanys(string filter) {
            var res = RedisHelper.HashGet<List<TempSerialize>>("Company", "CompanyName");
            var result = res.Where(c => c.Name.Contains(filter)).OrderBy(c => c.Id).Take(5);
            return Json(new { Result = res.Count > 0 ? 1 : 0, Data = result });
        }
        /// <summary>
        /// 从缓存中获取 工作名称
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RedisWorks(string filter) {
            var res = RedisHelper.HashGet<List<TempSerialize>>("Work", "WorkName");

            var result = res.Where(c => c.Name.Contains(filter)).OrderBy(c => c.Id).Take(5);
            return Json(new { Result = res.Count > 0 ? 1 : 0, Data = result });
        }
        /// <summary>
        /// 获取用户openId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("token")]
        [AllowAnonymous]
        public IHttpActionResult GetWeChatopenId(string code) {
            var url = $@"https://api.weixin.qq.com/sns/oauth2/access_token?appid={AppId}&secret={AppSecret}&code={code}&grant_type=authorization_code";
            var res = HttpGet(url);
            if (!string.IsNullOrWhiteSpace(res)) {
                var result = JsonConvert.DeserializeObject<dynamic>(res);

                if (res.Contains("errcode")) {
                    return Json(new { Result = result.errcode, Data = result.errmsg });
                }
                return Json(new { Result = 1, Data = result.openid });
            }
            return Json(new { Result = -1, ErrorMsg = "请求失败" });
        }
        /// <summary>
        /// 获取验签
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("signature")]
        [AllowAnonymous]
        public IHttpActionResult Signature(string url) {


            string timestamp = DateTime.Now.Ticks.ToString().Substring(0, 10);
            string noncestr = (DateTime.Now.Ticks-new Random().Next(1,9999999)).ToString();
            string signature = string.Empty;


            var token = CacheHelper.CacheReader("token")?.ToString();
            if (string.IsNullOrWhiteSpace(token)) {
                token = GetAccessToken();
            }
            if (string.IsNullOrWhiteSpace(token)) {
                return Json(new { Result = 0, error = "token为空" });
            }

            var ticket = CacheHelper.CacheReader("ticket")?.ToString();
            if (string.IsNullOrWhiteSpace(ticket)) {
                ticket = GetTickect(token);

            }
            if (string.IsNullOrWhiteSpace(ticket)) {
                return Json(new { Result = 0, error = "ticket为空" });
            }
            var uri = url.Replace("#", "");
            var temp = $@"jsapi_ticket={ticket}&noncestr={noncestr}&timestamp={timestamp}&url={uri}";
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(temp, "SHA1").ToLower();
            return Json(new {
                Result = 1,
                timestamp = timestamp,
                noncestr = noncestr,
                signature = signature,
                appId = AppId
            });

        }
        /// <summary>
        /// 获取验签
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("wechatimage")]
        [AllowAnonymous]
        public IHttpActionResult GetWeChatImage(string mediaId) {
            var pic = GetWxPic(mediaId);
            if (!string.IsNullOrWhiteSpace(pic)) {
                return Json(new {
                    Result = 1,
                    Data = $@"https://www.ujspace.com{pic}"
                });
            }
            return Json(new {
                Result = 0,
                ErrorMsg="获取图片失败"
            });
        }

        private static string SHA1(string text) {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        /// <summary>  
        /// 获取AccessToken  
        /// </summary>  
        /// <returns></returns>  
        protected string GetAccessToken() {
            string grant_type = "client_credential";
            string tokenUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                grant_type, AppId, AppSecret);
            var res = HttpGet(tokenUrl);
            if (!string.IsNullOrWhiteSpace(res)) {
                var result = JsonConvert.DeserializeObject<dynamic>(res);

                if (res.Contains("errcode")) {
                    return string.Empty;
                }
                CacheHelper.CacheWriter("token", result.access_token, 120);

                return result.access_token;
            }
            return string.Empty;
        }
        protected string GetTickect(string token) {
            string tokenUrl = $@"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={token}&type=jsapi";
            var res = HttpGet(tokenUrl);
            if (!string.IsNullOrWhiteSpace(res)) {
                var result = JsonConvert.DeserializeObject<dynamic>(res);
                if (!res.Contains("ticket")) {
                    return string.Empty;
                }
                CacheHelper.CacheWriter("ticket", result.ticket, 120);
                return result.ticket;
            }
            return string.Empty;

        }

        private string GetWxPic(string mediaId) {
            var token = GetAccessToken();
            var url = $"https://api.weixin.qq.com/cgi-bin/media/get?access_token={token}&media_id={mediaId}";
            string path = "";
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
                    if (response.StatusCode == HttpStatusCode.OK) {
                        string fileName = Guid.NewGuid().ToString("N") + ".jpeg";
                        path = "~/Files/";
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(path))) {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                        }
                        path = "/Files/" + fileName;
                        Stream responseStream = response.GetResponseStream();
                        BinaryReader br = new BinaryReader(responseStream);
                        FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.Create, FileAccess.Write);
                        const int buffsize = 1024;
                        byte[] bytes = new byte[buffsize];
                        int totalread = 0;

                        int numread = buffsize;
                        while (numread != 0) {
                            // read from source  
                            numread = br.Read(bytes, 0, buffsize);
                            totalread += numread;

                            // write to disk  
                            fs.Write(bytes, 0, numread);
                        }

                        br.Close();
                        fs.Close();

                        response.Close();

                    }
                    else {
                        response.Close();
                        path = "";
                    }

                }
            }
            catch (Exception) {
                path = "";
            }
            return path;
        }
    }
}