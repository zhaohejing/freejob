using Job.Application.IService;
using Job.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Job.Application.Service {
    public abstract class BaseService : IBaseService {
        protected Logger _logger;

        public BaseService( ) {
            _logger = Logger.Singleton;
        }
        public TEntity MapEntity<TEntity>(SqlDataReader reader) where TEntity : class, new() {
            try {
                var props = typeof(TEntity).GetProperties( );
                var entity = new TEntity( );
                foreach (var prop in props) {
                    if (prop.CanWrite) {
                        try {

                            var index = reader.GetOrdinal(prop.Name);
                            var data = reader.GetValue(index);
                            if (data != DBNull.Value) {
                                prop.SetValue(entity, Convert.ChangeType(data, prop.PropertyType), null);
                            }
                        }
                        catch (IndexOutOfRangeException) {
                            continue;
                        }
                    }
                }
                return entity;
            }
            catch {
                return null;
            }
        }
        public List<T> ConvertToModel<T>(DataTable dt) where T : class, new() {
            // 定义集合    
            List<T> ts = new List<T>( );

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows) {
                T t = new T( );
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType( ).GetProperties( );
                foreach (PropertyInfo pi in propertys) {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName)) {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        private string PushToWeb(string weburl, string data, Encoding encode) {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), encode);
            return aspx.ReadToEnd();
        }
        protected  string CreateNum() {
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
        public string SendMessage(string mobile,string content) {
            StringBuilder sms = new StringBuilder();
            sms.AppendFormat("name={0}", "15010286102");
            sms.AppendFormat("&pwd={0}", "DD9E7D5E66053B0170E4E43D52CF");//登陆平台，管理中心--基本资料--接口密码（28位密文）；复制使用即可。
            sms.AppendFormat("&content={0}", content);
            sms.AppendFormat("&mobile={0}", mobile);
            sms.AppendFormat("&sign={0}", "天津市共享时代网络科技有限责任公司");// 公司的简称或产品的简称都可以
            sms.Append("&type=pt");
            string resp = PushToWeb("http://web.wasun.cn/asmx/smsservice.aspx", sms.ToString(), Encoding.UTF8);
           
            return resp;
        }
        public bool VilidateCode(string phone, string code) {
            var sql = $@"SELECT TOP 1 vc.VerifyCode
  FROM Tbl_VerifyCode AS vc WHERE vc.Mobile='{phone}' AND vc.VerifyType=1 and GETDATE()<=DATEADD(n,30,vc.CreationTime) 
ORDER BY vc.CreationTime desc";
            var model = MySqlHelper.ExecuteScalar(CommandType.Text, sql);
            if (model == null || !model.ToString( ).Equals(code)) {
                return false;
            }
            return true;
        }


        public enum ErrEnum {
            HadRegistWork = 1,
            Ok = 2,
            Fail = 3,
            VilidateMobileFail = 4,
            WorksIsNull = 5,
            ProjectIsNull = 6,
            SendMessageOk = 7,
            SendMessageFail = 8,
            IndustrysIsNull = 9,
            NoRegistWork = 10
        }
        public class temp {
            public temp(int id, string url) {
                Id = id; Url = url;
            }
            public int Id { get; set; }
            public string Url { get; set; }
        }

    }
}
