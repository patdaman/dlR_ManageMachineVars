using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ViewModel
{
    public partial class ApplicationInsights
    {
        [JsonProperty("value")]
        public Value value { get; set; }

        public class MachineMetric
        {
            [JsonProperty("avg")]
            public int? Avg { get; set; }
            [JsonProperty("min")]
            public int? Min { get; set; }
            [JsonProperty("max")]
            public int? Max { get; set; }
            [JsonProperty("stdDev")]
            public int? stdDev { get; set; }
        }

        public class Segment
        {
            [JsonProperty("start")]
            public string Start { get; set; }
            [JsonProperty("end")]
            public string End { get; set; }
            [JsonProperty("requests/duration")]
            public MachineMetric RequestsDuration { get; set; }
            [JsonProperty("users/count")]
            public MachineMetric UsersCount { get; set; }
            [JsonProperty("requests/count")]
            public MachineMetric RequestsCount { get; set; }
            [JsonProperty("exceptions/count")]
            public MachineMetric ExceptionsCount { get; set; }
            [JsonProperty("performanceCounters/processorCpuPercentage")]
            public MachineMetric ProcessorCpuPercentage { get; set; }
            [JsonProperty("performanceCounters/memoryAvailableBytes")]
            public MachineMetric MemoryAvailableBytes { get; set; }
        }

        public class Value
        {
            [JsonProperty("start")]
            public string Start { get; set; }
            [JsonProperty("end")]
            public string End { get; set; }
            [JsonProperty("interval")]
            public string Interval { get; set; }
            [JsonProperty("requests/duration")]
            public MachineMetric RequestsDuration { get; set; }

            [JsonProperty("segments")]
            public Segment[] Segments { get; set; }
        }
    }

    public class ApplicationInsightRequest
    {
        public string appId { get; set; }
        public string apiKey { get; set; }
        public string resourceName { get; set; }
        public string resourceType { get; set; }
        public string resourceGroup { get; set; }
        public string subscriptionId { get; set; }
        public string requestType { get; set; }
        public string interval { get; set; }
        public string filter { get; set; }
        public string sort { get; set; }
        public string baseUrl { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string parameters { get; set; }
        public string message { get; set; }
        public ApplicationInsightRequest()
        { }

    }
}
