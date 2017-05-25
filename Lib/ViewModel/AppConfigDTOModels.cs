using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Enum_Locations
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool active { get; set; }
        public virtual List<Machine> Machines { get; set; }

        public Enum_Locations()
        { }

        public Enum_Locations(Enum_Locations e)
        {
            name = e.name;
            value = e.value;
            active = e.active;
            //Machines = e.Machines;
        }
    }

    public class Application
    {
        public int id { get; set; }
        public string application_name { get; set; }
        public string release { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public string last_modify_user { get; set; }
        public bool active { get; set; }
        public virtual List<Component> Components { get; set; }
        public virtual List<EnvironmentDtoVariable> EnvironmentVariables { get; set; }

        public Application()
        { }

        public Application(Application a)
        {
            id = a.id;
            application_name = a.application_name;
            release = a.release;
            create_date = a.create_date;
            modify_date = a.modify_date;
            last_modify_user = a.last_modify_user;
            active = a.active;
            Components = a.Components;
            //EnvironmentVariables = a.EnvironmentVariables;
        }
        public Application(ApplicationDto a)
        {
            Components = new List<Component>();
            id = a.id ?? 0;
            application_name = a.name;
            if (a.components != null)
            {
                foreach (var c in a.components)
                {
                    Components.Add(new Component(c));
                };
            }
            last_modify_user = a.last_modify_user ?? string.Empty;
            active = true;
        }
    }

    public class ApplicationDto
    {
        public ApplicationDto()
        {
            published = false;
        }
        public int? id { get; set; }
        public string name { get; set; }
        //public string components { get; set; }
        public List<ComponentDto> components { get; set; }
        public string release { get; set; }
        public string last_modify_user { get; set; }
        public bool published { get; set; }
        public ApplicationDto(ApplicationDto a)
        {
            id = a.id;
            name = a.name;
            release = a.release;
            last_modify_user = a.last_modify_user;
            published = a.published;
        }
    }

    public class Component
    {
        public int id { get; set; }
        public string component_name { get; set; }
        public string relative_path { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public string last_modify_user { get; set; }
        public bool active { get; set; }
        public virtual List<MachineComponentPath> MachineComponentPaths { get; set; }
        public virtual List<Application> Applications { get; set; }
        public virtual List<ConfigVariable> ConfigVariables { get; set; }
        public virtual List<ConfigFile> ConfigFiles { get; set; }

        public Component()
        { }

        public Component(Component c)
        {
            id = c.id;
            component_name = c.component_name;
            relative_path = c.relative_path;
            create_date = c.create_date;
            modify_date = c.modify_date;
            last_modify_user = c.last_modify_user;
            active = c.active;
            //MachineComponentPaths = c.MachineComponentPaths;
            //Applications = c.Applications;
            //ConfigVariables = c.ConfigVariables;
        }

        public Component(ComponentDto c)
        {
            id = c.id ?? 0;
            component_name = c.componentName ?? string.Empty;
            relative_path = c.filePath ?? string.Empty;
            last_modify_user = c.last_modify_user ?? string.Empty;
            active = true;
            MachineComponentPaths = new List<MachineComponentPath>();
            Applications = new List<Application>();
            if (c.applications != null)
            {
                foreach (var a in c.applications)
                {
                    Applications.Add(new Application(a));
                };
            }
        }
    }

    public class ComponentDto
    {
        public ComponentDto()
        {
            published = false;
        }
        public int? id { get; set; }
        public string componentName { get; set; }
        public string filePath { get; set; }
        public string last_modify_user { get; set; }
        public bool published { get; set; }
        public List<ApplicationDto> applications { get; set; }
        //public string applications { get; set; }

        public ComponentDto(ComponentDto c)
        {
            componentName = c.componentName;
            filePath = c.filePath;
            applications = c.applications;
            last_modify_user = c.last_modify_user;
            published = c.published;
        }
    }

    public class ConfigFile
    {
        public ConfigFile() { }

        public int id { get; set; }
        public int component_id { get; set; }
        public string file_name { get; set; }
        public string xml_declaration { get; set; }
        public System.DateTime create_date { get; set; }
        public System.DateTime modify_date { get; set; }
        public string last_modify_user { get; set; }
        public string root_element { get; set; }
        public Component Component { get; set; }
        //public ICollection<ConfigFileElement> ConfigFileElements { get; set; }
        
        public ConfigFile(ConfigFile c)
        {
            id = c.id;
            component_id = c.component_id;
            file_name = c.file_name;
            xml_declaration = c.xml_declaration;
            create_date = c.create_date;
            modify_date = c.modify_date;
            last_modify_user = c.last_modify_user;
            root_element = c.root_element;
            //Component = c.Component;
        }
    }

    public class ConfigVariable
    {
        public int id { get; set; }
        public string parent_element { get; set; }
        public string full_element { get; set; }
        public string element { get; set; }
        public string attribute { get; set; }
        public string key { get; set; }
        public string value_name { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public string last_modify_user { get; set; }
        public bool active { get; set; }
        public virtual ICollection<ConfigVariableValue> ConfigVariableValues { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public virtual ConfigFile ConfigFile { get; set; }

        public ConfigVariable()
        { }

        public ConfigVariable(ConfigVariable c)
        {
            id = c.id;
            parent_element = c.parent_element;
            full_element = c.full_element;
            element = c.element;
            attribute = c.attribute;
            key = c.key;
            value_name = c.value_name;
            create_date = c.create_date;
            modify_date = c.modify_date;
            last_modify_user = c.last_modify_user;
            active = c.active;
            //ConfigVariableValues = c.ConfigVariableValues;
            //Components = c.Components;
            ConfigFile = c.ConfigFile;
        }
    }

    public class ConfigVariableValue
    {
        public int configvar_id { get; set; }
        public Nullable<int> id { get; set; }
        public string environment { get; set; }
        public string value { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public string last_modify_user { get; set; }
        public Nullable<DateTime> publish_date { get; set; }
        public bool? published { get; set; }
        public virtual ConfigVariable ConfigVariable { get; set; }
        public virtual Enum_EnvironmentType Enum_EnvironmentType { get; set; }

        public ConfigVariableValue()
        { }

        public ConfigVariableValue(ConfigVariableValue c)
        {
            configvar_id = c.configvar_id;
            id = c.id;
            environment = c.environment;
            value = c.value;
            create_date = c.create_date;
            modify_date = c.modify_date;
            last_modify_user = c.last_modify_user;
            publish_date = c.publish_date;
            published = c.published;
            //ConfigVariable = c.ConfigVariable;
            //Enum_EnvironmentType = c.Enum_EnvironmentType;
            //Machine = c.Machine;
        }
    }

    public class Enum_EnvironmentType
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool active { get; set; }
        public virtual List<ConfigVariableValue> ConfigVariableValues { get; set; }

        public Enum_EnvironmentType()
        { }

        public Enum_EnvironmentType(Enum_EnvironmentType e)
        {
            name = e.name;
            value = e.value;
            active = e.active;
        }
    }
}