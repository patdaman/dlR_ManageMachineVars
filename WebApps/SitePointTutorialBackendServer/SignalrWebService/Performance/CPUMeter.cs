using System;
using System.Diagnostics;

namespace SignalrWebService.Performance
{
    public class CPUMeter : IDisposable
    {
        CounterSample _startSample;
        PerformanceCounter _cnt;

        /// Creates a per-process CPU meter instance tied to the current process.
        public CPUMeter()
        {
            String instancename = GetCurrentProcessInstanceName();
            _cnt = new PerformanceCounter("Process", "% Processor Time", instancename, true);
            ResetCounter();
        }

        /// Creates a per-process CPU meter instance tied to a specific process.
        public CPUMeter(int pid)
        {
            String instancename = GetProcessInstanceName(pid);
            _cnt = new PerformanceCounter("Process", "% Processor Time", instancename, true);
            ResetCounter();
        }

        /// Resets the internal counter. All subsequent calls to GetCpuUtilization() will 
        /// be relative to the point in time when you called ResetCounter(). This 
        /// method can be call as often as necessary to get a new baseline for 
        /// CPU utilization measurements.
        public void ResetCounter()
        {
            _startSample = _cnt.NextSample();
        }

        /// Returns this process's CPU utilization since the last call to ResetCounter().
        public double GetCpuUtilization()
        {
            CounterSample curr = _cnt.NextSample();

            double diffValue = curr.RawValue - _startSample.RawValue;
            double diffTimestamp = curr.TimeStamp100nSec - _startSample.TimeStamp100nSec;

            double usage = (diffValue / diffTimestamp) * 100;
            return usage;
        }

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

        public void Dispose()
        {
            if (_cnt != null) _cnt.Dispose();
        }
    }
}