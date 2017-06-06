using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.EnvironmentVariables
{
    public class EnvVariable
    {
        public string key { get; set; }
        public string value { get; set; }
        public EnvironmentVariableTarget varType { get; set; }

        public EnvVariable()
        { }

        public EnvVariable(EnvVariable enVar)
        {
            this.key = enVar.key;
            this.value = enVar.value;
            this.varType = enVar.varType;
        }
    }

    public enum ModifyResult
    {
        Created,
        Updated,
        Removed,
        Failed,
        AccessDenied,
        NotFound,
        Unknown,
        Commented
    }
    public class ConfigModifyResult
    {
        public string key;
        public ModifyResult result;

        public ConfigModifyResult()
        { }

        public ConfigModifyResult(ConfigModifyResult c)
        {
            key = c.key;
            result = c.result;
        }
    }

    public class AttributeKeyValuePair
    {
        public string parentElement;
        public string fullElement;
        public string element;
        public string attribute;
        public string key;
        public string valueName;
        public string value;
        public ConfigModifyResult Result;

        public AttributeKeyValuePair()
        { }

        public AttributeKeyValuePair(AttributeKeyValuePair a)
        {
            parentElement = a.parentElement;
            fullElement = a.fullElement;
            element = a.element;
            attribute = a.attribute;
            key = a.key;
            valueName = a.valueName;
            value = a.value;
            Result = a.Result;
        }
    }
}
