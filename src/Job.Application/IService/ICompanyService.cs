using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Job.Application.Service.BaseService;

namespace Job.Application.IService {
   public interface ICompanyService:IBaseService {
        CompanyWorkUsers CompanyRegistUsers(int workId,string state);
        BaseUser GetRegistUserInfo(int userId, int workId);
        int VilidateRegistUser(int userId, int workId);

        int NoPassRegistUser(int userId, int workId);
        List<Tbl_CompanyInfo> GetCompanyInfos();
        int UpdateCompanyInfos(List<temp> list);
    }
}
