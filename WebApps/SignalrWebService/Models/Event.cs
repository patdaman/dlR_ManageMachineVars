using System;

namespace SignalrWebService.Models
{
    public class Event
    {
        public int id { get; set; }
        public Nullable<int> device_id { get; set; }
        public string Hostname { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public string Priority { get; set; }
        public string Message { get; set; }

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