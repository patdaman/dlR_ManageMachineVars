using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.AppConfiguration
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Used to provide config values from the command line. </summary>
    ///
    /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class CommandLineConfigProvider : IAppConfigurationProvider
    {

        private List<string> Args = null; 

        public CommandLineConfigProvider(string[] args)
        {
            Args = new List<string>(args);
        }


        public object GetValue(string key)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (Args == null)
                throw new Exception("Args was null. CommandLineConfigProvider was not configured correctly.");

            int index = GetIndex(key);
            object value = null; 
            if(index>-1)
            {
                value = Args[index + 1];
            }
            return value;
        }

        private int GetIndex(string key)
        {
            int index = -1;
            index = Args.IndexOf($"-{key}");
            if( index == -1)
                index = Args.IndexOf($"--{key}");
            if(index == -1)
                index = Args.IndexOf($"/{key}");
            return index;
        }
    }
}
