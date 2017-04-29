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
            //machineName = Environment.MachineName.ToString();
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
            DevOpsContext = new DevOpsEntities();
        }

        public ManageConfig_AppVariables(DevOpsEntities entities, XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
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
        public List<AttributeKeyValuePair> ListAllAppConfigVariablesFromDb(int? componentId = null, string environment = null, string filename = null)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            if (!string.IsNullOrWhiteSpace(environment))
                this.environment = environment;
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars = QueryConfigVariables(componentId ?? 0, this.environment, filename);
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
            this.configFile = configFile;
            EFDataModel.DevOps.Component efComp = new EFDataModel.DevOps.Component();
            EFDataModel.DevOps.Component newComp = new EFDataModel.DevOps.Component();
            EFDataModel.DevOps.ConfigFile efConfigFile = new EFDataModel.DevOps.ConfigFile();
            //EFDataModel.DevOps.Application efApp = new EFDataModel.DevOps.Application();
            EFDataModel.DevOps.Application efApp = null;
            EFDataModel.DevOps.MachineComponentPathMap machinePath = new MachineComponentPathMap();
            EFDataModel.DevOps.Machine efMac = new EFDataModel.DevOps.Machine();

            if (!string.IsNullOrWhiteSpace(this.appName) && this.appName != @"[]")
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
            if (componentId != null)
                CompQuery = (from Comp in DevOpsContext.Components
                             where Comp.id == componentId
                             select Comp);
            else
                CompQuery = (from Comp in DevOpsContext.Components
                             where Comp.component_name.ToLower() == componentName.ToLower()
                             select Comp);
            efComp = CompQuery.FirstOrDefault();

            string file_name = Path.GetFileNameWithoutExtension(this.path);

            if (efComp == null)
            {
                if (!string.IsNullOrWhiteSpace(this.componentName))
                {
                    if (string.IsNullOrWhiteSpace(file_name))
                        file_name = this.componentName + ".config";
                    string xml_dec = string.Empty;
                    if (configFile.Declaration != null)
                        xml_dec = configFile.Declaration.ToString();
                    else
                        xml_dec = new XDeclaration("1.0", "utf-8", "yes").ToString();
                    efConfigFile = new EFDataModel.DevOps.ConfigFile()
                    {
                        file_name = file_name,
                        environment = this.environment,
                        xml_declaration = xml_dec,
                        root_element = configFile.Root.Name.ToString(),
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                    };
                    newComp = new EFDataModel.DevOps.Component()
                    {
                        component_name = this.componentName,
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                        active = true,
                        relative_path = Path.GetDirectoryName(this.path),
                    };
                    newComp.ConfigFiles.Add(efConfigFile);
                }
                else
                {
                    throw new ObjectNotFoundException("No Component found or created.");
                }
            }
            else
            {
                this.componentId = efComp.id;
                this.componentName = efComp.component_name;
                efConfigFile = efComp.ConfigFiles.Where(x => x.file_name == file_name && x.component_id == this.componentId && x.environment == this.environment).FirstOrDefault();
            }

            if (efConfigFile == null)
            {
                string xml_dec = string.Empty;
                if (configFile.Declaration != null)
                    xml_dec = configFile.Declaration.ToString();
                else
                    xml_dec = new XDeclaration("1.0", "utf-8", "yes").ToString();
                efConfigFile = new EFDataModel.DevOps.ConfigFile()
                {
                    file_name = file_name,
                    environment = this.environment,
                    xml_declaration = xml_dec,
                    root_element = configFile.Root.Name.ToString(),
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                };
                if (efComp != null)
                    efComp.ConfigFiles.Add(efConfigFile);
                else
                    newComp.ConfigFiles.Add(efConfigFile);
            }

            if (string.IsNullOrWhiteSpace(this.path))
                this.path = String.Format(@"PrintableConfig\{0}\{0}.config", this.componentName);

            IQueryable<EFDataModel.DevOps.Machine> macQuery;
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = string.Empty;
            macQuery = DevOpsContext.Machines.Where(x => x.machine_name.ToLower() == this.machineName.ToLower());
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
            }
            efMac = macQuery.FirstOrDefault();

            if (efMac != null)
            {
                machineId = efMac.id;
                machinePath = (from macComp in DevOpsContext.MachineComponentPathMaps
                               where macComp.machine_id == efMac.id
                               where macComp.config_path == path
                               select macComp).FirstOrDefault();
                if (machinePath == null)
                {
                    machinePath = new EFDataModel.DevOps.MachineComponentPathMap()
                    {
                        machine_id = this.machineId.Value,
                        config_path = Path.GetDirectoryName(this.path)
                    };
                    if (efComp.component_name != null)
                    {
                        efComp.MachineComponentPathMaps.Add(machinePath);
                    }
                    else
                    {
                        newComp.MachineComponentPathMaps.Add(machinePath);
                    }
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
                             where n.configfile_id == efConfigFile.id || n.configfile_id == 0
                             select n).FirstOrDefault();
                if (configVar == null)
                {
                    configVar = CreateAppConfigEntity(x);
                    configVar.ConfigFile = efConfigFile;
                    if (efComp != null)
                        efComp.ConfigVariables.Add(configVar);
                    else
                        newComp.ConfigVariables.Add(configVar);
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
                if (efComp != null)
                    if (!efComp.ConfigVariables.Contains(configVar))
                        efComp.ConfigVariables.Add(configVar);
                else
                    newComp.ConfigVariables.Add(configVar);
            }
            if (efComp != null)
            {
                if (efApp != null)
                    efComp.Applications.Add(efApp);
            }
            else
            {
                if (efApp != null)
                    newComp.Applications.Add(efApp);
                DevOpsContext.Components.Add(newComp);
            }

            DevOpsContext.SaveChanges();
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="component">    The component. </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddComponent(ViewModel.Component component)
        {
            EFDataModel.DevOps.Component efComponent = new EFDataModel.DevOps.Component()
            {
                active = component.active,
                component_name = component.component_name,
                create_date = component.create_date,
                id = component.id,
                modify_date = component.modify_date ?? DateTime.Now,
                relative_path = component.relative_path,
                MachineComponentPathMaps = new List<EFDataModel.DevOps.MachineComponentPathMap>(),
                ConfigFiles = new List<EFDataModel.DevOps.ConfigFile>(),
                Applications = new List<EFDataModel.DevOps.Application>(),
                ConfigVariables = new List<EFDataModel.DevOps.ConfigVariable>(),
            };
            List<EFDataModel.DevOps.Application> efApplications = new List<EFDataModel.DevOps.Application>();
            foreach (var a in component.Applications)
            {
                efComponent.Applications.Add(new EFDataModel.DevOps.Application()
                {
                    active = a.active,
                    application_name = a.application_name,
                    create_date = a.create_date,
                    modify_date = a.modify_date ?? DateTime.Now,
                    release = a.release,
                });
            }

            foreach (var f in component.ConfigFiles)
            {
                efComponent.ConfigFiles.Add(new EFDataModel.DevOps.ConfigFile()
                {
                    component_id = f.component_id,
                    create_date = f.create_date,
                    file_name = f.file_name,
                    modify_date = f.modify_date,
                    root_element = f.root_element,
                    xml_declaration = f.xml_declaration,
                });
            }
            foreach (var m in component.MachineComponentPaths)
            {
                efComponent.MachineComponentPathMaps.Add(new EFDataModel.DevOps.MachineComponentPathMap()
                {
                    component_id = m.component_id,
                    config_path = m.config_path,
                    machine_id = m.machine_id,
                });
            }
            foreach (var v in component.ConfigVariables)
            {
                efComponent.ConfigVariables.Add(new EFDataModel.DevOps.ConfigVariable()
                {
                    active = v.active,
                    attribute = v.attribute,
                    create_date = v.create_date,
                    element = v.element,
                    key = v.key,
                    modify_date = v.modify_date ?? DateTime.Now,
                    parent_element = v.parent_element,
                    value_name = v.value_name,
                    ConfigFile = efComponent.ConfigFiles.FirstOrDefault(),
                });
            }

            DevOpsContext.Components.Add(efComponent);
            DevOpsContext.SaveChanges();
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
                active = true,
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

        public List<ConfigModifyResult> AddAllAppConfigVariables(int componentId)
        {
            ICollection<EFDataModel.DevOps.ConfigVariable> configVars;
            if (appId == 0)
                configVars = QueryConfigVariables();
            else
                configVars = QueryConfigVariables(componentId);
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

        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(string componentName, string environment = null)
        {
            this.environment = environment;
            this.componentName = componentName;
            var componentId = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault().id;
            return QueryConfigVariables(componentId, 0);
        }

        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(int componentId, string environment = null)
        {
            this.environment = environment;
            return QueryConfigVariables(componentId, 0);
        }

        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(int componentId, string environment, string fileName)
        {
            this.environment = environment;
            return QueryConfigVariables(componentId, 0, 0, fileName);
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
        private ICollection<EFDataModel.DevOps.ConfigVariable> QueryConfigVariables(int componentId, int appId, int? machineId = null, string fileName = null)
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

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = "";
            if (componentId != 0)
            {
                if (!string.IsNullOrWhiteSpace(this.environment))
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  where cvar.Components.FirstOrDefault().id == componentId
                                  //where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower() || cvar.ConfigFile == null
                                  where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower()
                                  where environmentObjects.Contains(cvar)
                                  select cvar).ToList();
                else
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  //where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower() || cvar.ConfigFile == null
                                  where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower()
                                  where cvar.Components.FirstOrDefault().id == componentId
                                  select cvar).ToList();
            }
            else if (appId != 0)
            {
                if (!string.IsNullOrWhiteSpace(this.environment))
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  //where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower() || cvar.ConfigFile == null
                                  where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower()
                                  where appObject.FirstOrDefault().Components.Contains(cvar.Components.FirstOrDefault())
                                  select cvar).ToList();
                else
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  where appObject.FirstOrDefault().Components.Contains(cvar.Components.FirstOrDefault())
                                  //where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower() || cvar.ConfigFile == null
                                  where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower()
                                  where environmentObjects.Contains(cvar)
                                  select cvar).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(machineName))
            {
                if (!string.IsNullOrWhiteSpace(this.environment))
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  where environmentObjects.Contains(cvar)
                                  where cvar.Components.Select(x => x.id).Intersect(
                                  machineObject.FirstOrDefault().MachineComponentPathMaps.Select(ci => ci.component_id)
                                  ).Any()
                              select cvar).ToList();
                else
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  where cvar.Components.Select(x => x.id).Intersect(
                                      machineObject.FirstOrDefault().MachineComponentPathMaps.Select(ci => ci.component_id)
                                      ).Any()
                                  //where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower() || cvar.ConfigFile == null
                                  where cvar.ConfigFile.file_name.ToLower() == fileName.ToLower()
                                  select cvar).ToList();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.environment))
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  where environmentObjects.Contains(cvar)
                                  select cvar).ToList();
                else
                    configVars = (from cvar in DevOpsContext.ConfigVariables
                                  select cvar).ToList();
            }
            return configVars;
        }
    }
}
