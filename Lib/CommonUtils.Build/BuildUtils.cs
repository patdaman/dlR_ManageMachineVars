using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Build
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Utilities centered around builds and build configurations. </summary>
    ///
    /// <remarks>   Dtorres, 20151006. </remarks>
    ///             -------------------------------------------------------------------------------------------------

    public class BuildUtils
    {
        public static string CurrentConfiguration
        {
            get
            {
                string config = "UNDEFINED"; //Should never be this.
#if DEBUG
                        config = "DEBUG";
#elif STAGING
                        config = "STAGING";
#elif BETA
                        config = "BETA";
#elif QA
                        config = "QA";
#elif PRODUCTION_RELEASE
                config = "PROD";
#elif DEVAPP
                config = "DEVAPP";
#elif RELEASE
                config = "RELEASE";
#elif SIGNALCI
                config = "SIGNALCI";   
#endif
                return config;
            }
        }
    }
}
