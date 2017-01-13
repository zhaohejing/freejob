using Job.Application;
using Job.Application.IService;
using Job.Application.Service;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Job.Controllers {
    /// <summary>
    /// 工作控制器
    /// </summary>
    [RoutePrefix("api/Work")]
    public class WorkController : BaseApiController {
        private readonly IWorkService _workService;

        public WorkController( ) {
            _workService = new WorkService( );
        }
        /// <summary>
        /// 创建工作
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Insert(WorkInfo work) {
            var res = _workService.InsertWork(work, CurrentUser);
            if (res == BaseService.ErrEnum.IndustrysIsNull) {
                return Json(new { Result = 0, ErrorMsg = "所属行业不能为空" });
            }
            return Json(new { Result = res == BaseService.ErrEnum.Ok ? 1 : 0, ErrorMsg = res == BaseService.ErrEnum.Ok ? "创建成功" : "创建失败" });
        }
        /// <summary>
        /// 获取当前人所在公司已发布工作
        /// </summary>
        /// <param name="filer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPublishList(BaseFilter filer) {
            int count = 0;
            var list = _workService.GetPublishList(filer, out count, CurrentUser.Id);
            if (list != null && list.Count( ) > 0) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = new { rows = list, total = count } });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据", Data = new { rows = list, total = 0 } });
        }
        /// <summary>
        /// 获取当前人所在公司已审核工作
        /// </summary>
        /// <param name="filer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetCheckedList(BaseFilter filer) {
            int count = 0;
            var list = _workService.GetCheckedList(filer, out count, CurrentUser.Id);
            if (list != null && list.Count( ) > 0) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = new { rows = list, total = count } });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据", Data = new { rows = list, total = 0 } });
        }

        /// <summary>
        /// 获取已发布的工作列表（已审核）
        /// </summary>
        /// <param name="filer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetVilidateList(WorkFilter filer) {
            int count = 0;
            var list = _workService.SearchWork(filer, out count);
            if (list != null && list.Count( ) > 0) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = new { rows = list, total = count } });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据", Data = new { rows = list, total = 0 } });
        }
        /// <summary>
        /// 获取 区域全部 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAreas( ) {

            var list = _workService.GetArea<SerializeModel>( );
            if (list != null && list.Count( ) > 0) {

                return Json(new { Result = 1, ErrorMsg = "成功", Data =ChangeModel(list) });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据" });
        }
        /// <summary>
        /// 获取工作类型 全部
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWorks( ) {
            var list = _workService.GetWorks<SerializeModel>( );
            if (list != null && list.Count( ) > 0) {

                return Json(new { Result = 1, ErrorMsg = "成功", Data = ChangeModel(list) });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据" });
        }
        /// <summary>
        /// 获取工作年限类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWorkYears( ) {
            var list = _workService.GetWorkYear<SerializeModel>( );
            if (list != null && list.Count( ) > 0) {

                return Json(new { Result = 1, ErrorMsg = "成功", Data = ChangeModel(list) });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据" });
        }
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGenders( ) {
            var list = _workService.GetGenders<SerializeModel>( );
            if (list != null && list.Count( ) > 0) {

                return Json(new { Result = 1, ErrorMsg = "成功", Data = ChangeModel(list) });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据" });
        }
        /// <summary>
        /// 工作详情
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDetail(int workId) {
            var model = _workService.WorkDetail(workId, CurrentUser.Id);
            if (model != null) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = model });
            }
            return Json(new { Result = 0, ErrorMsg = "失败" });
        }
        /// <summary>
        /// 工作搜索
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWorksByCode(string filter) {
            var model = _workService.GetWorks(filter);
            if (model != null) {
                return Json(new { Result = 1, ErrorMsg = "成功", Data = model });
            }
            return Json(new { Result = 0, ErrorMsg = "失败" });
        }

        /// <summary>
        /// 关闭工作
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CloseWork(int workId) {
            var res = _workService.CloseWork(workId, CurrentUser.Id);
            return Json(new { Result = res > 0 ? 1 : 0, ErrorMsg = res > 0 ? "关闭成功" : "关闭失败" });
        }
        /// <summary>
        /// 获取结算方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPayState( ) {
            var list = _workService.GetPayState<SerializeModel>( );
            if (list != null && list.Count( ) > 0) {

                return Json(new { Result = 1, ErrorMsg = "成功", Data =ChangeModel(list) });
            }
            return Json(new { Result = 0, ErrorMsg = "暂无数据" });
        }
        private List<TempLevelModel> ChangeModel(IEnumerable<SerializeModel> list) {
            var res = new List<TempLevelModel>( );

            foreach (var item in list) {
                var temp = new TempLevelModel( ) { value = item.Id, text = item.Name };
                if (!res.Contains(temp) && item.LevelCode.Length == 14) {
                    temp.children = new List<TempLevelModel>( );
                    foreach (var pp in list) {
                        var ct = new TempLevelModel( ) { value = pp.Id, text = pp.Name, children = new List<TempLevelModel>( ) };
                        if (!temp.children.Contains(ct) && pp.LevelCode.Contains(item.LevelCode) && pp.LevelCode.Length == 19) {
                            temp.children.Add(ct);
                        }
                    }
                    res.Add(temp);
                }
            }
            return res;
        }
    
    }
    public class TempLevelModel {
        public int value { get; set; }
        public string text { get; set; }
        public List<TempLevelModel> children { get; set; }
    }

}
