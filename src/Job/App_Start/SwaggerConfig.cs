using System.Web.Http;
using Job;
using WebActivatorEx;
using Swashbuckle.Application;
using WebApi;
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Job
{
    public class SwaggerConfig
     {
         public static void Register()
         {
             Swashbuckle.Bootstrapper.Init(GlobalConfiguration.Configuration);
  
             // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...
             SwaggerSpecConfig.Customize(c =>
             {
                 c.IncludeXmlComments(GetXmlCommentsPath());
                 c.OperationFilter<HttpHeaderFilter>();
             });
        
        }
  
         private static string GetXmlCommentsPath()
         {
             return System.String.Format(@"{0}\bin\Job.XML", System.AppDomain.CurrentDomain.BaseDirectory);
         }
     }
}