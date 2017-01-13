using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Job.Application.Service.BaseService;
using static Job.Application.Service.WorkService;

namespace Job.Application.IService {
   public interface IWorkService:IBaseService {
        ErrEnum InsertWork(WorkInfo work,Tbl_SysUser user);
        IEnumerable<RegistWorkOutput> GetPublishList(BaseFilter filter, out int totalCount, int userid);
        IEnumerable<BaseWorkOutput> GetCheckedList(BaseFilter filter, out int totalCount, int userid);
        WorkDetail WorkDetail(int workId, int userId);
        int CloseWork(int workId, int userId);
        IEnumerable<BaseWorkOutput> SearchWork(WorkFilter filter, out int totalCount);
        IEnumerable<T> GetPositionList<T>() where T : class, new();
        IEnumerable<Parttime> GetWorks(string filter);

         IEnumerable<T> GetArea<T>() where T : class, new();
         IEnumerable<T> GetWorks<T>() where T : class, new();
        IEnumerable<T> GetWorkYear<T>() where T : class, new();
        IEnumerable<T> GetGenders<T>() where T : class, new();
        IEnumerable<T> GetPayState<T>( ) where T : class, new();

    }
}
