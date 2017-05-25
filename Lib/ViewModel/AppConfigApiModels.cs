using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class NameValuePair
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public bool active { get; set; }
        public NameValuePair()
        {
            active = true;
        }
        public NameValuePair(NameValuePair x)
        {
            id = x.id;
            name = x.name;
            value = x.value;
            active = x.active;
        }
    }

    public class AppVar
    {
        public Nullable<int> configvar_id { get; set; }
        public string applicationNames { get; set; }
        public Nullable<int> componentId { get; set; }
        public string componentName { get; set; }
        public string fileName { get; set; }
        public string configParentElement { get; set; }
        public string fullElement { get; set; }
        public string configElement { get; set; }
        public string attribute { get; set; }
        public string key { get; set; }
        public string valueName { get; set; }
        public string userName { get; set; }
        public List<ConfigVariableValue> values { get; set; }
        public bool hasNotes { get; set; }
        public bool isGlobal { get; set; }

        public AppVar()
        {
            hasNotes = false;
            isGlobal = false;
        }
        public AppVar(AppVar x)
        {
            configvar_id = x.configvar_id;
            applicationNames = x.applicationNames;
            componentId = x.componentId;
            componentName = x.componentName;
            fileName = x.fileName;
            configParentElement = x.configParentElement;
            configElement = x.configElement;
            attribute = x.attribute;
            key = x.key;
            valueName = x.valueName;
            userName = x.userName;
            values = x.values;
            hasNotes = x.hasNotes;
            isGlobal = x.isGlobal;
        }

        public AppVar(ConfigVariable x)
        {
            configvar_id = x.id;
            configParentElement = x.parent_element;
            fullElement = x.full_element;
            configElement = x.element;
            attribute = x.attribute;
            fileName = x.ConfigFile.file_name;
            if (string.IsNullOrWhiteSpace(x.key))
                key = x.value_name;
            else
                key = x.key;
            valueName = x.value_name;
            values = new List<ConfigVariableValue>();
            hasNotes = false;
            isGlobal = false;
        }
    }

    public class ConfigXml
    {
        public Nullable<int> componentId { get; set; }
        public string componentName { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public string fileName { get; set; }
        public string text { get; set; }
        public ConfigXml()
        { }
        public ConfigXml(ConfigXml x)
        {
            componentId = x.componentId;
            componentName = x.componentName;
            title = x.title;
            path = x.path;
            fileName = x.fileName;
            text = x.text;
        }
    }

    public class ConfigFiles
    {
        public Nullable<int> fileId { get; set; }
        public string fileName { get; set; }
        public string path { get; set; }
        public Nullable<DateTime> createDate { get; set; }
        public Nullable<DateTime> modifyDate { get; set; }
        public string last_modify_user { get; set; }
        public ConfigFiles()
        { }
        public ConfigFiles(ConfigFiles c)
        {
            fileId = c.fileId;
            fileName = c.fileName;
            path = c.path;
            createDate = c.createDate;
            modifyDate = c.modifyDate;
            last_modify_user = c.last_modify_user;
        }
    }
}
