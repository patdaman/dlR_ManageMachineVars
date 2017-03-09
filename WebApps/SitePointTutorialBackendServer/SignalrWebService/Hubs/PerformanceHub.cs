using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalrWebService.Models;
using SignalrWebService.Performance;
using System.Threading.Tasks;

namespace SignalrWebService.Hubs
{
    public class PerformanceHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();  
        }

        public void SendPerformance(IList<PerformanceModel> performanceModels)
        {
            Clients.All.broadcastPerformance(performanceModels);
        }

        public void Communicate(string messageId, string message)
        {
            Clients.All.addNewMessageToPage(messageId, message);
        }

        public void Heartbeat()
        {
            Clients.All.heartbeat();
        }

        public dynamic MonitoringFor()
        {

            return PerformanceEngine.ServiceCounters.Select(counter =>
                new
                {
                    MachineName = counter.CounterName,
                    CategoryName = counter.CategoryName,
                    CounterName = counter.CounterName,
                    InstanceName = counter.InstanceName
                });
        }

        public override Task OnConnected()
        {
            return (base.OnConnected());
        }

        public string GetServerTime()
        {
            return DateTime.UtcNow.ToString();
        }
    }
}