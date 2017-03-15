using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;

namespace CommonUtils.PerformanceCounters
{
    public class PerformanceCounters
    {
        public List<PerformanceCounter> serviceCounters = new List<PerformanceCounter>();
        public string machineName { get; set; }

        public PerformanceCounters()
        {
            if (string.IsNullOrWhiteSpace(machineName))
                machineName = Environment.MachineName;
        }

        public PerformanceCounters(string machineName)
        {
            this.machineName = Environment.MachineName;
        }

        public List<PerformanceCounter> GetNetworkCounters(List<string> instances)
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            foreach (var nic in instances)
            {
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Received/sec", nic, machineName));
                counters.Add(new PerformanceCounter("Network Adapter", "Bytes Sent/sec", nic, machineName));
            }
            return counters;
        }

        public List<PerformanceCounter> GetSystemCounters()
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            counters.Add(new PerformanceCounter("Processor Information", "% Processor Time", "_Total", machineName));
            counters.Add(new PerformanceCounter("Memory", "Available MBytes", string.Empty, machineName));
            return counters;
        }

        public List<PerformanceCounter> GetProcessSystemCounters(int processId)
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            counters.Add(new PerformanceCounter("Processor Information", "% Processor Time", GetProcessInstanceName(processId), machineName));
            counters.Add(new PerformanceCounter("Memory", "Available MBytes", GetProcessInstanceName(processId), machineName));
            return counters;
        }

        public List<PerformanceCounter> GetProcessSystemCounters(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            List<PerformanceCounter> counters = new List<PerformanceCounter>();

            foreach (Process proc in processes)
            {
                counters.Add(new PerformanceCounter("Processor Information", "% Processor Time", GetProcessInstanceName(proc.Id), machineName));
                counters.Add(new PerformanceCounter("Memory", "Available MBytes", string.Empty, machineName));
            }
            return counters;
        }

        public List<PerformanceCounter> GetDriveCounters()
        {
            return GetDriveCounters(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2));
        }

        public List<PerformanceCounter> GetDriveCounters(string driveLetter)
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            if (string.IsNullOrWhiteSpace(driveLetter))
                driveLetter = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);

            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Disk Reads/sec", driveLetter, machineName));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Disk Writes/sec", driveLetter, machineName));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "Free Megabytes", driveLetter, machineName));
            serviceCounters.Add(new PerformanceCounter("LogicalDisk", "% Free Space", driveLetter, machineName));
            return counters;
        }

        public List<PerformanceCounter> GetWebServiceCounters()
        {
            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            if (CheckIISRunning())
            {
                counters.Add(new PerformanceCounter("Web Service Cache", "Current File Cache Memory Usage", string.Empty, machineName));
                counters.Add(new PerformanceCounter("Web Service Cache", "Maximum File Cache Memory Usage", string.Empty, machineName));
                counters.Add(new PerformanceCounter("Web Service", "Bytes Sent/sec", "_Total", machineName));
                counters.Add(new PerformanceCounter("Web Service", "Bytes Received/sec", "_Total", machineName));
                counters.Add(new PerformanceCounter("Web Service", "Current Connections", "_Total", machineName));
            }
            return counters;
        }

        public bool CheckIISRunning()
        {
            ServiceController controller = new ServiceController("W3SVC", machineName);
            return controller.Status == ServiceControllerStatus.Running;
        }

        public List<ServiceController> SqlServerServices()
        {
            ServiceController[] controllers = ServiceController.GetServices();
            List<ServiceController> sqlControllers = new List<ServiceController>();
            foreach (ServiceController service in controllers)
            {
                if (service == null)
                    continue;
                if (service.ServiceName.ToLower().Contains("SQLServer".ToLower()))
                    {
                        //serviceoutput = serviceoutput + System.Environment.NewLine + "Service Name = " + service.ServiceName + System.Environment.NewLine + "Display Name = " + service.DisplayName + System.Environment.NewLine + "Status = " + service.Status + System.Environment.NewLine;
                        sqlControllers.Add(service);
                    }
            }
            return sqlControllers;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current process instance name. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/10/2017. </remarks>
        ///
        /// <returns>   The current process instance name. </returns>
        ///-------------------------------------------------------------------------------------------------
        public string GetCurrentProcessInstanceName()
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
        public string GetProcessInstanceName(int pid)
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process", machineName);

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
    }
}
