using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Id
    {
        public string type { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
    }

    public class MetricId
    {
        public string type { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
    }

    public class Timespan
    {
        public string type { get; set; }
        public string description { get; set; }
    }

    public class Aggregation
    {
        public string type { get; set; }
        public string description { get; set; }
    }

    public class Interval
    {
        public string type { get; set; }
        public string description { get; set; }
    }

    public class Segment
    {
        public string type { get; set; }
        public string description { get; set; }
    }

    public class Top
    {
        public string type { get; set; }
        public string description { get; set; }
        public Top() { }
    }

    public class Orderby
    {
        public string type { get; set; }
        public string description { get; set; }
        public Orderby() { }
    }

    public class Filter
    {
        public string type { get; set; }
        public string description { get; set; }
        public Filter() { }
    }

    public class Properties2
    {
        public MetricId metricId { get; set; }
        public Timespan timespan { get; set; }
        public Aggregation aggregation { get; set; }
        public Interval interval { get; set; }
        public Segment segment { get; set; }
        public Top top { get; set; }
        public Orderby orderby { get; set; }
        public Filter filter { get; set; }
        public Properties2() { }
    }

    public class Parameters
    {
        public string type { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public Properties2 properties { get; set; }
        public Parameters() { }
    }

    public class Properties
    {
        public Id id { get; set; }
        public Parameters parameters { get; set; }
        public Properties() { }
    }

    public class Items
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Items() { }
    }

    public class RootObject
    {
        public string title { get; set; }
        public string type { get; set; }
        public Items items { get; set; }
        public RootObject() { }

        public RootObject(RootObject r)
        {
            title = r.title;
            type = r.type;
            items = r.items;
        }
    }

}
