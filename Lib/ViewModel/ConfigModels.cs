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
            public string attribute;
            public string key;
            public string value;
        }

        public class ConfigModifyResult
        {
            public string key;
            public ModifyResult result;
        }
    }
}
