﻿using BusinessLayer;
using CommonUtils.AppConfiguration;
using CommonUtils.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageConfigVariables
{
    public partial class Actions
    {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets application configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void GetAppConfigValue(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            AppConfigFunctions appConfigVars = new AppConfigFunctions(adminArgs.Path);
            ViewModel.ConfigModels.AttributeKeyValuePair configVars = appConfigVars.GetAppConfigValue(adminArgs.Key, adminArgs.Parent);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Parent Attribute = {0}", configVars.parentElement);
            Console.WriteLine("Attribute = {0}", configVars.element);
            Console.WriteLine("key name = {0}", configVars.keyName);
            Console.WriteLine("key = {0}", configVars.key);
            Console.WriteLine("value name = {0}", configVars.valueName);
            Console.WriteLine("value = {0}", configVars.value);
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the application configuration variable described by adminArgs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RemoveAppConfigVariable(AdminArgs adminArgs)
        {
            AppConfigFunctions appConfigVars = new AppConfigFunctions(adminArgs.Path);
            var configVars = appConfigVars.RemoveAppConfigVariable(adminArgs.Key, adminArgs.Parent);
            appConfigVars.configFile.Save(adminArgs.Path);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("key = {0}", configVars.key);
            Console.WriteLine("result = {0}", configVars.result);
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an application configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void AddAppConfigVariable(AdminArgs adminArgs)
        {
            AppConfigFunctions appConfigVars = new AppConfigFunctions(adminArgs.Path);
            var configVars = appConfigVars.AddAppConfigVariable(adminArgs.Key, adminArgs.Value, adminArgs.Parent);
            appConfigVars.configFile.Save(adminArgs.Path);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("key = {0}", configVars.key);
            Console.WriteLine("result = {0}", configVars.result);
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllDbConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables("", adminArgs.MachineName);
            var configVars = appConfigProcessor.ListAllAppConfigVariablesFromDb();
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Parent Attribute = {0}", x.parentElement);
                Console.WriteLine("Attribute = {0}", x.element);
                Console.WriteLine("key name = {0}", x.keyName);
                Console.WriteLine("Key = {0}", x.key);
                Console.WriteLine("Value name = {0}", x.valueName);
                Console.WriteLine("Value = {0}", x.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.ListAllAppConfigVariables();
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Parent Attribute = {0}", x.parentElement);
                Console.WriteLine("Attribute = {0}", x.element);
                Console.WriteLine("key name = {0}", x.keyName);
                Console.WriteLine("Key = {0}", x.key);
                Console.WriteLine("Value name = {0}", x.valueName);
                Console.WriteLine("Value = {0}", x.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all difference configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllDiffConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path, adminArgs.MachineName);
            var configVars = appConfigProcessor.ListAllAppConfigVariablesDifferentFromDb();
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Parent Attribute = {0}", x.parentElement);
                Console.WriteLine("Attribute = {0}", x.element);
                Console.WriteLine("key name = {0}", x.keyName);
                Console.WriteLine("Key = {0}", x.key);
                Console.WriteLine("Value name = {0}", x.valueName);
                Console.WriteLine("Value = {0}", x.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all application configuration variables described by adminArgs.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RemoveAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.RemoveAllAppConfigVariables(string.Empty);
            appConfigProcessor.configFile.Save(adminArgs.Path);
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result);
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void AddAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            List<ViewModel.ConfigModels.ConfigModifyResult> configVars = appConfigProcessor.AddAllAppConfigVariables(string.Empty);
            appConfigProcessor.configFile.Save(adminArgs.Path);
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result);
            }
            Console.WriteLine("-------------------------------------------");
        }

        public static void ImportAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path, adminArgs.MachineName, 
                                                                                        adminArgs.ConfigEnvironment, 
                                                                                        adminArgs.ComponentName, adminArgs.ApplicationName);
            List<ViewModel.ConfigModels.AttributeKeyValuePair> configVars = appConfigProcessor.ImportAllAppConfigVariablesToDb();
            Console.WriteLine("-------------------------------------------");
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Parent Attribute = {0}", x.parentElement);
                Console.WriteLine("Attribute = {0}", x.element);
                Console.WriteLine("key name = {0}", x.keyName);
                Console.WriteLine("Key = {0}", x.key);
                Console.WriteLine("Value name = {0}", x.valueName);
                Console.WriteLine("Value = {0}", x.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void AddAllEnvVariables(AdminArgs adminArgs)
        {
            List<ViewModel.ConfigModels.ConfigModifyResult> results = new List<ViewModel.ConfigModels.ConfigModifyResult>();
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            results = envProcessor.AddAllEnvVariables();

            foreach (var x in results)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all environment variables described by adminArgs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RemoveAllEnvVariables(AdminArgs adminArgs)
        {
            List<ViewModel.ConfigModels.ConfigModifyResult> results = new List<ViewModel.ConfigModels.ConfigModifyResult>();
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            results = envProcessor.RemoveAllEnvVariables();

            foreach (var x in results)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets environment value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void GetEnvValue(AdminArgs adminArgs)
        {
            EnvVariable keyValue = new EnvVariable();
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            keyValue = envProcessor.GetEnvValue(adminArgs.Key);

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("environment variable type: ", keyValue.varType);
            Console.WriteLine("key = {0}", keyValue.key);
            Console.WriteLine("result = {0}", keyValue.value);
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the environment variable described by adminArgs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RemoveEnvVariable(AdminArgs adminArgs)
        {
            ViewModel.Enums.ModifyResult result;
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            result = envProcessor.RemoveEnvVariable(adminArgs.Key, adminArgs.Parent);

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Parent Element: ", adminArgs.Parent ?? "NA");
            Console.WriteLine("key = {0}", adminArgs.Key);
            Console.WriteLine("result = {0}", result.ToString());
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an environment variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void AddEnvVariable(AdminArgs adminArgs)
        {
            ViewModel.Enums.ModifyResult result;
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            result = envProcessor.AddEnvVariable(adminArgs.Key, adminArgs.Value, adminArgs.KeyType);

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("environment variable type: ", adminArgs.KeyType ?? "User");
            Console.WriteLine("key = {0}", adminArgs.Key);
            Console.WriteLine("value = {0}", adminArgs.Value);
            Console.WriteLine("result = {0}", result.ToString());
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllEnvVariables(AdminArgs adminArgs)
        {
            List<EnvVariable> result;
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            result = envProcessor.GetAllEnvVariables();

            foreach (var env in result)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("environment variable type: ", env.varType.ToString() ?? "User");
                Console.WriteLine("key = {0}", env.key);
                Console.WriteLine("value = {0}", env.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all database environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllDbEnvVariables(AdminArgs adminArgs)
        {
            List<EnvVariable> result;
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            result = envProcessor.GetAllDbEnvVariables();

            foreach (var env in result)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("environment variable type: ", env.varType.ToString() ?? "User");
                Console.WriteLine("key = {0}", env.key);
                Console.WriteLine("value = {0}", env.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all database difference environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ListAllDbDiffEnvVariables(AdminArgs adminArgs)
        {
            List<EnvVariable> result;
            ManageEnvironmentVariables envProcessor = new ManageEnvironmentVariables();
            result = envProcessor.GetAllDiffEnvVariables();

            foreach (var env in result)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("environment variable type: ", env.varType.ToString() ?? "User");
                Console.WriteLine("key = {0}", env.key);
                Console.WriteLine("value = {0}", env.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }
    }
}
