using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SignalrWebService.Models;
using SignalrWebService.Hubs;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace SignalrWebService.Performance
{
    public class PerformanceEngine 
    {
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        private static NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        private static PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
        private static List<string> networkInstances = category.GetInstanceNames().ToList();
        private static IEnumerable<PerformanceCounter> counters;

        /// <summary>   The service counters. </summary>
        public static IEnumerable<PerformanceCounter> ServiceCounters = new[]
        {
            new PerformanceCounter("Processor Information", "% Processor Time", "_Total"),
            new PerformanceCounter("Memory", "Available MBytes"),
            new PerformanceCounter("Process", "% Processor Time", GetCurrentProcessInstanceName(), true),
            new PerformanceCounter("Process", "Working Set", GetCurrentProcessInstanceName(), true),
            //new PerformanceCounter("Network Adapter", "Bytes Sent/sec", networkInterfaces.Where(x => x.Name.ToLower().Contains("printable.com".ToLower())).FirstOrDefault().Name),
            //new PerformanceCounter("Network Adapter", "Bytes Received/sec", networkInterfaces.Where(x => x.Name.ToLower().Contains("printable.com".ToLower())).FirstOrDefault().Name),
            new PerformanceCounter("LogicalDisk", "Disk Reads/sec", Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0,2)),
            new PerformanceCounter("LogicalDisk", "Disk Writes/sec", Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0,2)),
            new PerformanceCounter("LogicalDisk", "Free Megabytes", Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0,2)),
            new PerformanceCounter("LogicalDisk", "% Free Space", Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0,2)),
            //new PerformanceCounter("Web Service Cache", "Current File Cache Memory Usage"),
            //new PerformanceCounter("Web Service Cache", "Maximum File Cache Memory Usage"),
            //new PerformanceCounter("Web Service", "Bytes Sent/sec", "_Total"),
            //new PerformanceCounter("Web Service", "Bytes Received/sec", "_Total"),
            //new PerformanceCounter("Web Service", "Current Connections", "_Total")
        };

        public PerformanceEngine(int pollIntervalMillis)
        {
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<PerformanceHub>();
            _pollIntervalMillis = pollIntervalMillis;
            counters = GetNetworkCounters();
        }

        private IEnumerable<PerformanceCounter> GetNetworkCounters()
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            foreach (var nic in networkInstances)
            {
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Received/sec", nic));
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Sent/sec", nic));
            }
            counters.Add(new PerformanceCounter("Network Adapter", "Bytes Sent/sec", "Microsoft Kernel Debug Network Adapter"));

            return counters;
        }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Executes the performance monitor action. </summary>
    ///
    /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
    ///
    /// <returns>   A Task. </returns>
    ///-------------------------------------------------------------------------------------------------
    public async Task OnPerformanceMonitor()
        {
            List<PerformanceModel> pList = new List<PerformanceModel>()
            {
                GetPerformanceModel("Processor Information", "% Processor Time"),
                GetPerformanceModel("Memory", "Available MBytes"),
                //GetPerformanceModel("Network Adapter", "Bytes Received/sec"),
                //GetPerformanceModel("Network Adapter", "Bytes Sent/sec"),
                GetPerformanceModel("LogicalDisk", "Disk Reads/sec"),
                GetPerformanceModel("LogicalDisk", "Disk Writes/sec"),
                GetPerformanceModel("LogicalDisk", "% Free Space"),
                GetPerformanceModel("LogicalDisk", "Free Megabytes"),
                //GetPerformanceModel("Web Service Cache", "Current File Cache Memory Usage"),
                //GetPerformanceModel("Web Service Cache", "Maximum File Cache Memory Usage"),
                //GetPerformanceModel("Web Service", "Bytes Sent/sec"),
                //GetPerformanceModel("Web Service", "Bytes Received/sec"),
                //GetPerformanceModel("Web Service", "Current Connections")
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current process instance name. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
        ///
        /// <returns>   The current process instance name. </returns>
        ///-------------------------------------------------------------------------------------------------
        private static string GetCurrentProcessInstanceName()
        {
            Process proc = Process.GetCurrentProcess();
            int pid = proc.Id;
            return GetProcessInstanceName(pid);
        }

        private PerformanceCounter GetPerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            return GetPerformanceCounter(categoryName, counterName, instanceName, Environment.MachineName);
        }

        private PerformanceCounter GetPerformanceCounter(string categoryName, string counterName, string instanceName, string machineName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
                instanceName = ".";
            try
            {
                return new PerformanceCounter()
                {
                    CategoryName = categoryName,
                    CounterName = counterName,
                    InstanceName = instanceName,
                    MachineName = machineName
                };
            }
            catch
            {
                return new PerformanceCounter();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets performance model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
        ///
        /// <param name="categoryName"> Name of the category. </param>
        /// <param name="counterName">  Name of the counter. </param>
        ///
        /// <returns>   The performance model. </returns>
        ///-------------------------------------------------------------------------------------------------
        private PerformanceModel GetPerformanceModel(string categoryName, string counterName)
        {
            return GetPerformanceModel(categoryName, counterName, ServiceCounters.Where(x => x.CategoryName == categoryName && x.CounterName == counterName).FirstOrDefault().InstanceName ?? ".", Environment.MachineName);
        }

        private PerformanceModel GetPerformanceModel(string categoryName, string counterName, string instanceName)
        {
            return GetPerformanceModel(categoryName, counterName, instanceName, Environment.MachineName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets performance model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
        ///
        /// <param name="categoryName"> Name of the category. </param>
        /// <param name="counterName">  Name of the counter. </param>
        /// <param name="instanceName"> Name of the instance. </param>
        /// <param name="machineName">  Name of the machine. </param>
        ///
        /// <returns>   The performance model. </returns>
        ///-------------------------------------------------------------------------------------------------
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
                    Value = Math.Round(ServiceCounters.Where(x => x.CategoryName == categoryName && x.CounterName == counterName).FirstOrDefault().NextValue(), 2)
                };
            }
            catch
            {
                return new PerformanceModel();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets process instance name. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="pid">  The PID. </param>
        ///
        /// <returns>   The process instance name. </returns>
        ///-------------------------------------------------------------------------------------------------
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