using CommonUtils.AppConfiguration;
using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ViewModel;

namespace BusinessLayer
{
    public class ManageConfig_AppVariables
    {
        public AppConfigFunctions appConfigVars { get; private set; }

        public int? machineId { get; set; }
        public string machineName { get; set; }
        public int? appId { get; set; }
        public string appName { get; set; }
        public int? componentId { get; set; }

        public List<AttributeKeyValuePair> GetPublishValues()
        {
            throw new NotImplementedException();
        }

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
        public ManageConfig_AppVariables()
        {
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
            if (String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(componentName))
            {
                defaultPath = String.Format(@"D:\PrintableConfig\{0}\{0}.Config", componentName);
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
        public ManageConfig_AppVariables(XDocument xDoc)
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
        public ManageConfig_AppVariables(DevOpsEntities entities)
        {
            configFile = new XDocument();
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        public ManageConfig_AppVariables(DevOpsEntities entities, XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = entities;
        }

        public ManageConfig_AppVariables(string path)
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

        public ManageConfig_AppVariables(string path, string machineName)
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

        public ManageConfig_AppVariables(string path, string machineName, string configEnvironment)
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

        public ManageConfig_AppVariables(string path, string machineName, string configEnvironment, string componentName, string appName)
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
        public List<AttributeKeyValuePair> ListAllAppConfigVariablesFromDb(int? componentId = null, string environment = null)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            if (!string.IsNullOrWhiteSpace(environment))
                this.environment = environment;
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars = QueryConfigVariables(componentId ?? 0);
            foreach (var cvar in configVars.ToList())
            {
                foreach (var val in cvar.ConfigVariableValues)
                {
                    keyValues.Add(new AttributeKeyValuePair()
                    {
                        parentElement = cvar.parent_element,
                        element = cvar.element,
                        attribute = cvar.attribute,
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
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ImportAllAppConfigVariablesToDb()
        {
            if (this.configFile == null)
                throw new FileNotFoundException("No config file loaded.");
            return ImportAllAppConfigVariablesToDb(this.configFile);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Import all application configuration variables to database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
        ///
        /// <param name="filePath"> Full pathname of the file. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ImportAllAppConfigVariablesToDb(string filePath)
        {
            this.path = filePath;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                Uri uri;
                if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
                {
                    this.configFile = XDocument.Load(uri.ToString());
                }
                else
                    throw new UriFormatException("Path not a proper URI");
            }
            return ImportAllAppConfigVariablesToDb(this.configFile);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Import all application configuration variables to database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/3/2017. </remarks>
        ///
        /// <exception cref="ObjectNotFoundException">  Thrown when an Object Not Found error condition
        ///                                             occurs. </exception>
        ///
        /// <param name="configFile">   The configuration file. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ImportAllAppConfigVariablesToDb(XDocument configFile)
        {
            this.configFile = configFile;
            EFDataModel.DevOps.Component efComp = new EFDataModel.DevOps.Component();
            EFDataModel.DevOps.Component newComp = new EFDataModel.DevOps.Component();
            EFDataModel.DevOps.ConfigFile efConfigFile = new ConfigFile();
            EFDataModel.DevOps.Application efApp = new EFDataModel.DevOps.Application();

            if (!string.IsNullOrWhiteSpace(this.appName))
            {
                efApp = DevOpsContext.Applications.Where(x => x.application_name == this.appName).FirstOrDefault();
                if (efApp == null)
                    efApp = new EFDataModel.DevOps.Application()
                    {
                        active = true,
                        application_name = this.appName,
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                        release = ""
                    };
            }

            IQueryable<EFDataModel.DevOps.Component> CompQuery;
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
                this.componentId = efComp.id;
                this.componentName = efComp.component_name;
            }

            EFDataModel.DevOps.Machine efMac;
            IQueryable<EFDataModel.DevOps.Machine> macQuery;
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
                EFDataModel.DevOps.MachineComponentPathMap machinePath;

                if (efComp == null)
                {
                    if (!string.IsNullOrWhiteSpace(this.componentName))
                    {
                        machinePath = new EFDataModel.DevOps.MachineComponentPathMap()
                        {
                            machine_id = this.machineId.Value,
                            config_path = Path.GetDirectoryName(this.path)
                        };
                        string file_name = Path.GetFileNameWithoutExtension(this.path);
                        if (string.IsNullOrWhiteSpace(file_name))
                            file_name = this.componentName + ".config";
                        efConfigFile = new ConfigFile()
                        {
                            file_name = file_name,
                            xml_declaration = configFile.Declaration.ToString() ?? new XDeclaration("1.0", "utf-8", "yes").ToString(),
                            create_date = DateTime.Now,
                            modify_date = DateTime.Now,
                        };
                        if (string.IsNullOrWhiteSpace(this.path))
                            this.path = String.Format(@"PrintableConfig\{0}\{0}.config", this.componentName);
                        newComp = new EFDataModel.DevOps.Component()
                        {
                            component_name = this.componentName,
                            create_date = DateTime.Now,
                            modify_date = DateTime.Now,
                            active = true,
                            relative_path = this.path
                        };
                        newComp.MachineComponentPathMaps.Add(machinePath);
                        newComp.ConfigFiles.Add(efConfigFile);
                        if (efApp != null)
                            newComp.Applications.Add(efApp);
                        DevOpsContext.Components.Add(newComp);
                    }
                    else
                    {
                        throw new ObjectNotFoundException("No Component found or created.");
                    }
                }
                else
                {
                    machinePath = (from macComp in DevOpsContext.MachineComponentPathMaps
                                   where macComp.machine_id == efMac.id
                                   where macComp.component_id == efComp.id
                                   where macComp.config_path == path
                                   select macComp).FirstOrDefault();
                    if (machinePath == null || machinePath.config_path != this.path)
                    {
                        efComp.MachineComponentPathMaps.Add(new EFDataModel.DevOps.MachineComponentPathMap()
                        {
                            machine_id = this.machineId.Value,
                            component_id = efComp.id,
                            config_path = Path.GetDirectoryName(this.path)
                        });
                    }
                }
            }

            if (efComp == null)
            {
                if (!string.IsNullOrWhiteSpace(this.componentName))
                {
                    string file_name = Path.GetFileNameWithoutExtension(this.path);
                    if (string.IsNullOrWhiteSpace(file_name))
                        file_name = this.componentName + ".config";
                    efConfigFile = new ConfigFile()
                    {
                        file_name = file_name,
                        xml_declaration = configFile.Declaration.ToString() ?? new XDeclaration("1.0", "utf-8", "yes").ToString(),
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                    };
                    if (string.IsNullOrWhiteSpace(this.path))
                        this.path = String.Format(@"PrintableConfig\{0}\{0}.config", this.componentName);
                    newComp = new EFDataModel.DevOps.Component()
                    {
                        component_name = this.componentName,
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                        active = true,
                        relative_path = this.path
                    };
                    newComp.ConfigFiles.Add(efConfigFile);
                    if (efApp != null)
                        newComp.Applications.Add(efApp);
                    DevOpsContext.Components.Add(newComp);
                }
                else
                {
                    throw new ObjectNotFoundException("No Component found or created.");
                }
            }

            List<AttributeKeyValuePair> configVars = appConfigVars.ListConfigVariables(configFile);
            foreach (var x in configVars)
            {
                EFDataModel.DevOps.ConfigVariable configVar;
                EFDataModel.DevOps.ConfigVariableValue efValue;
                configVar = (from n in DevOpsContext.ConfigVariables
                             where n.element == x.element
                             where n.attribute == x.attribute
                             where n.key == x.key
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
                    efValue = configVar.ConfigVariableValues.Where(c => c.environment_type == environment).FirstOrDefault();
                    if (efValue == null)
                        configVar.ConfigVariableValues.Add(
                            new EFDataModel.DevOps.ConfigVariableValue()
                            {
                                environment_type = environment.ToLower(),
                                configvar_id = configVar.id,
                                value = x.value,
                                create_date = DateTime.Now,
                                modify_date = DateTime.Now
                            });
                    else
                    {
                        efValue.value = x.value;
                        efValue.modify_date = DateTime.Now;
                    }
                }

                /// ----------------------------------------------------------------------- ///
                ///   REFACTOR!!!!
                ///   
                /// Todo:
                ///  Add import of element / attribute structure to save all file detail
                ///     in database: [DevOps].[config].[ConfigFile]
                ///         etc
                ///  Minimize footprint of this gawdawful method!      
                ///   REFACTOR!!!!
                ///  ---------------------------------------------------------------------- ///

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
        private EFDataModel.DevOps.ConfigVariable CreateAppConfigEntity(AttributeKeyValuePair vars)
        {
            List<EFDataModel.DevOps.ConfigVariableValue> valueList = new List<EFDataModel.DevOps.ConfigVariableValue>();
            List<EFDataModel.DevOps.Enum_EnvironmentType> environments = new List<EFDataModel.DevOps.Enum_EnvironmentType>();

            if (string.IsNullOrWhiteSpace(this.environment))
                environments.AddRange(DevOpsContext.Enum_EnvironmentType.ToList());
            else
            {
                environments.Add(new EFDataModel.DevOps.Enum_EnvironmentType() { name = this.environment.ToLower(), value = this.environment });
            }

            foreach (var environment in environments)
            {
                valueList.Add(new EFDataModel.DevOps.ConfigVariableValue()
                {
                    environment_type = environment.name,
                    value = vars.value,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    published = false
                });
            }
            EFDataModel.DevOps.ConfigVariable efConfigVar = new EFDataModel.DevOps.ConfigVariable()
            {
                parent_element = vars.parentElement,
                element = vars.element,
                attribute = vars.attribute,
                key = vars.key,
                value_name = vars.valueName,
                ConfigVariableValues = valueList,
                create_date = DateTime.Now,
                modify_date = DateTime.Now,
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
                                          JoinProperty3 = keys.attribute,
                                          JoinProperty4 = keys.key,
                                          JoinProperty5 = keys.valueName,
                                          JoinProperty6 = keys.value
                                      }
                                      equals
                                      new
                                      {
                                          JoinProperty1 = db.parentElement,
                                          JoinProperty2 = db.element.Replace("DB: ", ""),
                                          JoinProperty3 = db.attribute,
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
                                 JoinProperty3 = db.attribute,
                                 JoinProperty4 = db.key,
                                 JoinProperty5 = db.valueName,
                                 JoinProperty6 = db.value
                             }
                             equals new
                             {
                                 JoinProperty1 = keys.parentElement,
                                 JoinProperty2 = keys.element.Replace("Config File: ", ""),
                                 JoinProperty3 = keys.attribute,
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
            this.componentId = DevOpsContext.Components.Where(x => x.component_name == component).FirstOrDefault().id;
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars = QueryConfigVariables(componentId.Value);
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
        public List<ConfigModifyResult> AddAllAppConfigVariables(string environment = null)
        {
            if (string.IsNullOrWhiteSpace(environment))
                this.environment = environment;
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars = QueryConfigVariables();
            return AddAllAppConfigVariables(configVars);
        }

        //public List<ConfigModifyResult> AddAllAppConfigVariables(int componentId)
        //{
        //    ICollection<ConfigVariable> configVars;
        //    if (appId == 0)
        //        configVars = QueryConfigVariables();
        //    else
        //        configVars = QueryConfigVariables(componentId);
        //    return AddAllAppConfigVariables(configVars);
        //}

        //public List<ConfigModifyResult> AddAllAppConfigVariables(int appId, int componentId)
        //{
        //    ICollection<ConfigVariable> configVars = QueryConfigVariables(appId, componentId);
        //    return AddAllAppConfigVariables(configVars);
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="configVars">   The configuration variables. </param>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<ConfigModifyResult> AddAllAppConfigVariables(ICollection<EFDataModel.DevOps.ConfigVariable> configVars)
        {
            var resultList = new List<ConfigModifyResult>();

            foreach (var x in configVars)
            {
                if (x.active)
                {
                    var val = (from y in x.ConfigVariableValues
                               where y.environment_type == environment
                               select y.value).FirstOrDefault();
                    resultList.Add(new ConfigModifyResult() { key = x.key, result = appConfigVars.AddKeyValue(x.attribute, x.key, x.value_name, val, x.element, x.parent_element) });
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
        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables()
        {
            return QueryConfigVariables(0, 0);
        }

        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(int componentId, string environment = null)
        {
            return QueryConfigVariables(componentId, 0);
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
        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(int componentId, int appId)
        {
            IQueryable<EFDataModel.DevOps.Machine> machineObject = (from mac in DevOpsContext.Machines
                                                                    where mac.machine_name == machineName
                                                                    select mac);
            IQueryable<EFDataModel.DevOps.Machine> envMachineObjects = (from mac in DevOpsContext.Machines
                                                                        where mac.usage == environment
                                                                        select mac);
            IQueryable<EFDataModel.DevOps.Application> appObject = (from app in DevOpsContext.Applications
                                                                    where app.id == appId
                                                                    select app);
            IQueryable<EFDataModel.DevOps.ConfigVariable> environmentObjects = (from config in DevOpsContext.ConfigVariableValues
                                                                                where config.environment_type == environment
                                                                                select config.ConfigVariable);
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars;
            //if (!string.IsNullOrWhiteSpace(environment))
            //{
            //    configVars = (from cvar in DevOpsContext.ConfigVariables
            //                  where environmentObjects.Contains(cvar)
            //                  select cvar).ToList();
            //}
            //else 
            if (componentId != 0)
            {
                if (!string.IsNullOrWhiteSpace(this.environment))
                   configVars = (from cvar in DevOpsContext.ConfigVariables
                              where cvar.Components.FirstOrDefault().id == componentId
                              where environmentObjects.Contains(cvar)
                              select cvar).ToList();
                else
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
                                  machineObject.FirstOrDefault().MachineComponentPathMaps.Select(ci => ci.component_id)
                                  ).Any()
                              select comp).ToList();
            }
            return configVars;
        }
    }
}
