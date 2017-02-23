using System;
using System.Collections.Generic;
using CommonUtils.EnvironmentVariables;
using static ViewModel.Enums;
using EFDataModel.DevOps;
using static ViewModel.ConfigModels;

namespace ManageConfigVariables
{
    public class ManageEnvironmentVariables
    {
        public EnvironmentVariableFunctions enVars { get; private set; }
        public string machineName { get; private set; }
        DevOpsEntities DevOpsContext;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageEnvironmentVariables()
        {
            enVars = new EnvironmentVariableFunctions();
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets environment value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The environment value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public EnvVariable GetEnvValue(string key)
        {
            return enVars.GetEnvironmentVariable(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the environment variable described by key. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   A ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult RemoveEnvVariable(string key)
        {
            return enVars.RemoveEnvironmentVariable(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an environment variable to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult AddEnvVariable(string key, string value)
        {
            return enVars.SetEnvironmentVariable(key, value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   all environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetAllEnvVariables()
        {
            return enVars.GetAllEnvironmentVariables();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigVariableResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ModifyResult> RemoveAllEnvVariables()
        {
            var keyList = new List<string>();
            var resultList = new List<ModifyResult>();
            foreach (var key in keyList)
            { 
                resultList.Add(enVars.RemoveEnvironmentVariable(key));
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigVariableResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ModifyResult> AddAllEnvVariables()
        {
            var keyValueList = new List<AttributeKeyValuePair>();
            var resultList = new List<ModifyResult>();
            foreach (var keyValue in keyValueList)
            {
                resultList.Add(enVars.SetEnvironmentVariable(keyValue.key, keyValue.value));
            }
            return resultList;
        }
    }
}
