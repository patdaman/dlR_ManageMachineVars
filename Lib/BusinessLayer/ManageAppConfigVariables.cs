using CommonUtils.AppConfiguration;
using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static ViewModel.ConfigModels;

namespace BusinessLayer
{
    public class ManageAppConfigVariables
    {
        public AppConfigFunctions appConfigVars { get; private set; }
        public string machineName { get; private set; }
        public string appName { get; set; }
        public string path { get; set; }
        public string environment { get; set; }
        public XDocument configFile { get; private set; }
        private string defaultPath { get; set; }

        DevOpsEntities DevOpsContext;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables()
        {
            appConfigVars = new AppConfigFunctions();
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
            if (String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(appName))
            {
                defaultPath = String.Format(@"D:\Apps\{0}\Config\AppConfig.xml", appName);
                path = defaultPath;
            }
            if (File.Exists(path))
            {
                configFile = XDocument.Load(path);
            }
            else
            {
                configFile = new XDocument();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="xDoc"> The document. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables(XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables(DevOpsEntities entities)
        {
            configFile = new XDocument();
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(DevOpsEntities entities, XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = entities;
        }

        public ManageAppConfigVariables(string path)
        {
            this.path = path;
            configFile = XDocument.Load(path);
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets application configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="key">          The key. </param>
        ///
        /// <returns>   The application configuration value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AttributeKeyValuePair GetAppConfigValue(string key, string keyName = null)
        {
            if (keyName == "connectionStrings" || keyName == "connstring" || keyName == "name")
                return appConfigVars.GetKeyValue(key, "connectionStrings");
            if (keyName == "appsetting" || keyName == "key" )
                return appConfigVars.GetKeyValue(key, "appSettings");
            return appConfigVars.GetKeyValue(key, keyName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the application configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="key">          The key. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigModifyResult RemoveAppConfigVariable(string attribute, string key)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = appConfigVars.RemoveKeyValue(attribute, key)
            };
            //configFile.Save(path);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an application configuration variable to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        /// <param name="parent_element">    The value. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigModifyResult AddAppConfigVariable(string key, string value, string parent_element = null)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = appConfigVars.AddKeyValue(key, value, parent_element)
            };
            configFile.Save(path);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables from database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="appId">    (Optional) Identifier for the application. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllAppConfigVariablesFromDb(int? appId = null)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            ICollection<ConfigVariable> configVars = QueryConfigVariables(appId);
            foreach (var cvar in configVars)
            {
                foreach (var val in cvar.ConfigVariableValues)
                {
                    keyValues.Add(new AttributeKeyValuePair()
                    {
                        attribute = cvar.parent_element,
                        key = cvar.key,
                        value = val.value
                    });
                }
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables from database that exist in config file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllMatchingAppConfigVariables(int? appId = null)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            var dbKeyValues = ListAllAppConfigVariablesFromDb(appId);

            foreach (var dbKey in dbKeyValues)
            {
                var keyValue = appConfigVars.GetKeyValue(dbKey.key, dbKey.attribute);
                if (keyValue == dbKey)
                    keyValues.Add(keyValue);
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables from database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="appId">    (Optional) Identifier for the application. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllAppConfigVariablesDifferentFromDb(int? appId = null)
        {
            List<AttributeKeyValuePair> keyValues = ListAllAppConfigVariables();
            foreach (var config in keyValues)
                config.attribute = "Config File: " + config.attribute;

            List<AttributeKeyValuePair> dbKeyValues = ListAllAppConfigVariablesFromDb(appId);
            foreach (var db in dbKeyValues)
                db.attribute = "DB: " + db.attribute;

            List<AttributeKeyValuePair> combinedDiff = new List<AttributeKeyValuePair>();

            combinedDiff.AddRange( 
                        (from keys in keyValues
                        join db in dbKeyValues
                             on new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.attribute
                             }
                             equals
                             new
                             {
                                 JoinProperty1 = db.key,
                                 JoinProperty2 = db.value,
                                 JoinProperty3 = db.attribute
                             }
                            into combined
                        from both in combined.DefaultIfEmpty()
                        where both == null
                        select keys).ToList());

            combinedDiff.AddRange(
                        (from db in dbKeyValues
                         join keys in keyValues
                             on new
                             {
                                 JoinProperty1 = db.key,
                                 JoinProperty2 = db.value,
                                 JoinProperty3 = db.attribute
                             }
                             equals new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.attribute
                             }
                             into combined
                         from both in combined.DefaultIfEmpty()
                         where both == null
                         select db).ToList());
            return combinedDiff;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllAppConfigVariables()
        {
            return appConfigVars.ListConfigVariables(configFile);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> RemoveAllAppConfigVariables(int? appId = null)
        {
            var resultList = new List<ConfigModifyResult>();
            ICollection<ConfigVariable> configVars = QueryConfigVariables(appId);
            foreach (var cvar in configVars)
            {
                // May not be necessary since inactive keys can be removed anyways
                // if (dbKey.active)
                {
                    resultList.Add(new ConfigModifyResult() { key = cvar.key, result = appConfigVars.RemoveKeyValue(cvar.attribute, cvar.key) });
                }
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> AddAllAppConfigVariables(int? appId = null)
        {
            var resultList = new List<ConfigModifyResult>();

            ICollection<ConfigVariable> configVars = QueryConfigVariables(appId);
            foreach (var x in configVars)
            {
                if (x.active)
                {
                    var val = (from y in x.ConfigVariableValues
                               where y.environment_type == environment
                               select y.value).FirstOrDefault();
                    resultList.Add(new ConfigModifyResult() { key = x.key, result = appConfigVars.UpdateOrCreateAppSetting(x.attribute, x.key, x.value_name, val, x.parent_element, x.element) });
                }
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Queries configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/2/2017. </remarks>
        ///
        /// <param name="appId">    (Optional) Identifier for the application. </param>
        ///
        /// <returns>   The configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ICollection<ConfigVariable> QueryConfigVariables(int? appId)
        {
            IQueryable<Machine> machineObject = (from mac in DevOpsContext.Machines
                                                 where mac.machine_name == machineName
                                                 select mac);
            IQueryable<Application> appObject = (from app in DevOpsContext.Applications
                                                 where app.id == appId
                                                 select app);
            IQueryable<MachineComponentPath> varPaths = (from cfvar in DevOpsContext.MachineComponentPaths
                                                         where cfvar.machine_id == machineObject.FirstOrDefault().id
                                                         select cfvar);
            IQueryable<Component> components;
            ICollection<ConfigVariable> configVars;

            if (appId == null)
            {
                components = (from comp in DevOpsContext.Components
                              where comp.MachineComponentPaths.Contains(
                                  varPaths.FirstOrDefault())
                              select comp);
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.Components == components
                              select cvar).ToList();
            }
            else
            {
                components = (from comp in DevOpsContext.Components
                              where comp.Applications.Contains(
                                  appObject.FirstOrDefault())
                              where comp.MachineComponentPaths.Contains(
                                  varPaths.FirstOrDefault())
                              select comp);
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.Components == components
                              select cvar).ToList();
            }
            return configVars;
        }
    }
}
