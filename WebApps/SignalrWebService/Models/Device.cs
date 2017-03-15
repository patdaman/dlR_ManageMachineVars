using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SignalrWebService.Models
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
}