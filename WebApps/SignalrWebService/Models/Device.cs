using System;
using System.Collections.Generic;

namespace SignalrWebService.Models
{
    public class Device
    {
        public int id { get; set; }
        public string device_name { get; set; }
        public string ip_address { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
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