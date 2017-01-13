using Job.Application.IService;
using Job.Application.Service;
using Job.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Job.Providers {
    /// <summary>
    /// 自定义此特性用于接口的身份验证
    /// </summary>
    public class RequestAuthorizeAttribute : AuthorizeAttribute {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext) {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            var authorization = actionContext.Request.Headers.Authorization;
            if ((authorization != null) && (authorization.Parameter != null)) {
                //解密用户ticket,并校验用户名密码是否匹配
                var encryptTicket = authorization.Parameter;
                if (ValidateTicket(encryptTicket)) {

                    base.IsAuthorized(actionContext);
                }
                else {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                    .OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket) {
            //解密Ticket
            var strTicket = encryptTicket;
            IUserService _userService = new UserService();
            var res = RedisHelper.GetStringKey(strTicket);
            var t = _userService.GetUserInfo(strTicket);
            if (t != null) {
                if (res.HasValue) {
                    var tokens =  res.ToString().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Id.ToString().Equals(tokens[0])&&t.WeChatToken.Equals(tokens[1])) {
                        return true;
                    }
                    return false;
                }
                else {
                    RedisHelper.KeyDelete(strTicket);
                    return false;
                }
            }
            else {
                RedisHelper.KeyDelete(strTicket);
                return false;
            }
          

        }
    }
}