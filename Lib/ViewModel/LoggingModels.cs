using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Device
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("device_name")]
        public string device_name { get; set; }

        [JsonProperty("ip_address")]
        public string ip_address { get; set; }

        [JsonProperty("create_date")]
        public System.DateTime create_date { get; set; }

        [JsonProperty("modify_date")]
        public Nullable<System.DateTime> modify_date { get; set; }

        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("Events")]
        public virtual ICollection<Event> Events { get; set; }

        public Device()
        { }

        public Device(Device device)
        {
            id = device.id;
            device_name = device.device_name;
            ip_address = device.ip_address;
            create_date = device.create_date;
            modify_date = device.modify_date;
            active = device.active;
            Events = device.Events;
        }
    }

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

