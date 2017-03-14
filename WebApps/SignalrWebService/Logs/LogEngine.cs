using Microsoft.AspNet.SignalR;
using SignalrWebService.Hubs;
using SignalrWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalrWebService.Logs
{
    public class LogEngine
    {
        public static string machineName { get; set; }
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        public static List<Event> events = new List<Event>();

        public LogEngine(int pollIntervalMillis)
        {
            if (string.IsNullOrWhiteSpace(machineName))
                machineName = Environment.MachineName;
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            _pollIntervalMillis = pollIntervalMillis;
        }


    }
}