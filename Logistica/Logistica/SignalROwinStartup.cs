using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Logistica.SignalROwinStartup), "Configuration")]
namespace Logistica
{
    public class SignalROwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());

            HubConfiguration hubConfiguration = new HubConfiguration()
            {
                EnableDetailedErrors = true,
                //EnableJavaScriptProxies = true,

                //EnableJSONP = true
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}