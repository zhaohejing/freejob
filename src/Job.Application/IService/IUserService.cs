using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Job.Application.Service.BaseService;

namespace Job.Application.IService {
   public interface IUserService:IBaseService {
        Tbl_SysUser GetUserInfo(string token);
        ErrEnum RegisterUserInfo(UserInfo user, Tbl_SysUser info);
        ErrEnum RegisterCompanyInfo(CompanyInfo company, Tbl_SysUser info);
        string SendMessage(MessageModel model);
        ErrEnum RegisterWeChat(string token);
        ErrEnum RegistWork(int userId, int workId);
        ErrEnum DeleteRegistWork(int userId, int workId);
        IEnumerable<RegistWorkOutPut> GetRegistWorks(BaseFilter filter, Tbl_SysUser user, out int count);
    }
}
