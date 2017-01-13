using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using Swashbuckle.Swagger;
namespace WebApi {
    public class HttpHeaderFilter : IOperationFilter {
        public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription) {
            if (operation.Parameters == null)
                operation.Parameters = new List<Parameter>();
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline(); //判断是否添加权限过滤器
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter =>
            filter is IAuthorizationFilter); //判断是否允许匿名方法 
            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (isAuthorized && !allowAnonymous) {
                operation.Parameters.Add(new Parameter {

                    Name = "Authorization",
                    ParamType = "header",
                    Description = "free",
                    Required = false,
                    Type = "string"
                });
            }

        }

   
    }
}