using Job.Application.IService;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Application.Service
{
    public class CompanyService : BaseService, ICompanyService
    {

        /// <summary>
        /// 获取公司下 报名的用户信息列表
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public CompanyWorkUsers CompanyRegistUsers(int workId, string state)
        {
            var sql = $@"SELECT a.Id, b.ParttimeName,b.Pay,e.SubName PayState,d.SubName [State]
  FROM Tbl_ParttimeJob AS a LEFT JOIN Tbl_ParttimeInfo AS b ON a.Id=b.Pid
  LEFT JOIN Tbl_ParttimeIndustry AS c ON b.Pid=c.Pid 
  LEFT JOIN Tbl_PublicSubEnum AS d ON c.IndustryId=d.Id
  LEFT JOIN Tbl_PublicSubEnum AS e ON b.PayState=e.Id
WHERE a.Id={workId}";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var temp = ConvertToModel<CompanyWork>(dt);
            var result = new CompanyWorkUsers() { CompanyWork = new CompanyWork(), WorkUsers = new List<WorkUsers>() };
            if (temp != null && temp.Count > 0)
            {
                result.CompanyWork = temp.FirstOrDefault();
                result.CompanyWork.Industrys = temp.Select(c => c.State).Distinct().ToList();
            }
            var usql = $@"SELECT a.ParttimeId,c.Pid,c.Name,f.SubName WorkYear,d.SubName[Sex],c.Introduction,e.SubName [State]
  FROM Tbl_UserRegistion AS a 
LEFT JOIN Tbl_SysUser AS b ON a.Pid=b.Id
LEFT JOIN Tbl_UserInfo AS c ON b.Id=c.Pid
LEFT JOIN Tbl_PublicSubEnum AS d ON c.Sex=d.Id
LEFT JOIN Tbl_PublicSubEnum AS e ON a.[State]=e.Id
LEFT JOIN Tbl_PublicSubEnum AS f ON c.WorkYear=f.Id
WHERE a.ParttimeId={workId} and e.SubName='{state}' ";
            var ddt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, usql);
            var userlist = ConvertToModel<WorkUsers>(ddt);
            result.WorkUsers = userlist;
            return result;
        }

        /// <summary>
        /// 获取公司下 报名的用户信息详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        public BaseUser GetRegistUserInfo(int userId, int workId)
        {
            var sql = $@"SELECT a.Id,b.Name,e.SubName WorkYear,b.Introduction,d.SubName [State],b.Mail,b.phone
  FROM Tbl_SysUser AS a
 LEFT JOIN Tbl_UserInfo AS b ON a.Id=b.Pid
 LEFT JOIN Tbl_UserRegistion AS c ON a.Id=c.Pid
 LEFT JOIN Tbl_PublicSubEnum AS d ON c.[State]=d.id
 LEFT JOIN Tbl_PublicSubEnum AS e ON b.WorkYear=e.id
WHERE a.Id={userId} AND c.ParttimeId={workId}";
            var userinfo = ConvertToModel<BaseUser>(MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql)).FirstOrDefault();
            if (userinfo == null)
            {
                return null;
            }
            var worksql = $@"SELECT b.Id,b.SubName [WorkName]
  FROM Tbl_UserWork AS a LEFT JOIN Tbl_PublicSubEnum AS b ON a.WorkId=b.id WHERE a.Pid={userId}";
            var work = ConvertToModel<WorkTemp>(MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, worksql));
            userinfo.Works = work.Select(c => c.WorkName);
            var project = $@"SELECT a.JobName [ProjectName],
a.PositionName [positionName],a.JobIntroduction [ProjectIntroduction] FROM Tbl_UserProject AS a WHERE a.Pid={userId}";
            var projects = ConvertToModel<Project>(MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, project));
            userinfo.Projects = projects;
            return userinfo;
        }


        public int VilidateRegistUser(int userId, int workId)
        {
            var sql = $@"UPDATE Tbl_UserRegistion SET [State] =(  select top 1 id from Tbl_PublicSubEnum where SubName='已通过') WHERE ParttimeId={workId} AND Pid={userId}";
            return MySqlHelper.ExecteNonQuery(System.Data.CommandType.Text, sql);
        }
        public int NoPassRegistUser(int userId, int workId)
        {
            var sql = $@"UPDATE Tbl_UserRegistion SET [State] =(select top 1 id from Tbl_PublicSubEnum where SubName='未通过') WHERE ParttimeId={workId} AND Pid={userId}";
            return MySqlHelper.ExecteNonQuery(System.Data.CommandType.Text, sql);
        }
        public List<Tbl_CompanyInfo> GetCompanyInfos() {
            var sql = $@"select * from  Tbl_CompanyInfo";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
           return ConvertToModel<Tbl_CompanyInfo>(dt);
        }
        public int UpdateCompanyInfos(List<temp> list) {
            var sql =string.Empty;
            foreach (var item in list) {
                sql += $@"update Tbl_CompanyInfo set CompanyLogo='{item.Url}' where  Id={item.Id};";
            }
            if (!string.IsNullOrWhiteSpace(sql)) {
                return MySqlHelper.ExecteNonQuery(CommandType.Text, sql);
            }
            return 0;
        }




    }

}
