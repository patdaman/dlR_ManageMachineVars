using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DevOpsApi.Controllers
{
    [EnableCors("*", "*", "*", "*")]
    public abstract class ApiControllerWithHub<THub> : ApiController
    where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
}