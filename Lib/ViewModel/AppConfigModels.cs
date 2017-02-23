using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Machine
    {
        public int id { get; set; }
        public string machine_name { get; set; }
        public string location { get; set; }
        public string usage { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public List<Application> Applications { get; set; }
        public List<EnvironmentVariable> EnVars { get; set; }
        public List<ConfigVariable> ConfigVars { get; set; }
    }

    public partial class MachineGroup
    {
        public int id { get; set; }
        public string group_name { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
    }

    public class Application
    {
        public int id { get; set; }
        public string name { get; set; }
        public string release { get; set; }
        public System.DateTime create_date { get; set; }
        public int configVariable_id { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public List<Machine> Machines { get; set; }
        public List<ConfigVariable> ConfigVars { get; set; }
        public List<EnvironmentVariable> EnVars { get; set; }
    }

    public class ConfigVariable
    {
        public int id { get; set; }
        public string element { get; set; }
        public string attribute { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public string value { get; set; }
        public string config_path { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public List<Application> Applications { get; set; }
        public List<Machine> Machines { get; set; }
    }

    public class EnvironmentVariable
    {
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string path { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public List<Application> Applications { get; set; }
        public List<Machine> Machines { get; set; }
    }
}