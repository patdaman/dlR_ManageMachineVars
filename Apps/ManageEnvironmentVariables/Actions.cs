using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageConfigVariables
{
    public partial class Actions
    {

        public static void AddAllEnvVariables(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void RemoveAllEnvVariables(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void GetAppConfigValue(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void RemoveAppConfigVariable(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.RemoveAppConfigVariable(adminArgs.Key, adminArgs.KeyType);
            appConfigProcessor.configFile.Save(adminArgs.Path);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("key = {0}", configVars.key);
            Console.WriteLine("result = {0}", configVars.result);
            Console.WriteLine("-------------------------------------------");
        }

        public static void AddAppConfigVariable(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.AddAppConfigVariable(adminArgs.Key, adminArgs.Value, adminArgs.KeyType);
            appConfigProcessor.configFile.Save(adminArgs.Path);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("key = {0}", configVars.key);
            Console.WriteLine("result = {0}", configVars.result);
            Console.WriteLine("-------------------------------------------");
        }

        public static void ListAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.ListAllAppConfigVariables();
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Attribute = {0}", x.attribute);
                Console.WriteLine("Key = {0}", x.key);
                Console.WriteLine("Value = {0}", x.value.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        public static void RemoveAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.RemoveAllAppConfigVariables();
            appConfigProcessor.configFile.Save(adminArgs.Path);
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result);
            }
            Console.WriteLine("-------------------------------------------");
        }

        public static void AddAllAppConfigVariables(AdminArgs adminArgs)
        {
            ManageAppConfigVariables appConfigProcessor = new ManageAppConfigVariables(adminArgs.Path);
            var configVars = appConfigProcessor.AddAllAppConfigVariables();
            appConfigProcessor.configFile.Save(adminArgs.Path);
            foreach (var x in configVars)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("key = {0}", x.key);
                Console.WriteLine("result = {0}", x.result);
            }
            Console.WriteLine("-------------------------------------------");
        }

        public static void GetEnvValue(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void RemoveEnvVariable(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void AddEnvVariable(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }

        public static void ListAllEnvVariables(AdminArgs adminArgs)
        {
            throw new NotImplementedException();
        }
    }
}
