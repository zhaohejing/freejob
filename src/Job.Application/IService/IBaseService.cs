using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Application.IService {
 public   interface IBaseService {
        TEntity MapEntity<TEntity>(SqlDataReader reader) where TEntity : class, new();
        List<T> ConvertToModel<T>(DataTable dt) where T : class, new();
        bool VilidateCode(string phone, string code);
    }
}
