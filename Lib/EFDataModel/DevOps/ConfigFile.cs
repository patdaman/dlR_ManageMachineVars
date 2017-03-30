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
    
    public partial class ConfigFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConfigFile()
        {
            this.ConfigFileElements = new HashSet<ConfigFileElement>();
        }
    
        public int id { get; set; }
        public int component_id { get; set; }
        public string file_name { get; set; }
        public string environment { get; set; }
        public string xml_declaration { get; set; }
        public System.DateTime create_date { get; set; }
        public System.DateTime modify_date { get; set; }
    
        public virtual Component Component { get; set; }
        public virtual Enum_EnvironmentType Enum_EnvironmentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigFileElement> ConfigFileElements { get; set; }
    }
}
