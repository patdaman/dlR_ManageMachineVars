﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageConfigVariables
{
    public class AdminActions
    {
        public const string AddAllEnvVariables = "add-allenvars";
        public const string RemoveAllEnvVariables = "remove-allenvars";
        public const string ListAllEnvVariables = "get-allenvars";
        public const string AddEnvVariable = "add-envar";
        public const string RemoveEnvVariable = "remove-envar";
        public const string GetEnvValue = "get-envalue";

        public const string ListAllAppConfigVariables = "get-allappconfig";
        public const string ListAllAppConfigVariablesFromDb = "get-dbappconfig";
        public const string ListAllDiffAppConfigVariables = "get-diffappconfig";
        public const string AddAllAppConfigVariables = "add-allappconfig";
        public const string RemoveAllAppConfigVariables = "remove-allappconfig";
        public const string AddAppConfigVariable = "add-appconfig";
        public const string RemoveAppConfigVariable = "remove-appconfig";
        public const string GetAppConfigValue = "get-appconfigvalue";
        public const string ImportAppConfig = "import-appconfig";
    }

    public class ConfigEnvironment
    {
        public const string User = "user";
        public const string Machine = "machine";
        public const string Session = "session";
        public const string Development = "development";
        public const string QA = "qa";
        public const string Production = "production";
        public const string DR = "dr";
    }

    public class AdminArgs
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
        public string ConfigEnvironment { get; set; }
        public string MachineName { get; set; }
        public string ApplicationName { get; set; }
        public string ComponentName { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
        public string Attribute { get; set; }
        public string KeyType { get; set; }
        public string KeyName { get; set; }
        public string Key { get; set; }
        public string ValueName { get; set; }
        public string Value { get; set; }
        public string CustomSuffix { get; set; }
    }
}
