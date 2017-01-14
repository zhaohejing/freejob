using Job.Application.IService;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Application.Service {
    public class WorkService : BaseService, IWorkService {
        public WorkService( ) : base( ) {

        }

        public ErrEnum InsertWork(WorkInfo work, Tbl_SysUser user) {
            string inse = string.Empty;
            foreach (var item in work.WorkCates) {
                inse += $@"(@pid,{item},@pid,getdate(),0,@pid,getdate()),";
            }
            inse = inse.Trim(',');
            if (string.IsNullOrWhiteSpace(inse)) {
                return ErrEnum.IndustrysIsNull;
            }
            var tran = $@"
BEGIN TRAN Tran_InsertWork    --开始事务
DECLARE @tran_error int;
DECLARE @pid int;
SET @tran_error = 0;
SET @pid=0;
    BEGIN TRY 
    INSERT INTO Tbl_ParttimeJob( CreationTime)  VALUES(getdate()); SELECT @pid= @@identity;
        SET @tran_error = @tran_error + @@ERROR;
      INSERT INTO Tbl_ParttimeInfo(
ParttimeName,ParttimeArea,StartDate,EndDate,
StartTime,EndTime,CloseTime,Pay,PayState,ParttimeIntroduction,FromState,
	[State],NeedSum,NeedSex,Pid,CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime
)VALUES('{work.WorkName}',{work.WorkArea},'{work.StartDate}',
'{work.EndDate}',{work.StartTime},{work.EndTime},
'{work.CloseTime}',{work.Pay},{work.PayState},'{work.WorkIntroduction}',21,
(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已发布'),{work.NeedSum},
{work.NeedSex},@pid,{user.Id},getdate(),0,{user.Id},getdate())
        SET @tran_error = @tran_error + @@ERROR;
        INSERT INTO Tbl_ParttimeIndustry
(Pid,IndustryId,CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime)
VALUES {inse}
        SET @tran_error = @tran_error + @@ERROR;
    END TRY

BEGIN CATCH
    SET @tran_error = @tran_error + 1
END CATCH

IF(@tran_error > 0)
    BEGIN
         select 0;
        --执行出错，回滚事务
        ROLLBACK TRAN;
    END
ELSE
    BEGIN
        select 1;
        --没有异常，提交事务
        COMMIT TRAN;
    END";
            var b = (int)MySqlHelper.ExecuteScalar(System.Data.CommandType.Text, tran);
            return b > 0 ? ErrEnum.Ok : ErrEnum.Fail;

        }

        public CompanyOutPut CompanyDetail(int companyId) {
            var sql = $@"SELECT a.Id, a.CompanyName,c.SubName [Area],a.CompanyLogo,d.SubName [Industry]
  FROM Tbl_CompanyInfo AS a
   LEFT JOIN Tbl_CompanyIndustry AS b ON a.Id=b.Pid 
   LEFT JOIN Tbl_PublicSubEnum AS d ON b.IndustryId=d.Id
   LEFT JOIN Tbl_PublicSubEnum AS c ON a.CompanyArea=c.Id
WHERE a.Id={companyId}";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<CompanyTemp>(dt);
            CompanyOutPut output = new CompanyOutPut( );
            if (list != null && list.Count > 0) {
                foreach (var item in list) {
                    if (item.Id != output.Id) {
                        output.CompanyName = item.CompanyName;
                        output.CompanyLogo = item.CompanyLogo;
                        output.Area = item.Area;
                        output.Id = item.Id;
                    }
                    output.Industry = new List<string>( );

                    if (!string.IsNullOrWhiteSpace(item.Industry)) {
                        output.Industry.Add(item.Industry);
                    }
                }
                return output;
            }
            return null;
        }

        public WorkDetail WorkDetail(int workId, int userId) {
            var sql = $@" SELECT b.Id,b.ParttimeName [WorkName],d.SubName [WorkArea],b.Pay,e.SubName [PayState],f.CompanyName,jj.SubName [State],
b.CreationTime,b.StartDate,b.EndDate,b.StartTime,b.EndTime,b.CloseTime,b.ParttimeIntroduction,b.CreationOne,b.NeedSum,
h.SubName [NeedSex],m.JoinCount,CASE WHEN n.Pid IS NULL THEN 0 ELSE 1 end HadRegist,n.SubName
  FROM Tbl_ParttimeJob AS a INNER JOIN Tbl_ParttimeInfo AS b ON a.Id=b.Pid
  LEFT JOIN Tbl_PublicSubEnum AS d ON b.ParttimeArea=d.Id 
  LEFT JOIN Tbl_PublicSubEnum AS e ON b.PayState=e.Id 
  LEFT JOIN Tbl_CompanyInfo AS f ON b.CreationOne=f.CreationOne
 LEFT JOIN Tbl_PublicSubEnum AS jj ON jj.Id=b.[State]
  LEFT JOIN Tbl_PublicSubEnum AS h ON b.NeedSex=h.Id 
  LEFT JOIN (
SELECT ur.ParttimeId,COUNT(ur.Pid) JoinCount FROM Tbl_UserRegistion AS ur
WHERE ur.[State]=(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已通过')
 GROUP BY ur.ParttimeId
  ) m ON m.ParttimeId=b.Id 
  LEFT JOIN( SELECT ur.ParttimeId,ur.Pid,ur.[State],pee.SubName
   FROM Tbl_UserRegistion AS ur 
   left join Tbl_PublicSubEnum pee on ur.[state]=pee.Id WHERE ur.Pid={userId} AND ur.ParttimeId={workId}
   ) n ON b.Id=n.ParttimeId
where 1=1
 and b.Id={workId}";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var model = ConvertToModel<WorkOutput>(dt).FirstOrDefault( );
            if (model != null) {
                var state = dt.Rows[0]["SubName"].ToString();
                switch (state) {
                    case "已通过":
                        model.CanRegist = 2;
                        
                        break;
                    case "未通过":
                        model.CanRegist = 0;
                        break;
                    case "已驳回":
                        model.CanRegist = 0;
                        break;
                    case "已报名":
                        model.CanRegist = 1;
                        break;
                    default:
                        model.CanRegist = 0;
                        break;
                }
                model.PayState = model.PayState.Replace("结", "");

                var worksql = $@"SELECT c.Id,c.SubName[WorkName]
  FROM  Tbl_ParttimeIndustry AS b 
LEFT JOIN Tbl_PublicSubEnum AS c ON b.IndustryId=c.Id
WHERE b.Pid={workId}";
                var work = ConvertToModel<WorkTemp>(MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, worksql));
                if (work != null && work.Count > 0) {
                    model.WorkCate = work.Select(c => c.WorkName).ToList( );
                }
                WorkDetail detail = new Application.WorkDetail( );
                detail.WorkOutput = model;
                detail.CompanyOutPut = CompanyDetail(model.CreationOne);
                return detail;
            }
            return null;
        }
        /// <summary>
        /// 自由职业者,应只搜索已发布工作
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="totalCount"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IEnumerable<BaseWorkOutput> SearchWork(WorkFilter filter, out int totalCount) {
            var sql = $@"SELECT distinct b.Id,b.ParttimeName [WorkName],c.temp,
d.SubName [WorkArea],b.Pay,e.SubName [PayState],f.CompanyName,b.CreationTime,g.SubName [State]
  FROM Tbl_ParttimeJob AS a INNER JOIN Tbl_ParttimeInfo AS b ON a.Id=b.Pid
  LEFT JOIN (SELECT b.IndustryId, b.Pid, c.Id,c.SubName temp
  FROM  Tbl_ParttimeIndustry AS b 
LEFT JOIN Tbl_PublicSubEnum AS c ON b.IndustryId=c.Id) AS c ON c.Pid=a.Id
  LEFT JOIN Tbl_PublicSubEnum AS d ON b.ParttimeArea=d.Id 
  LEFT JOIN Tbl_PublicSubEnum AS e ON b.PayState=e.Id 
 LEFT JOIN Tbl_PublicSubEnum AS g ON b.[State]=g.Id

  LEFT JOIN Tbl_CompanyInfo AS f ON b.CreationOne=f.CreationOne where   
b.[State]=(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已发布')
";
            if (filter.Area > 0) {
                sql += $" and ( b.ParttimeArea={filter.Area} or b.ParttimeArea=-1)";
            }          
            if (filter.Cate > 0) {
                sql += $" and ( c.IndustryId={filter.Cate} or c.IndustryId=-1)";
            }
            if (filter.CompanyId > 0) {
                sql += $" and f.Pid={filter.CompanyId}";
            }
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<BaseWorkOutput>(dt);
            var res = new List<BaseWorkOutput>();
            foreach (var a in list) {
                if (res.Any(e=>e.Id==a.Id)) {
                    continue;
                }
                a.WorkCate =
              list.Where(e => e.Id == a.Id).Select(w => w.temp).ToList();
                res.Add(a);
            }
            if (res == null || res.Count < 0) {
                totalCount = 0;
                return null;
            }
            totalCount = res.Count;
            return res.OrderByDescending(c => c.CreationTime).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
        }
        public IEnumerable<T> GetPositionList<T>( ) where T : class, new() {
            var sql = @"SELECT b.Id,b.SubName [Name]
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '擅长工作' and len(b.LevelCode) > 14";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public IEnumerable<T> GetArea<T>( ) where T : class, new() {
            if (RedisHelper.HashExists("Area", "Level")) {

                return RedisHelper.HashGet<IList<T>>("Area", "Level");
            }

            var sql = @"SELECT b.Id,b.SubName [Name],b.LevelCode
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '企业所在区域' ";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<T>(dt);
            RedisHelper.HashDelete("Area", "Level");
            RedisHelper.HashSet<IList<T>>("Area", "Level", list);
            return list;
        }
        /// <summary>
        /// 获取工作年限
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetWorkYear<T>( ) where T : class, new() {
            if (RedisHelper.HashExists("WorkYear", "Year")) {

                return RedisHelper.HashGet<IList<T>>("WorkYear", "Year");
            }

            var sql = @"SELECT b.Id,b.SubName [Name],b.LevelCode
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '工作年限' ";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<T>(dt);
            RedisHelper.HashDelete("WorkYear", "Year");
            RedisHelper.HashSet<IList<T>>("WorkYear", "Year", list);
            return list;
        }
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetGenders<T>( ) where T : class, new() {
            if (RedisHelper.HashExists("Gender", "Sex")) {

                return RedisHelper.HashGet<IList<T>>("Gender", "Sex");
            }

            var sql = @"SELECT b.Id,b.SubName [Name],b.LevelCode
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '性别' ";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<T>(dt);
            RedisHelper.HashDelete("Gender", "Sex");
            RedisHelper.HashSet<IList<T>>("Gender", "Sex", list);
            return list;
        }


        public IEnumerable<Parttime> GetWorks(string filter)  {
            if (RedisHelper.HashExists("Work", "Part")) {
                var t= RedisHelper.HashGet<IList<Parttime>>("Work", "Part");
                return t.Where(c => c.ParttimeName.Contains(filter));
            }

            var sql = $@"SELECT Id, ParttimeName
  FROM Tbl_ParttimeInfo  ";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);

            var a = ConvertToModel<Parttime>(dt);
            RedisHelper.HashDelete("Work", "Part");

            RedisHelper.HashSet<IList<Parttime>>("Work", "Part", a);

            return a.Where(c=>c.ParttimeName.Contains(filter));
        }
        public IEnumerable<T> GetWorks<T>( ) where T : class, new() {
           // RedisHelper.HashDelete("Work", "Level");
            if (RedisHelper.HashExists("Work", "Level")) {
                return RedisHelper.HashGet<IList<T>>("Work", "Level");
            }
            var sql = @"SELECT b.Id,b.SubName [Name],b.LevelCode
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '擅长工作'";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);

            var list = ConvertToModel<T>(dt);
            RedisHelper.HashDelete("Work", "Level");

            RedisHelper.HashSet<IList<T>>("Work", "Level", list);

            return list;
        }


        public IEnumerable<RegistWorkOutput> GetPublishList(BaseFilter filter, out int totalCount, int userid) {
            var sql = $@"SELECT distinct b.Id,b.ParttimeName [WorkName],
d.SubName [WorkArea],b.Pay,e.SubName [PayState],f.CompanyName,b.CreationTime,g.SubName [State],case when isnull(pa.Regist,0)>0 then 1 else 0 end IsHaveNew
  FROM Tbl_ParttimeJob AS a INNER JOIN Tbl_ParttimeInfo AS b ON a.Id=b.Pid
  LEFT JOIN Tbl_ParttimeIndustry AS c ON c.Pid=a.Id
  LEFT JOIN Tbl_PublicSubEnum AS d ON b.ParttimeArea=d.Id 
  LEFT JOIN Tbl_PublicSubEnum AS e ON b.PayState=e.Id 
 LEFT JOIN Tbl_PublicSubEnum AS g ON b.[State]=g.Id
  LEFT JOIN Tbl_CompanyInfo AS f ON b.CreationOne=f.CreationOne
  LEFT JOIN (  select count(Pid) Regist,ParttimeId from Tbl_UserRegistion 
  where IsDeleted<>1 and [State]=(select top 1 Id from Tbl_PublicSubEnum where SubName='已报名')
  group by ParttimeId) pa on b.Id=pa.ParttimeId where f.Pid={userid}";
         
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<RegistWorkOutput>(dt);
            if (list == null || list.Count < 0) {
                totalCount = 0;
                return null;
            }
            totalCount = list.Count;
            return list.OrderByDescending(c => c.CreationTime).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
        }

        public IEnumerable<BaseWorkOutput> GetCheckedList(BaseFilter filter, out int totalCount, int userid) {
            var sql = $@"SELECT distinct b.Id,b.ParttimeName [WorkName],
d.SubName [WorkArea],b.Pay,e.SubName [PayState],f.CompanyName,b.CreationTime,g.SubName [State]
  FROM Tbl_ParttimeJob AS a INNER JOIN Tbl_ParttimeInfo AS b ON a.Id=b.Pid
  LEFT JOIN Tbl_ParttimeIndustry AS c ON c.Pid=a.Id
  LEFT JOIN Tbl_PublicSubEnum AS d ON b.ParttimeArea=d.Id 
  LEFT JOIN Tbl_PublicSubEnum AS e ON b.PayState=e.Id 
 LEFT JOIN Tbl_PublicSubEnum AS g ON b.[State]=g.Id
  LEFT JOIN Tbl_CompanyInfo AS f ON b.CreationOne=f.CreationOne
  LEFT JOIN (  select count(Pid) Regist,ParttimeId from Tbl_UserRegistion 
  where IsDeleted<>1 and [State]=(select top 1 Id from Tbl_PublicSubEnum where SubName='已通过')
  group by ParttimeId) pa on b.Id=pa.ParttimeId where f.Pid={userid} and pa.Regist>0 ";

            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<BaseWorkOutput>(dt);
            if (list == null || list.Count < 0) {
                totalCount = 0;
                return null;
            }
            totalCount = list.Count;
            return list.OrderByDescending(c => c.CreationTime).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public int CloseWork(int workId, int userId) {
            var sql = $@"UPDATE Tbl_ParttimeInfo
SET
	[State] = (SELECT TOP 1 pse.Id FROM Tbl_PublicSubEnum AS pse WHERE pse.SubName='已关闭'),

	UpdateOne = {userId},
	updateTime = GETDATE() WHERE Id={workId}";
            return MySqlHelper.ExecteNonQuery(System.Data.CommandType.Text, sql);
        }

    
        public IEnumerable<T> GetPayState<T>( ) where T : class, new() {
            //if (RedisHelper.HashExists("PayState", "PayState")) {

            //    return RedisHelper.HashGet<IList<T>>("PayState", "PayState");
            //}

            var sql = @"SELECT b.Id,b.SubName [Name],b.LevelCode
  FROM Tbl_PublicEnum AS a INNER JOIN Tbl_PublicSubEnum AS b ON a.Id = b.Pid
WHERE a.EnumName = '结算方式' ";
            var dt = MySqlHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
            var list = ConvertToModel<T>(dt);
            RedisHelper.HashDelete("PayState", "PayState");
            RedisHelper.HashSet<IList<T>>("PayState", "PayState", list);
            return list;
        }
        public class Parttime {
            public int PartId { get; set; }
            public string ParttimeName { get; set; }
        }
    }
}
