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
    
    public partial class ConfigVariable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConfigVariable()
        {
            this.ConfigVariableValues = new HashSet<ConfigVariableValue>();
            this.Components = new HashSet<Component>();
        }
    
        public int id { get; set; }
        public string parent_element { get; set; }
        public string element { get; set; }
        public string attribute { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public System.DateTime create_date { get; set; }
        public System.DateTime modify_date { get; set; }
        public bool active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigVariableValue> ConfigVariableValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Component> Components { get; set; }
    }
}
