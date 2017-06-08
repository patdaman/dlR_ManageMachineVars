using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ViewModel
{
    public class PerformanceModel
    {
        [JsonProperty("machineName")]
        public string MachineName { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("counterName")]
        public string CounterName { get; set; }

        [JsonProperty("instanceName")]
        public string InstanceName { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        public PerformanceModel()
        { }

        public PerformanceModel(PerformanceModel p)
        {
            MachineName = p.MachineName;
            CategoryName = p.CategoryName;
            CounterName = p.CounterName;
            InstanceName = p.InstanceName;
            Value = p.Value;
        }
    }

    public class SystemInfo
    {
        public string machineName { get; set; }
        public string cpu { get; set; }
        public int totalRam { get; set; }
        public string operatingSystem { get; set; }
        public string systemDriveLetter { get; set; }
        public double discSpaceUsed { get; set; }
        public double discSpaceAvailable { get; set; }
        public string webServer { get; set; }

        public SystemInfo()
        { }
    }
}