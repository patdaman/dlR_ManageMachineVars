using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalrWebService.Models;
using SignalrWebService.Hubs;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace SignalrWebService.Performance
{
    public class PerformanceEngine 
    {
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        static Random _cpuRand;
        static Random _memRand;
        static Random _netIn;
        static Random _netOut;
        static Random _diskRd;
        static Random _diskWt;


        public static readonly IEnumerable<PerformanceCounter> ServiceCounters = new[]
        {
            // http://weblogs.thinktecture.com/ingo/2004/06/getting-the-current-process-your-own-cpu-usage.html
            // Code already written a LONG time ago to do this exact thing.
            new PerformanceCounter("Processor Information", "% Processor Time", "_Total"),
            new PerformanceCounter("Memory", "Available MBytes"),
            new PerformanceCounter("Process", "% Processor Time", GetCurrentProcessInstanceName(), true),
            new PerformanceCounter("Process", "Working Set", GetCurrentProcessInstanceName(), true),
            new PerformanceCounter("Network Adapter", "Bytes Received/sec", "isatap.corp.printable.com"),
            new PerformanceCounter("Network Adapter", "Bytes Sent/sec", "isatap.corp.printable.com"),
            new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", "_Total"),
            new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", "_Total"),
            new PerformanceCounter("LogicalDisk", "Free Megabytes", "C:"),
            new PerformanceCounter("Web Service Cache", "Current File Cache Memory Usage"),
            new PerformanceCounter("Web Service Cache", "Maximum File Cache Memory Usage"),
            new PerformanceCounter("Web Service", "Bytes Sent/sec", "_Total"),
            new PerformanceCounter("Web Service", "Bytes Received/sec", "_Total"),
        };

        private static PerformanceCounterCategory[] categories;
        private static PerformanceCounter[] counters;

        public PerformanceEngine(int pollIntervalMillis)
        {
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<PerformanceHub>();
            _pollIntervalMillis = pollIntervalMillis;
            _cpuRand = new Random();
            _memRand = new Random();
            _netIn = new Random();
            _netOut = new Random();
            _diskRd = new Random();
            _diskWt = new Random();
        }

        public async Task OnPerformanceMonitor()
        {
            List<PerformanceModel> pList = new List<PerformanceModel>()
            {
                GetPerformanceModel("Processor Information", "% Processor Time"),
                GetPerformanceModel("Memory", "Available MBytes"),
                GetPerformanceModel("Network Adapter", "Bytes Received/sec"),
                GetPerformanceModel("Network Adapter", "Bytes Sent/sec"),
                GetPerformanceModel("LogicalDisk", "Disk Read Bytes/Sec"),
                GetPerformanceModel("LogicalDisk", "Disk Write Bytes/Sec")

            };
            //Monitor for infinity!
            while (true)
            {
                await Task.Delay(_pollIntervalMillis);

                //List of performance models that is loaded up on every itteration.
                IList<PerformanceModel> performanceModels = new List<PerformanceModel>();
                foreach (var performanceCounter in pList)
                {
                    try
                    {
                            performanceModels.Add(GetPerformanceModel(
                                performanceCounter.CategoryName,
                                performanceCounter.CounterName));
                     }
                    catch (InvalidOperationException ex)
                    {
                        Trace.TraceError("Performance with Performance counter {0}.", performanceCounter.MachineName + performanceCounter.CategoryName + performanceCounter.CounterName);
                        Trace.TraceError(ex.Message);
                        Trace.TraceError(ex.StackTrace);
                    }
                }

                _hubs.Clients.All.broadcastPerformance(performanceModels);
                _hubs.Clients.All.serverTime(DateTime.UtcNow.ToString());
            }
        }

        public void Stop(bool immediate)
        {

            //HostingEnvironment.UnregisterObject(this);
        }

        #region engine helpers
        private static string GetCurrentProcessInstanceName()
        {
            Process proc = Process.GetCurrentProcess();
            int pid = proc.Id;
            return GetProcessInstanceName(pid);
        }

        private PerformanceModel GetPerformanceModel(string categoryName, string counterName)
        {
            return GetPerformanceModel(categoryName, counterName, ServiceCounters.Where(x => x.CategoryName == categoryName && x.CounterName == counterName).FirstOrDefault().InstanceName ?? "", Environment.MachineName);
        }

        private PerformanceModel GetPerformanceModel(string categoryName, string counterName, string instanceName)
        {
            return GetPerformanceModel(categoryName, counterName, instanceName, Environment.MachineName);
        }

        private PerformanceModel GetPerformanceModel(string categoryName, string counterName, string instanceName, string machineName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
                instanceName = ".";
            try
            {
                return new PerformanceModel()
                {
                    CategoryName = categoryName,
                    CounterName = counterName,
                    InstanceName = instanceName,
                    MachineName = machineName,
                    Value = ServiceCounters.Where(x => x.CategoryName == categoryName && x.CounterName == counterName).FirstOrDefault().NextValue()
                };
            }
            catch
            {
                return new PerformanceModel();
            }
        }

        private static string GetProcessInstanceName(int pid)
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {

                using (PerformanceCounter cnt = new PerformanceCounter("Process",
                     "ID Process", instance, true))
                {
                    int val = (int)cnt.RawValue;
                    if (val == pid)
                    {
                        return instance;
                    }
                }
            }
            throw new Exception("Could not find performance counter " +
                "instance name for current process. This is truly strange ...");
        }
        #endregion
    }

}