using Microsoft.AspNet.SignalR;
using SignalrWebService.Hubs;
using ViewModel;
using SignalrWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SignalrWebService.Logs
{
    public class LogEngine
    {
        public static string machineName { get; set; }
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        public static List<ViewModel.Event> events = new List<ViewModel.Event>();
        private BusinessLayer.ManageLogging logProcessor = new BusinessLayer.ManageLogging();

        public LogEngine(int pollIntervalMillis)
        {
            if (string.IsNullOrWhiteSpace(machineName))
                machineName = Environment.MachineName;
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            _pollIntervalMillis = pollIntervalMillis;
        }

        public async Task OnLogMonitor()
        {
            while (true)
            {
                await Task.Delay(_pollIntervalMillis);
                try
                {
                    events.AddRange(logProcessor.GetAllLogs(DateTime.Now.AddMilliseconds(- (_pollIntervalMillis + 1000)), DateTime.MaxValue));
                }
                catch (InvalidOperationException ex)
                {
                    Trace.TraceError("Event log read error");
                    Trace.TraceError(ex.Message);
                    Trace.TraceError(ex.StackTrace);
                }
                _hubs.Clients.All.broadcastPerformance(events);
                _hubs.Clients.All.serverTime(DateTime.UtcNow.ToString());
            }
        }

    }
}