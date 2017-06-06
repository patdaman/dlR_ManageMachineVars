using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DevOpsApi.Controllers
{
    [HubName("boardhub")]
    public class BoardHub : Hub
    {
        public void NotifyBoardUpdated()
        {
            Clients.All.BoardUpdated();
        }
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Communicate(string messageId, string message)
        {
            Clients.All.addNewMessageToPage(messageId, message);
        }

        public void Heartbeat()
        {
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            return (base.OnConnected());
        }
    }
}