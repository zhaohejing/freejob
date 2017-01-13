using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Job.Startup))]
namespace Job {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            Swashbuckle.Bootstrapper.Init(config);
            app.UseWebApi(config);
        }
    }
}