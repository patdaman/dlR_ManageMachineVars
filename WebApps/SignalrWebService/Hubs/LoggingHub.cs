using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalrWebService.Models;
using ViewModel;
using SignalrWebService.Logs;
using System.Threading.Tasks;

namespace SignalrWebService.Hubs
{
    public class EventHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendLogs(IList<ViewModel.Event> eventModels)
        {
            Clients.All.broadcastEvents(eventModels);
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
            return LogEngine.events.Select(e =>
                new ViewModel.Event(e));
        }

        public override Task OnConnected()
        {
            return (base.OnConnected());
        }
    }
}