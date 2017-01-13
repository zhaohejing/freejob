using Job.Application.IService;
using Job.Application.Service;
using Job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Job.Controllers
{
    /// <summary>
    /// 公司操作
    /// </summary>
    
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyService _companyService;
        public CompanyController() {
            _companyService = new CompanyService();
        }
        /// <summary>
        /// 已报名人员列表(仅含已报名)
        /// </summary>
        /// <param name="work"><see cref="RegistWork"/></param>
        /// <returns><see cref="Application.CompanyWorkUsers"/></returns>
        [HttpPost]
        public IHttpActionResult GetRegistUsers(RegistWork work) {
            var res = _companyService.CompanyRegistUsers(work.WorkId,"已报名");
            if (res!=null) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = res });
            }
            return Json(new { Result = 0, ErrorMsg = "获取失败" });
        }
        /// <summary>
        /// 已通过人员列表(仅含已通过)
        /// </summary>
        /// <param name="work"><see cref="RegistWork"/></param>
        /// <returns><see cref="Application.CompanyWorkUsers"/></returns>
        [HttpPost]
        public IHttpActionResult GetCheckedUsers(RegistWork work) {
            var res = _companyService.CompanyRegistUsers(work.WorkId, "已通过");
            if (res != null) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = res });
            }
            return Json(new { Result = 0, ErrorMsg = "获取失败" });
        }
        /// <summary>
        ///  获取当前工作的报名人员详情
        /// </summary>
        /// <param name="work" ><see cref="RegistWork"/></param>
        /// <returns><see cref="Application.BaseUser"/></returns>
        [HttpPost]
        public IHttpActionResult GetRegistUsersInfo(RegistWork work) {
            if (work.UserId<=0) {
                return Json(new { Result = 0, ErrorMsg = "未找到当前用户" });
            }
            var res = _companyService.GetRegistUserInfo(work.UserId, work.WorkId);
            if (res==null) {
                return Json(new { Result = 0, ErrorMsg = "获取失败" });
            }
            return Json(new { Result = 1, ErrorMsg = "成功", Data = res });
        }
        /// <summary>
        /// 更新用户报名状态已通过
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VilidateRegistUserState(RegistUserState state) {
            var res = _companyService.VilidateRegistUser(state.UserId, state.WorkId);
            return Json(new { Result = res > 0 ? 1 : 0, ErrorMsg = res > 0 ? "审核成功" : "审核失败" });
        }
        /// <summary>
        /// 更新用户报名状态未通过
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NoVilidateRegistUserState(RegistUserState state)
        {
            var res = _companyService.NoPassRegistUser(state.UserId, state.WorkId);
            return Json(new { Result = res > 0 ? 1 : 0, ErrorMsg = res > 0 ? "审核成功" : "审核失败" });
        }

    }
   
}
