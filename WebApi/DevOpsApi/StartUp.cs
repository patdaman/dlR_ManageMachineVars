using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(DevOpsApi.StartUp))]
namespace DevOpsApi
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            var hubConfiguration = new HubConfiguration()
            {
                EnableDetailedErrors = true
            };
            //app.MapSignalR("/signalr/hubs", hubConfiguration);
            app.MapSignalR(hubConfiguration);

            app.MapSignalR();
        }
    }
}