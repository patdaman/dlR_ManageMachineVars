//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFDataModel.DevOps
{
    using System;
    using System.Collections.Generic;
    
    public partial class vi_ConfigVariables
    {
        public int config_id { get; set; }
        public int component_id { get; set; }
        public string component_name { get; set; }
        public string environment_type { get; set; }
        public string parent_element { get; set; }
        public string element { get; set; }
        public string attribute { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public string value { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public Nullable<System.DateTime> published_date { get; set; }
        public Nullable<bool> published { get; set; }
    }
}
