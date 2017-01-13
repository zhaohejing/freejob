using Job.Application;
using Job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using static Job.Application.Service.BaseService;

namespace Job.Controllers {
    public class UserController:BaseApiController {
        public UserController() {

        }
        /// <summary>
        /// 工作报名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RegisterWork(RegistWork model) {
            var res = _userService.RegistWork(CurrentUser.Id, model.WorkId);
            if (res== ErrEnum.HadRegistWork) {
                return Json(new { Result = 0, ErrorMsg = "已报名当前工作" });
            }
            else if (res==ErrEnum.Ok) {
                return Json(new { Result = 1, ErrorMsg = "成功"});
            }
            return Json(new { Result = 0, ErrorMsg = "失败" });
        }
        /// <summary>
        /// 获取用户已报名的工作
        /// </summary>
        /// <param name="filer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetRegistWorks(BaseFilter filer) {
            int count = 0;
            var res = _userService.GetRegistWorks(filer, CurrentUser, out count);
            if (res!=null&&res.Count()>0) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = new { rows = res, count = count } });
            }
            return Json(new { Result = 0, ErrorMsg = "失败" });
        }
        /// <summary>
        /// 获取用户已报名的工作
        /// </summary>
        /// <param name="filer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteRegistWork(RegistWork model) {
            var res = _userService.DeleteRegistWork(CurrentUser.Id, model.WorkId);
            if (res == ErrEnum.NoRegistWork) {
                return Json(new { Result = 0, ErrorMsg = "未报名过当前工作" });
            }
            else if (res == ErrEnum.Ok) {
                return Json(new { Result = 1, ErrorMsg = "成功" });
            }
            return Json(new { Result = 0, ErrorMsg = "失败" });
        }



    }
}