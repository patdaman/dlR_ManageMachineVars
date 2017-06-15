using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Machine
    {
        public int id { get; set; }
        public string machine_name { get; set; }
        public string ip_address { get; set; }
        public string location { get; set; }
        public string environment { get; set; }
        public string uri { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public string last_modify_user { get; set; }
        public bool active { get; set; }
        public virtual Enum_Locations Enum_Locations { get; set; }
        public virtual List<MachineComponentPath> MachineComponentPaths { get; set; }

        public Machine()
        { }

        public Machine(Machine m)
        {
            id = m.id;
            machine_name = m.machine_name;
            ip_address = m.ip_address;
            location = m.location;
            environment = m.environment;
            create_date = m.create_date;
            modify_date = m.modify_date;
            last_modify_user = m.last_modify_user;
            active = m.active;
        }
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

        public MachineComponentPath(MachineComponentPath m)
        {
            machine_id = m.machine_id;
            component_id = m.component_id;
            config_path = m.config_path;
            //Component = m.Component;
            //Machine = m.Machine;
        }
    }

    public class MachineGroup
    {
        public int id { get; set; }
        public string group_name { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }

        public MachineGroup()
        { }

        public MachineGroup(MachineGroup m)
        {
            id = m.id;
            group_name = m.group_name;
            create_date = m.create_date;
            modify_date = m.modify_date;
        }
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
        public string last_modify_user { get; set; }
        public bool active { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }

        public EnvironmentDtoVariable()
        { }

        public EnvironmentDtoVariable(EnvironmentDtoVariable e)
        {
            id = e.id;
            key = e.key;
            value = e.value;
            type = e.type;
            path = e.path;
            create_date = e.create_date;
            modify_date = e.modify_date;
            last_modify_user = e.last_modify_user;
            active = e.active;
            //Applications = e.Applications;
            //Machines = e.Machines;
        }
    }

    public class MachineAppVars
    {
        public Nullable<int> machineId { get; set; }
        public string machine_name { get; set; }
        public string location { get; set; }
        public string usage { get; set; }
        public System.DateTime machineCreate_date { get; set; }
        public Nullable<System.DateTime> machineModify_date { get; set; }
        public bool machineActive { get; set; }
        public List<int> applicationIds { get; set; }
        public List<string> applicationNames { get; set; }
        public List<string> applicationReleases { get; set; }
        public Nullable<int> componentId { get; set; }
        public string componentName { get; set; }
        public Nullable<int> varId { get; set; }
        public string varType { get; set; }
        public string configParentElement { get; set; }
        public string configElement { get; set; }
        public string configAttribute { get; set; }
        public string keyName { get; set; }
        public string key { get; set; }
        public string configValue_name { get; set; }
        public string valueName { get; set; }
        public string value { get; set; }
        public string varPath { get; set; }
        public bool varActive { get; set; }
        public string envType { get; set; }
        public System.DateTime varCreate_date { get; set; }
        public Nullable<System.DateTime> varModify_date { get; set; }

        public MachineAppVars()
        { }
        public MachineAppVars(MachineAppVars x)
        {
            machineId = x.machineId;
            machine_name = x.machine_name;
            location = x.location;
            usage = x.usage;
            machineCreate_date = x.machineCreate_date;
            machineModify_date = x.machineModify_date;
            machineActive = x.machineActive;
            applicationIds = x.applicationIds;
            applicationNames = x.applicationNames;
            applicationReleases = x.applicationReleases;
            varId = x.varId;
            varType = x.varType;
            configElement = x.configElement;
            configAttribute = x.configAttribute;
            key = x.key;
            keyName = x.keyName;
            configValue_name = x.configValue_name;
            varPath = x.varPath;
            varActive = x.varActive;
            varCreate_date = x.varCreate_date;
            varModify_date = x.varModify_date;
            envType = x.envType;
        }

        public MachineAppVars(ConfigVariable x)
        {
            varId = x.id;
            varType = "AppConfig";
            configParentElement = x.parent_element;
            configElement = x.element;
            keyName = x.attribute;
            keyName = x.attribute;
            key = x.key;
            configValue_name = x.value_name;
            varActive = x.active;
            varCreate_date = x.create_date;
            varModify_date = x.modify_date;
        }

        public MachineAppVars(EnvironmentDtoVariable x)
        {

        }
    }
}
