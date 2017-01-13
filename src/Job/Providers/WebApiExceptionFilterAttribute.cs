using Job.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Job.Providers {
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext) {
            Logger log = Logger.Singleton;
            log.Error("全局错误", actionExecutedContext.Exception);

            //2.返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException) {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else if (actionExecutedContext.Exception is TimeoutException) {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
          
            //.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            else {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            base.OnException(actionExecutedContext);
        }
    }
}