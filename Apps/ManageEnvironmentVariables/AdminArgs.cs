using System;
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
        public const string AddAllAppConfigVariables = "add-allappconfig";
        public const string RemoveAllAppConfigVariables = "remove-allappconfig";
        public const string ListAllAppConfigVariables = "get-allappconfig";
        public const string AddAppConfigVariable = "add-appconfig";
        public const string RemoveAppConfigVariable = "remove-appconfig";
        public const string GetAppConfigValue = "get-appconfigvalue";
    }

    public class AdminArgs
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
        public string MachineName { get; set; }
        public string Path { get; set; }
    }
}
