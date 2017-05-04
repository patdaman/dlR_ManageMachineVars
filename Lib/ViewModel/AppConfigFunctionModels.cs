using static ViewModel.Enums;

namespace ViewModel
{

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A key value pair. </summary>
    ///
    /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
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
}

