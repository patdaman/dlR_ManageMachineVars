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
        public string ip_address { get; set; }
        public string location { get; set; }
        public string usage { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public virtual List<ConfigVariableValue> ConfigVariableValues { get; set; }
        public virtual Enum_Locations Enum_Locations { get; set; }
        public virtual List<MachineComponentPath> MachineComponentPaths { get; set; }
        public virtual List<EnvironmentDtoVariable> EnvironmentVariables { get; set; }

        public Machine()
        { }

        public Machine(Machine m)
        {
            id = m.id;
            machine_name = m.machine_name;
            location = m.location;
            usage = m.usage;
            create_date = m.create_date;
            modify_date = m.modify_date;
            active = m.active;
            ConfigVariableValues = m.ConfigVariableValues;
            Enum_Locations = m.Enum_Locations;
            MachineComponentPaths = m.MachineComponentPaths;
            EnvironmentVariables = m.EnvironmentVariables;
        }
    }

    public class Enum_Locations
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool active { get; set; }
        public virtual List<Machine> Machines { get; set; }

        public Enum_Locations()
        { }
    }

    public class MachineGroup
    {
        public int id { get; set; }
        public string group_name { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }

        public MachineGroup()
        { }
    }

    public class Application
    {
        public int id { get; set; }
        public string application_name { get; set; }
        public string release { get; set; }
        public System.DateTime create_date { get; set; }
        public int configVariable_id { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public List<Machine> Machines { get; set; }
        public List<ConfigVariable> ConfigVars { get; set; }
        public virtual List<EnvironmentDtoVariable> EnvironmentVariables { get; set; }

        public Application()
        { }
    }

    public class Component
    {
        public int id { get; set; }
        public string component_name { get; set; }
        public string relative_path { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public virtual List<MachineComponentPath> MachineComponentPaths { get; set; }
        public virtual List<Application> Applications { get; set; }
        public virtual List<ConfigVariable> ConfigVariables { get; set; }

        public Component()
        { }
    }

    public class MachineComponentPath
    {
        public int machine_id { get; set; }
        public int component_id { get; set; }
        public string config_path { get; set; }
        public virtual Component Component { get; set; }
        public virtual Machine Machine { get; set; }

        public MachineComponentPath()
        { }
    }

    public class ConfigVariable
    {
        public int id { get; set; }
        public string parent_element { get; set; }
        public string element { get; set; }
        public string key_name { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool active { get; set; }
        public virtual List<ConfigVariableValue> ConfigVariableValues { get; set; }
        public virtual List<Component> Components { get; set; }

        public ConfigVariable()
        { }
    }

    public class ConfigVariableValue
    {
        public int id { get; set; }
        public int configvar_id { get; set; }
        public string environment_type { get; set; }
        public Nullable<int> machine_id { get; set; }
        public string value { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public virtual ConfigVariable ConfigVariable { get; set; }
        public virtual Enum_EnvironmentType Enum_EnvironmentType { get; set; }
        public virtual Machine Machine { get; set; }

        public ConfigVariableValue()
        { }
    }

    public class Enum_EnvironmentType
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool active { get; set; }
        public virtual List<ConfigVariableValue> ConfigVariableValues { get; set; }

        public Enum_EnvironmentType()
        { }
    }

    public class EnvironmentDtoVariable
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

        public EnvironmentDtoVariable()
        { }
    }
}