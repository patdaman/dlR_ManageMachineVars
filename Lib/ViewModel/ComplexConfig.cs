using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
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
            keyName = x.key_name;
            keyName = x.key_name;
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

    public class AppVar
    {
        public string applicationNames { get; set; }
        public Nullable<int> componentId { get; set; }
        public string componentName { get; set; }
        public Nullable<int> varId { get; set; }
        public string configParentElement { get; set; }
        public string configElement { get; set; }
        //public string configAttribute { get; set; }
        public string keyName { get; set; }
        public string key { get; set; }
        public string valueName { get; set; }
        public List<ConfigVarValues> values { get; set; }

        public AppVar()
        { }
        public AppVar(AppVar x)
        {
            applicationNames = x.applicationNames;
            componentId = x.componentId;
            componentName = x.componentName;
            varId = x.varId;
            configParentElement = x.configParentElement;
            configElement = x.configElement;
            //configAttribute = x.configAttribute;
            keyName = x.keyName;
            key = x.key;
            valueName = x.valueName;
            values = x.values;
        }

        public AppVar(ConfigVariable x)
        {
            varId = x.id;
            configParentElement = x.parent_element;
            configElement = x.element;
            keyName = x.key_name;
            if (string.IsNullOrWhiteSpace(x.key))
                key = x.value_name;
            else
                key = x.key;
            valueName = x.value_name;
            values = new List<ConfigVarValues>();
        }
    }

    public class ConfigVarValues
    {
        public int id { get; set; }
        public int configvar_id { get; set; }
        public string environment { get; set; }
        public string value { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public Nullable<DateTime> publish_date { get; set; }
        public ConfigVarValues()
        { }
        public ConfigVarValues(ConfigVarValues x)
        {
            id = x.id;
            configvar_id = x.configvar_id;
            environment = x.environment;
            value = x.value;
            create_date = x.create_date;
            modify_date = x.modify_date;
            publish_date = x.publish_date;
        }
    }
}
