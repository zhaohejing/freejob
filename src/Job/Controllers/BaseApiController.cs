using Job.Application;
using Job.Application.IService;
using Job.Application.Service;
using Job.Core;
using Job.Providers;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Job.Controllers {
    [WebApiExceptionFilter]
    [RequestAuthorize]
    public abstract class BaseApiController : ApiController {

        protected Logger _logger;
        protected IUserService _userService;
        public Tbl_SysUser CurrentUser;
        /// <summary>
        /// 基类控制器
        /// </summary>
        public BaseApiController() {
            _userService = new UserService();
            var a = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(a)) {
                var parms = a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var t = _userService.GetUserInfo(parms[1]);
                if (t != null) {
                    CurrentUser = t;
                }
                else {
                    RedisHelper.KeyDelete(parms[1]);
                }
            }
            _logger = Logger.Singleton;
        }
        protected string HttpGet(string Url, string postDataStr="") {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";


            //读取返回消息
            string res = string.Empty;
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex) {
                _logger.Error(ex.Message, ex);

            }

            return res;
        }
    }
}