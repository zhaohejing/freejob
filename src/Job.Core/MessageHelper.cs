using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core {
   public static class MessageHelper {
        /// <summary>
        /// 短信服务接口
        /// </summary>
        private static string url = System.Configuration.ConfigurationManager.AppSettings["SMSWebService"];
        /// <summary>
        /// 短信key
        /// </summary>
        private static string key = System.Configuration.ConfigurationManager.AppSettings["SMSKey"];
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobilePhone">手机号 多个用,分割</param>
        /// <param name="smsContent">短信内容</param>
        /// <returns></returns>
        public static string SendMessage(string mobilePhone, string smsContent) {
            //参数
            string[] args = new string[5];
            //key
            args[0] = key;
            //手机号
            args[1] = mobilePhone;
            //短信内容
            args[2] = smsContent;
            //发送时间
            args[3] = DateTime.Now.ToString();
            //子号码
            args[4] = "00";
            object result = WebServiceHelper.InvokeWebService(url, "MassSend", args);//路径，方法名，参数
            if (result != null) {
                //结果不为空
                if (result.ToString().Length > 0) {

                }
                return result.ToString();
            }
            return string.Empty;
        }

        public static string CreateNum() {
            //数组实例
            ArrayList MyArray = new ArrayList();
            //随机数实例
            Random random = new Random();
            //字符串
            string str = string.Empty;

            //循环的次数   
            int Nums = 6;
            //判断nums》0
            while (Nums > 0) {
                //取1-9的数字
                int i = random.Next(1, 9);
                ////数组不为空
                //if (!MyArray.Contains(i))
                //{
                //数组数量《6
                if (MyArray.Count < 6) {
                    MyArray.Add(i);
                }
                //}
                Nums -= 1;
            }
            //循环取数字
            for (int j = 0; j <= MyArray.Count - 1; j++) {
                str += MyArray[j].ToString();
            }

            return str;
        }
    }
}
