using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAppConfig_Compact
{
    public class Models
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   
        ///             A key value pair. 
        ///             Attribute required to find key property name
        /// </summary>
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

        public enum ModifyResult
        {
            Created,
            Updated,
            Removed,
            Failed,
            AccessDenied,
            NotFound,
            Unknown
        }
    }
}
