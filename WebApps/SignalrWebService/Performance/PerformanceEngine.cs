using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SignalrWebService.Models;
using SignalrWebService.Hubs;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.ServiceProcess;

namespace SignalrWebService.Performance
{
    public class PerformanceEngine 
    {
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        private static List<PerformanceCounter> serviceCounters;

        public PerformanceEngine(int pollIntervalMillis)
        {
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<PerformanceHub>();
            _pollIntervalMillis = pollIntervalMillis;

            //NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            List<string> networkInstances = category.GetInstanceNames().ToList();

            serviceCounters.AddRange(GetNetworkCounters(networkInstances));
            serviceCounters.AddRange(GetSystemCounters());
            serviceCounters.AddRange(GetDriveCounters());
            serviceCounters.AddRange(GetWebServiceCounters());
        }

        private List<PerformanceCounter> GetNetworkCounters(List<string> instances)
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            foreach (var nic in instances)
            {
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Received/sec", nic));
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Sent/sec", nic));
            }
            return counters;
        }

        private List<PerformanceCounter> GetSystemCounters()
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            counters.Add(new PerformanceCounter("Processor Information", "% Processor Time", "_Total"));
            counters.Add(new PerformanceCounter("Memory", "Available MBytes"));
            return counters;
        }

        private List<PerformanceCounter> GetDriveCounters()
        {
            return GetDriveCounters(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2));
        }

        private List<PerformanceCounter> GetDriveCounters(string driveLetter)
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            if (string.IsNullOrWhiteSpace(driveLetter))
                driveLetter = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);

            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Disk Reads/sec", driveLetter));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Disk Writes/sec", driveLetter));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Free Megabytes", driveLetter));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "% Free Space", driveLetter));
            return counters;
        }

        private List<PerformanceCounter> GetWebServiceCounters()
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            if (CheckIISRunning())
            {
                counters.Add(new PerformanceCounter("Web Service Cache", "Current File Cache Memory Usage"));
                counters.Add(new PerformanceCounter("Web Service Cache", "Maximum File Cache Memory Usage"));
                counters.Add(new PerformanceCounter("Web Service", "Bytes Sent/sec", "_Total"));
                counters.Add(new PerformanceCounter("Web Service", "Bytes Received/sec", "_Total"));
                counters.Add(new PerformanceCounter("Web Service", "Current Connections", "_Total"));
            }
            return counters;
        }

        private bool CheckIISRunning()
        {
            ServiceController controller = new ServiceController("W3SVC");
            return controller.Status == ServiceControllerStatus.Running;
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
            while (true)
            {
                await Task.Delay(_pollIntervalMillis);

                //List of performance models that is loaded up on every itteration.
                IList<PerformanceModel> performanceModels = new List<PerformanceModel>();
                //foreach (var performanceCounter in pList)
                foreach (PerformanceCounter performanceCounter in serviceCounters)
                {
                    try
                    {
                        new PerformanceModel()
                        {
                            CategoryName = performanceCounter.CategoryName,
                            CounterName = performanceCounter.CounterName,
                            InstanceName = performanceCounter.InstanceName,
                            MachineName = performanceCounter.MachineName,
                            Value = Math.Round(performanceCounter.NextValue(), 2)
                        };
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