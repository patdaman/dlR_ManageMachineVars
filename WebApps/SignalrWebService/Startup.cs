using Owin;
using System.Threading.Tasks;
using Microsoft.Owin;
using SignalrWebService.Performance;
using SignalrWebService.Logs;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalrWebService.Startup))]

namespace SignalrWebService
{
    public class Startup
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

            PerformanceEngine performanceEngine = new PerformanceEngine(800);
            Task.Factory.StartNew(async () => await performanceEngine.OnPerformanceMonitor());
            LogEngine logEngine = new LogEngine(5000);
            Task.Factory.StartNew(async () => await logEngine.OnLogMonitor());
        }
    }
}