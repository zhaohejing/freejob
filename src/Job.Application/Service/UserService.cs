using Job.Application.IService;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Application.Service {
    public class UserService : BaseService, IUserService {

        public UserService( ) : base( ) {

        }
        /// <summary>
        /// 注册微信信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ErrEnum RegisterWeChat(string token) {
            var sql = $@"INSERT INTO Tbl_SysUser(WeChatToken,CreationTime) VALUES('{token}',GETDATE());";
            var res = MySqlHelper.ExecteNonQuery(CommandType.Text, sql);
            return res > 0 ? ErrEnum.Ok : ErrEnum.Fail;
        }
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Tbl_SysUser GetUserInfo(string token) {
            var sql = $@"
select a.*,case  when b.Id is null then 0 else 1 end HadRegisterUser ,case when   c.Id  is
null then 0 else 1 end  HadRegisterCompany
 from Tbl_SysUser a left join Tbl_UserInfo b on a.Id=b.Pid left join Tbl_CompanyInfo c
on a.Id=c.Pid
where a.WeChatToken='{token}'";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            var model = ConvertToModel<Tbl_SysUser>(dt).FirstOrDefault( );
            return model;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ErrEnum RegisterUserInfo(UserInfo user, Tbl_SysUser info) {
            var v = VilidateCode(user.Phone, user.Code);
            if (!v) {
                return ErrEnum.VilidateMobileFail;
            }
            string work = string.Empty;
            foreach (var item in user.Works) {
                work += $@"(@pid,getdate(),0,@pid,{item}),";
            }
            work = work.Trim(',');
            if (string.IsNullOrWhiteSpace(work)) {
                return ErrEnum.WorksIsNull;
            }
            string project = string.Empty;
            foreach (var item in user.Projects) {
                project += $@"(@pid,'{item.ProjectName}','{item.positionName}','{item.ProjectIntroduction}',@pid,getdate(),0,@pid,getdate()),";
            }
            project = project.Trim(',');
            if (string.IsNullOrWhiteSpace(project)) {
                return ErrEnum.ProjectIsNull;
            }
            string userinfo = string.Empty;
            if (info.HadRegisterCompany > 0) {
                userinfo = $"SELECT @pid=su.Id FROM Tbl_SysUser AS su WHERE su.WeChatToken='{info.WeChatToken}'";
            }
            else if (info.Id > 0) {
                userinfo = $"select @pid={info.Id}";
            }
            else {
                userinfo = $" INSERT INTO Tbl_SysUser(WeChatToken,CreationTime) VALUES('{info.WeChatToken}',GETDATE());SELECT @pid= @@identity;";
            }

            var sql = $@"
BEGIN TRAN Tran_InsertUser    --开始事务
DECLARE @tran_error int;
DECLARE @pid int;
SET @tran_error = 0;
SET @pid=0;
    BEGIN TRY 
    --插入用户信息表
    {userinfo}
        SET @tran_error = @tran_error + @@ERROR;
        --插入用户信息表
     INSERT INTO Tbl_UserInfo
(Pid,Name,Mail,Sex,WorkYear,Area,Introduction,phone,[State],CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime)
VALUES(@pid,'{user.Name}','{user.Mail}','{user.Sex}','{user.WorkYear}','{user.Area}','{user.Introduction}',
'{user.Phone}',1,@pid,getdate(),0,@pid,getdate());
  SET @tran_error = @tran_error + @@ERROR;
--插入擅长工作表
INSERT INTO Tbl_UserWork
(CreationOne,CreationTime,IsDeleted,Pid,WorkId)
VALUES {work};
  SET @tran_error = @tran_error + @@ERROR;
--插入项目表
INSERT INTO Tbl_UserProject
(Pid,JobName,PositionName,JobIntroduction,CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime)
VALUES {project};
        SET @tran_error = @tran_error + @@ERROR;
    END TRY

BEGIN CATCH
    SET @tran_error = @tran_error + 1
END CATCH

IF(@tran_error > 0)
    BEGIN
        --执行出错，回滚事务
        ROLLBACK TRAN;
    END
ELSE
    BEGIN
        --没有异常，提交事务
        COMMIT TRAN;
    END";
            var res = MySqlHelper.ExecteNonQuery(System.Data.CommandType.Text, sql);
            return res > 0 ? ErrEnum.Ok : ErrEnum.Fail;
        }
        /// <summary>
        /// 注册公司信息
        /// </summary>
        /// <param name="company"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ErrEnum RegisterCompanyInfo(CompanyInfo company, Tbl_SysUser info) {
            var v = VilidateCode(company.Phone, company.Code);
            if (!v) {
                return ErrEnum.VilidateMobileFail;
            }
            string project = string.Empty;
            foreach (var item in company.CompanyIndustrys) {
                project += $@"(@pid,{item},@pid,getdate(),0,@pid,getdate()),";
            }
            project = project.Trim(',');
            if (string.IsNullOrWhiteSpace(project)) {
                return ErrEnum.ProjectIsNull;
            }
            string userinfo = string.Empty;
            if (info.HadRegisterUser > 0) {
                userinfo = $"SELECT @pid=su.Id FROM Tbl_SysUser AS su WHERE su.WeChatToken='{info.WeChatToken}'";
            }
            else if (info.Id > 0) {
                userinfo = $"select @pid={info.Id}";
            }
            else {
                userinfo = $" INSERT INTO Tbl_SysUser(WeChatToken,CreationTime) VALUES('{info.WeChatToken}',GETDATE());SELECT @pid= @@identity;";
            }
            var sql = $@"
--开始事务
BEGIN TRAN Tran_InsertCompany    
DECLARE @tran_error int;
DECLARE @pid int;
SET @tran_error = 0;
SET @pid=0;
    BEGIN TRY 
    --插入公司系统表
    {userinfo}
        SET @tran_error = @tran_error + @@ERROR;
        --插入公司信息表
  INSERT INTO Tbl_CompanyInfo
(CompanyName,CompanyArea,CompanyAddress,CompanyIntroduction,Mail,Phone,CompanyLogo,CardImage,
[State],Pid,CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime)
VALUES('{company.CompanyName}',{company.CompanyArea},'{company.CompanyAddress}','{company.CompanyIntroduction}',
'{company.Email}','{company.Phone}','{company.CompanyLogo}','{company.CardImage}',1,@pid,@pid,getdate(),0,@pid,getdate());
  SET @tran_error = @tran_error + @@ERROR;
  
--插入公司行业表
INSERT INTO Tbl_CompanyIndustry
(Pid,IndustryId,CreationOne,CreationTime,IsDeleted,UpdateOne,updateTime)
VALUES {project}
        SET @tran_error = @tran_error + @@ERROR;
    END TRY

BEGIN CATCH
    SET @tran_error = @tran_error + 1
END CATCH

IF(@tran_error > 0)
    BEGIN
        --执行出错，回滚事务
        ROLLBACK TRAN;
    END
ELSE
    BEGIN
        --没有异常，提交事务
        COMMIT TRAN;
    END";
            var res = MySqlHelper.ExecteNonQuery(System.Data.CommandType.Text, sql);
            return res > 0 ? ErrEnum.Ok : ErrEnum.Fail;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SendMessage(MessageModel model) {
            var num = MessageHelper.CreateNum( );
            ErrEnum aaa = ErrEnum.SendMessageFail;
            //暂时先不发短信
            //var message = ConfigurationManager.AppSettings["Message"];
            //var res = MessageHelper.SendMessage(model.Mobile, string.Format(message, num));
            //string[] tempA = res.Split('&');
            ////不为空
            //if (tempA != null && tempA.Length > 0) {
            //    for (int i = 0; i < tempA.Length; i++) {
            //        //成功
            //        if (tempA[i] == "err=发送成功！") {
            //            aaa = ErrEnum.SendMessageOk;
            //            break;
            //        }
            //    }
            //}
            // if (aaa == ErrEnum.SendMessageOk) {
            //保存数据库
            var sql = $@"INSERT INTO Tbl_VerifyCode
(SysId,Mobile,VerifyType,VerifyCode,CreationTime)
VALUES({model.SysId},'{model.Mobile}',1,'{num}',getdate())";
            MySqlHelper.ExecteNonQuery(CommandType.Text, sql);
            // }
            return num;
        }
        /// <summary>
        /// 报名工作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        public ErrEnum RegistWork(int userId, int workId) {
            var sql = $@"SELECT * FROM Tbl_UserRegistion AS ur WHERE ur.ParttimeId={workId} AND ur.Pid={userId}";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            var model = ConvertToModel<Tbl_UserRegistion>(dt).FirstOrDefault( );
            var isql = string.Empty;
            if (model != null)
                isql = $@"update Tbl_UserRegistion set state=(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已报名')
    where ParttimeId={workId} AND Pid={userId}";
            else
                isql = $@"INSERT INTO Tbl_UserRegistion
(ParttimeId,[State], Pid, CreationOne, CreationTime, IsDeleted, UpdateOne, updateTime)
VALUES({workId},(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已报名'),{userId},{userId},
getdate(),0,null,getdate())";
            var res = MySqlHelper.ExecteNonQuery(CommandType.Text, isql);
            return res > 0 ? ErrEnum.Ok : ErrEnum.Fail;
        }
        /// <summary>
        /// 放弃该工作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        public ErrEnum DeleteRegistWork(int userId, int workId) {
            var sql = $@"SELECT * FROM Tbl_UserRegistion AS ur WHERE ur.ParttimeId={workId} AND ur.Pid={userId}";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            var model = ConvertToModel<Tbl_UserRegistion>(dt).FirstOrDefault( );
            if (model == null) {
                return ErrEnum.NoRegistWork;
            }
            var isql = $@"update Tbl_UserRegistion set state=(SELECT TOP 1 id FROM Tbl_PublicSubEnum AS pse WHERE  pse.SubName='已放弃')
    where ParttimeId={workId} AND Pid={userId}";
            var res = MySqlHelper.ExecteNonQuery(CommandType.Text, isql);
            return res > 0 ? ErrEnum.Ok : ErrEnum.Fail;
        }
        /// <summary>
        /// 获取已报名的工作
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<RegistWorkOutPut> GetRegistWorks(BaseFilter filter, Tbl_SysUser user, out int count) {
            if (user.HadRegisterUser <= 0) {
                count = 0;
                return null;
            }
            var sql = $@"SELECT b.ParttimeId,c.ParttimeName,c.Pay,e.SubName PayState,d.CompanyName,f.SubName [State],c.CloseTime
,case when f.SubName='已通过' then 0 else 1 end as ShowDel 
  FROM Tbl_SysUser AS a
  inner JOIN Tbl_UserRegistion AS b ON a.Id=b.Pid
  LEFT JOIN Tbl_ParttimeInfo AS c ON b.ParttimeId=c.Id
  LEFT JOIN Tbl_CompanyInfo AS d ON c.CreationOne=d.CreationOne
  LEFT JOIN Tbl_PublicSubEnum AS e ON c.PayState=e.Id
  LEFT JOIN Tbl_PublicSubEnum AS f ON b.[State]=f.Id
WHERE a.Id={user.Id} and b.IsDeleted<>1 and f.SubName <> '已放弃'";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            var model = ConvertToModel<RegistWorkOutPut>(dt);

            if (model == null || model.Count <= 0) {
                count = 0;
                return null;
            }
            count = model.Count;
            var list = model.OrderBy(c => c.CloseTime).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
            return list;
        }


    }
}
