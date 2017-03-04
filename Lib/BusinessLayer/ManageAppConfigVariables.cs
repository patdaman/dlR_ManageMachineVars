using CommonUtils.AppConfiguration;
using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static ViewModel.ConfigModels;

namespace BusinessLayer
{
    public class ManageAppConfigVariables
    {
        public AppConfigFunctions appConfigVars { get; private set; }

        public int? machineId { get; set; }
        public string machineName { get; set; }
        public int? appId { get; set; }
        public string appName { get; set; }
        public int? componentId { get; set; }
        public string componentName { get; set; }

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
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
            if (String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(appName))
            {
                defaultPath = String.Format(@"D:\Apps\{0}\Config\AppConfig.xml", appName);
                path = defaultPath;
            }
            appConfigVars = new AppConfigFunctions(path);
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

        public ManageAppConfigVariables(string path, string machineName)
        {
            this.path = path;
            configFile = XDocument.Load(path);
            appConfigVars = new AppConfigFunctions(configFile);
            this.machineName = machineName;
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(string path, string machineName, string configEnvironment)
        {
            this.path = path;
            configFile = XDocument.Load(path);
            appConfigVars = new AppConfigFunctions(configFile);
            this.machineName = machineName;
            this.environment = configEnvironment;
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(string path, string machineName, string configEnvironment, string componentName, string appName)
        {
            this.path = path;
            configFile = XDocument.Load(path);
            appConfigVars = new AppConfigFunctions(configFile);
            this.machineName = machineName;
            this.environment = configEnvironment;
            this.componentName = componentName;
            this.appName = appName;
            DevOpsContext = new DevOpsEntities();
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
                        parentElement = cvar.parent_element,
                        element = cvar.element,
                        keyName = cvar.key_name,
                        key = cvar.key,
                        valueName = cvar.value_name,
                        value = val.value
                    });
                }
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Import all application configuration variables to database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/3/2017. </remarks>
        ///
        /// <exception cref="ObjectNotFoundException">  Thrown when an Object Not Found error condition
        ///                                             occurs. </exception>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ImportAllAppConfigVariablesToDb()
        {
            Component efComp;
            IQueryable<Component> CompQuery;
            if (componentId != null || !string.IsNullOrWhiteSpace(componentName))
            {
                if (componentId == null)
                    CompQuery = (from Comp in DevOpsContext.Components
                                where Comp.component_name.ToLower() == componentName.ToLower()
                                select Comp);
                else
                    CompQuery = (from Comp in DevOpsContext.Components
                                where Comp.id == componentId
                                select Comp);
                efComp = CompQuery.FirstOrDefault();
                componentId = efComp.id;
            }
            else
            {
                throw new ObjectNotFoundException("No matching Component found in database.");
            }

            Machine efMac;
            IQueryable<Machine> macQuery;
            if (machineId != null || !string.IsNullOrWhiteSpace(this.machineName))
            {
                if (machineId == null)
                    macQuery = (from Mac in DevOpsContext.Machines
                                 where Mac.machine_name.ToLower() == machineName.ToLower()
                                 select Mac);
                else
                    macQuery = (from Mac in DevOpsContext.Machines
                                 where Mac.id == machineId
                                 select Mac);
                efMac = macQuery.FirstOrDefault();
                machineId = efMac.id;
            }
            else
            {
                throw new ObjectNotFoundException("No matching Machine found in database.");
            }

            MachineComponentPath efMacCompPath = (from macComp in DevOpsContext.MachineComponentPaths
                                                  where macComp.machine_id == efMac.id
                                                  where macComp.component_id == efComp.id
                                                  where macComp.config_path == path
                                                  select macComp).FirstOrDefault();
            if (efMacCompPath == null)
                DevOpsContext.MachineComponentPaths.Add(new MachineComponentPath()
                                                            {
                                                                machine_id = efMac.id,
                                                                component_id = efComp.id,
                                                                config_path = path
                                                            });
            List<AttributeKeyValuePair> configVars = appConfigVars.ListConfigVariables(configFile);
            foreach (var x in configVars)
            {
                ConfigVariable newVar = CreateAppConfigEntity(x);
                efMac.ComponentConfigVariables.Add(new ComponentConfigVariable()
                {
                    component_id = (int)componentId,
                    machine_id = (int)machineId,
                    ConfigVariable = newVar
                });
            }
            DevOpsContext.SaveChanges();
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates application configuration entity. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/3/2017. </remarks>
        ///
        /// <param name="vars"> The variables. </param>
        ///
        /// <returns>   The new application configuration entity. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigVariable CreateAppConfigEntity(AttributeKeyValuePair vars)
        {
            List<ConfigVariableValue> valueList = new List<ConfigVariableValue>();
            valueList.Add(new ConfigVariableValue()
            {
                environment_type = environment.ToLower(),
                value = vars.value
            });
            ConfigVariable efConfigVar = new ConfigVariable()
            {
                parent_element = vars.parentElement,
                element = vars.element,
                key_name = vars.keyName,
                key = vars.key,
                value_name = vars.valueName,
                ConfigVariableValues = valueList,
                create_date = DateTime.UtcNow,
                active = true
            };
            return efConfigVar;
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
                var keyValue = appConfigVars.GetKeyValue(dbKey.key, dbKey.element);
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
                config.element = "Config File: " + config.element;

            List<AttributeKeyValuePair> dbKeyValues = ListAllAppConfigVariablesFromDb(appId);
            foreach (var db in dbKeyValues)
                db.element = "DB: " + db.element;

            List<AttributeKeyValuePair> combinedDiff = new List<AttributeKeyValuePair>();

            combinedDiff.AddRange( 
                        (from keys in keyValues
                        join db in dbKeyValues
                             on new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.element
                             }
                             equals
                             new
                             {
                                 JoinProperty1 = db.key,
                                 JoinProperty2 = db.value,
                                 JoinProperty3 = db.element
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
                                 JoinProperty3 = db.element
                             }
                             equals new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.element
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
                    resultList.Add(new ConfigModifyResult() { key = cvar.key, result = appConfigVars.RemoveKeyValue(cvar.element, cvar.key) });
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
                    resultList.Add(new ConfigModifyResult() { key = x.key, result = appConfigVars.UpdateOrCreateAppSetting(x.element, x.key, x.value_name, val, x.parent_element, x.element) });
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
            //IQueryable<Component> components;
            ICollection<ConfigVariable> configVars;

            if (appId == null)
            {
                //components = (from comp in DevOpsContext.Components
                //              where comp.MachineComponentPaths.Contains(
                //                  (from cfvar in DevOpsContext.MachineComponentPaths
                //                   where cfvar.machine_id == machineObject.FirstOrDefault().id
                //                   select cfvar).FirstOrDefault())
                //              select comp);
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.ComponentConfigVariables.FirstOrDefault().Component == appObject.FirstOrDefault().Components
                              where cvar.ComponentConfigVariables.FirstOrDefault().Machine == machineObject
                              select cvar).ToList();
            }
            else
            {
                //components = (from comp in DevOpsContext.Components
                //              where comp.Applications.Contains(
                //                  appObject.FirstOrDefault())
                //              //where comp.MachineComponentPaths.Contains(
                //              //    (from cfvar in DevOpsContext.MachineComponentPaths
                //              //     where cfvar.machine_id == machineObject.FirstOrDefault().id
                //              //     select cfvar).FirstOrDefault())
                //              select comp);
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.ComponentConfigVariables.FirstOrDefault().Machine == machineObject
                              select cvar).ToList();
            }
            return configVars;
        }
    }
}
