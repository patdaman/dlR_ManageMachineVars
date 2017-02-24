using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static ViewModel.Enums;
using ViewModel;

namespace CommonUtils.EnvironmentVariables
{
    public class EnvironmentVariableFunctions
    {
        //-------------------------------------------------------------------------------------
        // Globals: 
        //-------------------------------------------------------------------------------------
        public string myVarSuffix { get; set; }
        public string keyName { get; set; }
        private string subkeyUser = @"Environment";
        private string subkeyMachine = @"System\CurrentControlSet\Control\Session Manager\Environment";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public EnvironmentVariableFunctions()
        {
            myVarSuffix = String.Empty;
            keyName = String.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="suffix">   (Optional) The suffix. </param>
        ///-------------------------------------------------------------------------------------------------

        public EnvironmentVariableFunctions(string key, string suffix = null)
        {
            myVarSuffix = suffix;
            keyName = key;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <returns>   all environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetAllEnvironmentVariables()
        {
            List<EnvVariable> envVariables = new List<EnvVariable>();
            envVariables.AddRange(GetMachineEnvironmentVariables());
            envVariables.AddRange(GetUserEnvironmentVariables());
            envVariables.AddRange(GetProcessEnvironmentVariables());
            return envVariables;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets machine environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <returns>   The machine environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetMachineEnvironmentVariables()
        {
            return GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <returns>   The user environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetUserEnvironmentVariables()
        {
            return GetEnvironmentVariables(EnvironmentVariableTarget.User);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets process environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <returns>   The process environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetProcessEnvironmentVariables()
        {
            return GetEnvironmentVariables(EnvironmentVariableTarget.Process);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets environment variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The environment variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public EnvVariable GetEnvironmentVariable(string key)
        {
            return GetEnVar(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets environment variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A SetConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult SetEnvironmentVariable(string key, string value, string keyType = null)
        {
            if (!string.IsNullOrEmpty(keyType) && keyType.ToLower() == "machine")
                return SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(keyType) && keyType.ToLower() == "process")
                return SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            return SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets environment variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        /// <param name="target">   Target for the. </param>
        ///
        /// <returns>   A SetConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult SetEnvironmentVariable(string key, string value, EnvironmentVariableTarget target)
        {
            return SetEnVariable(key, value, target);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the environment variable described by key. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="varType">  (Optional) Type of the variable. </param>
        ///
        /// <returns>   An ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult RemoveEnvironmentVariable(string key, string varType = null)
        {
            ModifyResult machineResult = new ModifyResult();
            ModifyResult userResult = new ModifyResult();
            ModifyResult processResult = new ModifyResult();

            if (varType == null || varType.ToLower() == "machine")
                machineResult = RemoveEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            if (varType == null || varType.ToLower() == "user")
                userResult = RemoveEnvironmentVariable(key, EnvironmentVariableTarget.User);
            if (varType == null || varType.ToLower() == "process")
                processResult = RemoveEnvironmentVariable(key, EnvironmentVariableTarget.Process);

            if (ModifyResult.Removed.In(machineResult, userResult, processResult))
            {
                return ModifyResult.Removed;
            }

            else if (ModifyResult.AccessDenied.In(machineResult, userResult, processResult))
            {
                return ModifyResult.AccessDenied;
            }

            else if (ModifyResult.NotFound.In(machineResult, userResult, processResult))
            {
                return ModifyResult.NotFound;
            }

            else if (ModifyResult.Failed.In(machineResult, userResult, processResult))
            {
                return ModifyResult.Failed;
            }

            return ModifyResult.Unknown;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the environment variable described by key. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="target">   Target for the. </param>
        ///
        /// <returns>   An ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ModifyResult RemoveEnvironmentVariable(string key, EnvironmentVariableTarget target)
        {
            return SetEnVariable(key, String.Empty, target);
        }


        /// =====================================================================================
        /// =====================================================================================
        ///  Begin Private Methods
        /// =====================================================================================
        /// =====================================================================================


        /// -------------------------------------------------------------------------------------
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="tgt">  . </param>
        ///
        /// <returns>   The environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<EnvVariable> GetEnvironmentVariables(EnvironmentVariableTarget tgt)
        {
            List<EnvVariable> envVarList = new List<EnvVariable>();

            foreach (System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables(tgt))
            {
                string name = (string)de.Key;
                object value = de.Value;
                if (String.IsNullOrEmpty(myVarSuffix) || name.Contains(myVarSuffix))
                {
                    EnvVariable envVar = new EnvVariable()
                    {
                        key = name,
                        value = value.ToString(),
                        varType = tgt,
                    };
                    envVarList.Add(envVar);
                }
            }
            return envVarList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets environment variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The en variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private EnvVariable GetEnVar(string key)
        {
            EnvVariable enVar = new EnvVariable();
            enVar = GetEnVarType(key, EnvironmentVariableTarget.Machine);
            if (!String.IsNullOrEmpty(enVar.key))
                return enVar;
            enVar = GetEnVarType(key, EnvironmentVariableTarget.User);
            if (!String.IsNullOrEmpty(enVar.key))
                return enVar;
            enVar = GetEnVarType(key, EnvironmentVariableTarget.Process);
            return enVar;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets en variable type. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="target">   Target for the. </param>
        ///
        /// <returns>   The en variable type. </returns>
        ///-------------------------------------------------------------------------------------------------
        private EnvVariable GetEnVarType(string key, EnvironmentVariableTarget target)
        {
            foreach (System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables(target))
            {
                if ((string)de.Key == key)
                {
                    return new EnvVariable()
                    {
                        key = key,
                        value = de.Value.ToString(),
                        varType = target
                    };
                }
            }
            return new EnvVariable();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check environment variable in registry. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="rk">   . </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        private bool CheckEnvVarInRegistry(RegistryKey rk)
        {
            bool exists = false;
            if (!String.IsNullOrEmpty(keyName) && rk.GetValue(keyName) != null)
                exists = true;
            return exists;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check environment variable in registry. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="rk">   . </param>
        /// <param name="name"> The name. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        private bool CheckEnvVarInRegistry(RegistryKey rk, string name)
        {
            bool exists = false;
            if (rk.GetValue(name) != null)
                exists = true;
            return exists;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets en variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        /// <param name="target">   Target for the. </param>
        ///
        /// <returns>   A SetConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ModifyResult SetEnVariable(string key, string value, EnvironmentVariableTarget target)
        {
            try
            {
                if (Environment.GetEnvironmentVariable(key) != null)
                {
                    Environment.SetEnvironmentVariable(key, value, target);
                    if (String.IsNullOrEmpty(value))
                        return ModifyResult.Removed;
                    else
                        return ModifyResult.Updated;
                }
                else
                {
                    if (String.IsNullOrEmpty(value))
                        return ModifyResult.NotFound;
                    Environment.SetEnvironmentVariable(key, value, target);
                    return ModifyResult.Created;
                }
            }
            catch (SecurityException)
            {
                return ModifyResult.AccessDenied;
            }
            catch (Exception)
            {
                return ModifyResult.Failed;
            }
            return ModifyResult.Unknown;
        }
    }

    public static class Ext
    {
        public static bool In<T>(this T val, params T[] values) where T : struct
        {
            return values.Contains(val);
        }
    }
}
