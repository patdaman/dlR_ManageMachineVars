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
    
    public partial class usp_GetConfigVariables_Result
    {
        public int config_id { get; set; }
        public int machine_id { get; set; }
        public string machine_name { get; set; }
        public Nullable<int> application_id { get; set; }
        public string application_name { get; set; }
        public int component_id { get; set; }
        public string component_name { get; set; }
        public string environment_type { get; set; }
        public string parent_element { get; set; }
        public string element { get; set; }
        public string key_name { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public string value { get; set; }
        public string config_path { get; set; }
    }
}
