using Job.Application.IService;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public string SendMessage(string mobile) {
            return string.Empty;
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
