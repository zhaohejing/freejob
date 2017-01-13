using HuaTong.General.Utility;
using Job.Application;
using Job.Application.IService;
using Job.Application.Service;
using Job.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using static Job.Application.Service.BaseService;

namespace Job {
    public class WebApiApplication : System.Web.HttpApplication {
        public string AppId { get; set; } = System.Configuration.ConfigurationManager.AppSettings["AppId"];
        public string AppSecret { get; set; } = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
        private ICompanyService _companyService;
        protected void Application_Start() {
            _companyService = new CompanyService();
            //api 注册
            GlobalConfiguration.Configure(WebApiConfig.Register);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("/log4net.config")));
            var lc = DateTime.Today.AddHours(3);
            var now = DateTime.Now;
            var cz = lc - now;
            var temp = cz.TotalMilliseconds;
            if (temp < 0) {
                temp = (DateTime.Today.AddHours(27) - now).TotalMilliseconds;
            }


            //  ExecuteSomething(null, null);
            System.Timers.Timer timer = new System.Timers.Timer(temp);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteSomething);
            //AddCount是一个方法，此方法就是每个6分钟而做的事情  
            timer.AutoReset = true;

            timer.Enabled = true;

            timer.Start();
        }
        /// <summary>
        /// 自动同步redis中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteSomething(object sender, ElapsedEventArgs e) {

            if (RedisHelper.HashExists("Area", "Level")) {

                RedisHelper.HashDelete("Area", "Level"); ;
            }
            if (RedisHelper.HashExists("PayState", "PayState")) {
                RedisHelper.HashDelete("PayState", "PayState");
            }
            if (RedisHelper.HashExists("Gender", "Sex")) {

                RedisHelper.HashDelete("Gender", "Sex");
            }


            //从文件读取json  公司 和 职位
            var companyPath = HttpRuntime.AppDomainAppPath + "/Files/company.json";
            if (!FileHelper.IsExistFile(companyPath)) {
                return;
            }

            var companys = FileHelper.FileToString(companyPath);
            var list = JsonConvert.DeserializeObject<List<TempSerialize>>(companys);
            RedisHelper.HashDelete("Company", "CompanyName");
            RedisHelper.HashSet<List<TempSerialize>>("Company", "CompanyName", list);

            IWorkService _workService = new WorkService();
            var plist = _workService.GetPositionList<TempSerialize>();
            RedisHelper.HashDelete("Work", "WorkName");
            RedisHelper.HashSet<List<TempSerialize>>("Work", "WorkName", plist.ToList());

        }
        ///// <summary>
        ///// 从微信获取图片并保存
        ///// </summary>
        ///// <returns></returns>
        //public async Task DownLownImage() {
        //    var list = _companyService.GetCompanyInfos();
        //    if (list != null && list.Count > 0) {
        //        list = list.Where(c => !c.CompanyLogo.Contains("https://www.ujspace.com")).ToList();
        //        var tt = SaveAsWebImg(list);
        //        if (tt!=null&&tt.Count>0) {
        //            _companyService.UpdateCompanyInfos(tt);
        //        }
        //    }
        //}
        ///// <summary>  
        ///// 下载网站图片  
        ///// </summary>  
        ///// <param name="picUrl"></param>  
        ///// <returns></returns>  
        //public List<temp> SaveAsWebImg(List<Tbl_CompanyInfo> list) {
        //    List<temp> result = new List<temp>();

        //    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/Files/";  //目录  
        //    try {

        //        WebClient webClient = new WebClient();
        //        foreach (var item in list) {
        //            if (!string.IsNullOrWhiteSpace(item.CompanyLogo)) {
        //                var res = GetWxPic(item.CompanyLogo);
        //                if (!string.IsNullOrWhiteSpace(res)) {
        //                    result.Add(new temp(item.Id, $@"https://www.ujspace.com{res}"));

        //                }
        //            }

        //        }
        //        return result;

        //    }
        //    catch {
        //        return new List<temp>();
        //    }

        //}
        //protected string HttpGet(string Url, string postDataStr = "") {
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
        //    request.Method = "GET";
        //    request.ContentType = "text/html;charset=UTF-8";


        //    //读取返回消息
        //    string res = string.Empty;
        //    try {
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //        res = reader.ReadToEnd();
        //        reader.Close();
        //    }
        //    catch (Exception ex) {
        //    }

        //    return res;
        //}
   
    }
  
}
