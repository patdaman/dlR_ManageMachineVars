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
            new PerformanceCounter("Processor", "% Processor Time", "_Total"),
            new PerformanceCounter("Memory", "Available MBytes"),
            new PerformanceCounter("Process", "% Processor Time", GetCurrentProcessInstanceName(), true),
            new PerformanceCounter("Process", "Working Set", GetCurrentProcessInstanceName(), true),

            //new PerformanceCounter("Database", "Database", "Database"),
            //new PerformanceCounter("Network Interface", "Network Interface", "_Total"),
            //new PerformanceCounter("PhysicalDisk", "PhysicalDisk", "_Total"),
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
            //categories = System.Diagnostics.PerformanceCounterCategory.GetCategories();
            //List<string> lines = new List<string>();
            //lines.Add("CategoryName, CategoryType, MachineName, CategoryHelp");

            //List<string> counterLines = new List<string>();
            //foreach (var x in categories)
            //{
            //    if (x.GetInstanceNames().Count() < 2)
            //    {
            //        counters = x.GetCounters();
            //        foreach (var y in counters)
            //        {
            //            counterLines.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", y.CategoryName, y.Container, y.CounterName, y.CounterType, y.InstanceName, y.Site));
            //        }
            //    }
            //    lines.Add(string.Format("{0}, {1}, {2}, {3}", x.CategoryName, x.CategoryType, x.MachineName, x.CategoryHelp));
            //}
            //System.IO.File.WriteAllLines(@"C:\Users\pdelosreyes\Documents\Categories.csv", lines.ToArray());
            //System.IO.File.WriteAllLines(@"C:\Users\pdelosreyes\Documents\Counters.csv", counterLines.ToArray());
        }

        public async Task OnPerformanceMonitor()
        {
            List<PerformanceModel> pList = new List<PerformanceModel>()
            {
                new PerformanceModel() { CategoryName="Processor", Value=ServiceCounters.Where(x => x.CategoryName == "Processor").FirstOrDefault().NextValue(), CounterName="% Processor Time", InstanceName=".", MachineName="SitePointService"},
                new PerformanceModel() { CategoryName="Memory", Value=ServiceCounters.Where(x => x.CategoryName == "Memory").FirstOrDefault().NextValue(), CounterName="Available MBytes", InstanceName=".", MachineName="SitePointService"},
                //new PerformanceModel() { CategoryName="Network In", Value=ServiceCounters.Where(x => x.CategoryName == "Network In").FirstOrDefault().NextValue(), CounterName="Network In", InstanceName=".", MachineName="SitePointService"},
                //new PerformanceModel() { CategoryName="Network Out", Value=ServiceCounters.Where(x => x.CategoryName == "Network Out").FirstOrDefault().NextValue(), CounterName="Network Out", InstanceName=".", MachineName="SitePointService"},
                //new PerformanceModel() { CategoryName="Disk Read Bytes/Sec", Value=ServiceCounters.Where(x => x.CategoryName == "Disk Read Byted/Sec").FirstOrDefault().NextValue(), CounterName="Disk Read Bytes/Sec", InstanceName=".", MachineName="SitePointService"},
                //new PerformanceModel() { CategoryName="Disk Write Bytes/Sec", Value=ServiceCounters.Where(x => x.CategoryName == "Disk Write Bytes/Sec").FirstOrDefault().NextValue(), CounterName="Disk Write Bytes/Sec", InstanceName=".", MachineName="SitePointService"}
                
                //new PerformanceModel() { CategoryName="Processor", Value=_cpuRand.Next(64), CounterName="% Processor Time", InstanceName=".", MachineName="SitePointService"},
                //new PerformanceModel() { CategoryName="Memory", Value=_memRand.Next(1024,2048), CounterName="Available MBytes", InstanceName=".", MachineName="SitePointService"},
                new PerformanceModel() { CategoryName="Network In", Value=100*_netIn.NextDouble(), CounterName="Network In", InstanceName=".", MachineName="SitePointService"},
                new PerformanceModel() { CategoryName="Network Out", Value=90*_netOut.NextDouble(), CounterName="Network Out", InstanceName=".", MachineName="SitePointService"},
                new PerformanceModel() { CategoryName="Disk Read Bytes/Sec", Value=60*_diskRd.NextDouble(), CounterName="Disk Read Bytes/Sec", InstanceName=".", MachineName="SitePointService"},
                new PerformanceModel() { CategoryName="Disk Write Bytes/Sec", Value=60*_diskWt.NextDouble(), CounterName="Disk Write Bytes/Sec", InstanceName=".", MachineName="SitePointService"}

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
                        switch (performanceCounter.CategoryName)
                        {
                            case "Processor":
                                performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    Value = Math.Round(ServiceCounters.Where(x => x.CategoryName == "Processor").FirstOrDefault().NextValue(), 2, MidpointRounding.AwayFromZero)
                                    //Value = _cpuRand.Next(64)
                                });
                                break;
                            case "Memory":
                                performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    Value = Math.Round(ServiceCounters.Where(x => x.CategoryName == "Memory").FirstOrDefault().NextValue(), 2, MidpointRounding.AwayFromZero)
                                    //Value = _memRand.Next(1024, 2048)
                                });
                                break;
                            case "Network In":
                                performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    //Value = ServiceCounters.Where(x => x.CategoryName == "Network In").FirstOrDefault().NextValue()
                                    Value = 100 * _netIn.NextDouble()
                                });
                                break;
                            case "Network Out":
                                 performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    //Value = ServiceCounters.Where(x => x.CategoryName == "Network Out").FirstOrDefault().NextValue()
                                    Value = 90 * _netOut.NextDouble()
                                 });
                                break;
                            case "Disk Read Bytes/Sec":
                                performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    //Value = ServiceCounters.Where(x => x.CategoryName == "Disk Read Bytes/Sec").FirstOrDefault().NextValue()
                                    Value = 60 * _diskRd.NextDouble()
                                });
                                break;
                            case "Disk Write Bytes/Sec":
                                performanceModels.Add(new PerformanceModel
                                {
                                    MachineName = performanceCounter.MachineName,
                                    CategoryName = performanceCounter.CategoryName,
                                    CounterName = performanceCounter.CounterName,
                                    InstanceName = performanceCounter.InstanceName,
                                    //Value = ServiceCounters.Where(x => x.CategoryName == "Disk Write Bytes/Sec").FirstOrDefault().NextValue()
                                    Value = 60 * _diskWt.NextDouble()
                                });
                                break;

                        }
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