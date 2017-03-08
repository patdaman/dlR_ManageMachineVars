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
                defaultPath = String.Format(@"D:\Apps\{0}\Config\App.Config", appName);
                path = defaultPath;
            }
            if (!string.IsNullOrWhiteSpace(path))
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
            Uri uri;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
            {
                configFile = XDocument.Load(uri.ToString());
            }
            else
                throw new UriFormatException("Path not a proper URI");
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(string path, string machineName)
        {
            this.path = path;
            Uri uri;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri) && !string.IsNullOrWhiteSpace(path))
            {
                configFile = XDocument.Load(uri.ToString());
            }
            appConfigVars = new AppConfigFunctions(configFile);
            this.machineName = machineName;
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(string path, string machineName, string configEnvironment)
        {
            this.path = path;
            Uri uri;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri) && !string.IsNullOrWhiteSpace(path))
            {
                configFile = XDocument.Load(uri.ToString());
                appConfigVars = new AppConfigFunctions(configFile);
            }
            else
                appConfigVars = new AppConfigFunctions();
            this.machineName = machineName;
            this.environment = configEnvironment;
            DevOpsContext = new DevOpsEntities();
        }

        public ManageAppConfigVariables(string path, string machineName, string configEnvironment, string componentName, string appName)
        {
            this.path = path;
            Uri uri;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri) && !string.IsNullOrWhiteSpace(path))
            {
                configFile = XDocument.Load(uri.ToString());
                appConfigVars = new AppConfigFunctions(configFile);
            }
            else
                appConfigVars = new AppConfigFunctions();
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

            ICollection<ConfigVariable> configVars = QueryConfigVariables(appId ?? 0);
            foreach (var cvar in configVars.ToList())
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
                ConfigVariable configVar;
                ConfigVariableValue efValue;
                IEnumerable<ConfigVariableValue> efValueList;
                configVar = (from n in DevOpsContext.ConfigVariables
                         where n.element == x.element
                         where n.key == x.key
                         where n.key_name == x.keyName
                         where n.parent_element == x.parentElement
                         where n.value_name == x.valueName
                         select n).FirstOrDefault();
                if (configVar == null)
                {
                    configVar = CreateAppConfigEntity(x);
                    efComp.ConfigVariables.Add(configVar);
                }
                else
                {
                    efValueList = configVar.ConfigVariableValues.Where(c => c.environment_type == environment).ToList();
                    if (efValueList == null)
                    {
                        configVar.ConfigVariableValues.Add(newConfigVariableValue(environment.ToLower(), x.value, configVar.id, null));
                        configVar.ConfigVariableValues.Add(newConfigVariableValue(environment.ToLower(), x.value, configVar.id, machineId));
                    }
                    else
                    {
                        efValue = efValueList.Where(v => v.machine_id == machineId).FirstOrDefault();
                        if (efValue == null)
                            configVar.ConfigVariableValues.Add(newConfigVariableValue(environment.ToLower(), x.value, configVar.id, machineId));
                        else
                        {
                            efValue.value = x.value;
                            efValue.modify_date = DateTime.Now;
                        }
                        if (efValueList.Where(v => v.machine_id == null).Count() < 1)
                            configVar.ConfigVariableValues.Add(newConfigVariableValue(environment.ToLower(), x.value, configVar.id, null));
                    }
                }
            }
            DevOpsContext.SaveChanges();
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new configuration variable value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/7/2017. </remarks>
        ///
        /// <param name="env">      The environment. </param>
        /// <param name="val">      The value. </param>
        /// <param name="con_id">   Identifier for the con. </param>
        /// <param name="mac_id">   Identifier for the MAC. </param>
        ///
        /// <returns>   A ConfigVariableValue. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ConfigVariableValue newConfigVariableValue(string env, string val, int con_id, int? mac_id)
        {
            return new ConfigVariableValue()
            {
                machine_id = mac_id,
                environment_type = env,
                configvar_id = con_id,
                value = val,
                create_date = DateTime.Now,
                modify_date = DateTime.Now
            };
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
        private ConfigVariable CreateAppConfigEntity(AttributeKeyValuePair vars)
        {
            List<ConfigVariableValue> valueList = new List<ConfigVariableValue>();
            valueList.Add(new ConfigVariableValue()
            {
                machine_id = null,
                environment_type = environment.ToLower(),
                value = vars.value,
                create_date = DateTime.Now,
            });
            valueList.Add(new ConfigVariableValue()
            {
                machine_id = machineId,
                environment_type = environment.ToLower(),
                value = vars.value,
                create_date = DateTime.Now,
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
                                 JoinProperty1 = keys.parentElement,
                                 JoinProperty2 = keys.element.Replace("Config File: ", ""),
                                 JoinProperty3 = keys.keyName,
                                 JoinProperty4 = keys.key,
                                 JoinProperty5 = keys.valueName,
                                 JoinProperty6 = keys.value
                             }
                             equals
                             new
                             {
                                 JoinProperty1 = db.parentElement,
                                 JoinProperty2 = db.element.Replace("DB: ", ""),
                                 JoinProperty3 = db.keyName,
                                 JoinProperty4 = db.key,
                                 JoinProperty5 = db.valueName,
                                 JoinProperty6 = db.value
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
                                 JoinProperty1 = db.parentElement,
                                 JoinProperty2 = db.element.Replace("DB: ", ""),
                                 JoinProperty3 = db.keyName,
                                 JoinProperty4 = db.key,
                                 JoinProperty5 = db.valueName,
                                 JoinProperty6 = db.value
                             }
                             equals new
                             {
                                 JoinProperty1 = keys.parentElement,
                                 JoinProperty2 = keys.element.Replace("Config File: ", ""),
                                 JoinProperty3 = keys.keyName,
                                 JoinProperty4 = keys.key,
                                 JoinProperty5 = keys.valueName,
                                 JoinProperty6 = keys.value
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
        public List<ConfigModifyResult> RemoveAllAppConfigVariables(string component)
        {
            var resultList = new List<ConfigModifyResult>();
            ICollection<ConfigVariable> configVars = QueryConfigVariables(componentName);
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
        /// <remarks>   Pdelosreyes, 3/6/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> AddAllAppConfigVariables()
        {
            ICollection<ConfigVariable> configVars = QueryConfigVariables();
            return AddAllAppConfigVariables(configVars);
        }

        public List<ConfigModifyResult> AddAllAppConfigVariables(int appId)
        {
            ICollection<ConfigVariable> configVars;
            if (appId == 0)
                configVars = QueryConfigVariables();
            else
                configVars = QueryConfigVariables(appId);
            return AddAllAppConfigVariables(configVars);
        }

        public List<ConfigModifyResult> AddAllAppConfigVariables(string component)
        {
            ICollection<ConfigVariable> configVars = QueryConfigVariables(component);
            return AddAllAppConfigVariables(configVars);
        }

        public List<ConfigModifyResult> AddAllAppConfigVariables(int appId, int componentId)
        {
            ICollection<ConfigVariable> configVars = QueryConfigVariables(appId, componentId);
            return AddAllAppConfigVariables(configVars);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="configVars">   The configuration variables. </param>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<ConfigModifyResult> AddAllAppConfigVariables(ICollection<ConfigVariable> configVars)
        {
            var resultList = new List<ConfigModifyResult>();

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
        /// <remarks>   Pdelosreyes, 3/6/2017. </remarks>
        ///
        /// <returns>   The configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ICollection<ConfigVariable> QueryConfigVariables()
        {
            return QueryConfigVariables(0, 0);
        }

        private ICollection<ConfigVariable> QueryConfigVariables(int appId)
        {
            return QueryConfigVariables(appId, 0);
        }

        private ICollection<ConfigVariable> QueryConfigVariables(string component)
        {
            int componentId = (from comp in DevOpsContext.Components
                                                     where comp.component_name == component
                                                     select comp).FirstOrDefault().id;
            return QueryConfigVariables(0, componentId);
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
        private ICollection<ConfigVariable> QueryConfigVariables(int appId, int componentId)
        {
            IQueryable<Machine> machineObject = (from mac in DevOpsContext.Machines
                                                 where mac.machine_name == machineName
                                                 select mac);
            IQueryable<Application> appObject = (from app in DevOpsContext.Applications
                                                 where app.id == appId
                                                 select app);
            ICollection<ConfigVariable> configVars;
            if (componentId != 0)
            {
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.Components.FirstOrDefault().id == componentId
                              select cvar).ToList();
            }
            else if (appId != 0)
            {
                configVars = (from cvar in DevOpsContext.ConfigVariables
                              where appObject.FirstOrDefault().Components.Contains(cvar.Components.FirstOrDefault())
                              select cvar).ToList();
            }
            else
            {
                configVars = (from comp in DevOpsContext.ConfigVariables
                              where comp.Components.Select(x => x.id).Intersect(
                                  machineObject.FirstOrDefault().MachineComponentPaths.Select(ci => ci.component_id)
                                  ).Any()
                              select comp).ToList();
            }
            return configVars;
        }
    }
}
