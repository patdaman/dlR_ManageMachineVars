using static ViewModel.Enums;

namespace ViewModel
{
    public class ConfigModels
    {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A key value pair. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public class AttributeKeyValuePair
        {
            public string parentElement;
            public string element;
            public string keyName;
            public string key;
            public string valueName;
            public string value;
            public ConfigModifyResult Result;

            public AttributeKeyValuePair()
            { }

            public AttributeKeyValuePair(AttributeKeyValuePair a)
            {
                parentElement = a.parentElement;
                element = a.element;
                keyName = a.keyName;
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
}
