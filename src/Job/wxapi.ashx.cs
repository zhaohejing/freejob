using Job.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Job {
    /// <summary>
    /// wxapi 的摘要说明
    /// </summary>
    public class wxapi : IHttpHandler {
        protected Logger _logger=Logger.Singleton;
        public void ProcessRequest(HttpContext context) {
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST") {
                using (Stream stream = HttpContext.Current.Request.InputStream) {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString)) {
                   // Execute(postString);
                }
            }
            else {
                Auth(); //微信接入的测试
            }
        }

        private void Auth() {
            string token = "ujspace";//从配置文件获取Token
            if (string.IsNullOrEmpty(token)) {
                _logger.Error($"{token}配置项没有配置！");
              //  LogTextHelper.Error(string.Format("WeixinToken 配置项没有配置！"));
            }

            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
          //  _logger.Debug($"{echoString}---{signature}---{timestamp}---{nonce}");
            if (CheckSignature(token, signature, timestamp, nonce)) {
                if (!string.IsNullOrEmpty(echoString)) {
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }
        public bool CheckSignature(string token, string signature, string timestamp, string nonce) {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}