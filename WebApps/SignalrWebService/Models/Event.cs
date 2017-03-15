using Newtonsoft.Json;
using System;

namespace SignalrWebService.Models
{
    public class Event
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("device_id")]
        public Nullable<int> device_id { get; set; }

        [JsonProperty("Hostname")]
        public string Hostname { get; set; }

        [JsonProperty("Date")]
        public System.DateTime Date { get; set; }

        [JsonProperty("Time")]
        public System.TimeSpan Time { get; set; }

        [JsonProperty("Priority")]
        public string Priority { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Device")]
        public virtual Device Device { get; set; }

        public Event()
        { }

        public Event(Event e)
        {
            id = e.id;
            device_id = e.device_id;
            Hostname = e.Hostname;
            Date = e.Date;
            Time = e.Time;
            Priority = e.Priority;
            Message = e.Message;
        }
    }
}