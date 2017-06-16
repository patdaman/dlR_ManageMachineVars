using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModel.DevOps;
using ViewModel;

namespace BusinessLayer
{
    public class ManageConfig_ComplexVariables
    {
        public string userName { get; set; }
        DevOpsEntities DevOpsContext;
        ManageApplications appManager;
        ManageComponents compManager;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_ComplexVariables()
        {
            DevOpsContext = new DevOpsEntities();
            appManager = new ManageApplications(DevOpsContext);
            compManager = new ManageComponents(DevOpsContext);
            // EfToVmConverter = new ConvertObjects();
        }

        public ManageConfig_ComplexVariables(DevOpsEntities entities)
        {
            DevOpsContext = entities;
            appManager = new ManageApplications(DevOpsContext);
            compManager = new ManageComponents(DevOpsContext);
        }

        public ManageConfig_ComplexVariables(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
            appManager = new ManageApplications(DevOpsContext);
            compManager = new ManageComponents(DevOpsContext);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.MachineAppVars> GetAllVariables()
        {
            List<ViewModel.MachineAppVars> allVars = new List<ViewModel.MachineAppVars>();
            List<ViewModel.ConfigVariable> configVars = new List<ViewModel.ConfigVariable>();
            List<ViewModel.EnvironmentDtoVariable> enVars = new List<ViewModel.EnvironmentDtoVariable>();

            configVars = GetAllAppConfigVariables();
            //enVars = GetAllEnVariables();

            return allVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an update component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="component">    The component. </param>
        ///
        /// <returns>   A ViewModel.Component. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Component AddUpdateComponent(ViewModel.Component component, bool? publish = false)
        {
            EFDataModel.DevOps.Component efComp;
            if (component.id != null && component.id != 0)
                efComp = DevOpsContext.Components.Where(x => x.id == component.id).FirstOrDefault();
            else
                efComp = DevOpsContext.Components.Where(x => x.component_name == component.component_name).FirstOrDefault();
            if (efComp == null)
                DevOpsContext.Components.Add(new EFDataModel.DevOps.Component()
                {
                    component_name = component.component_name,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    last_modify_user = component.last_modify_user ?? this.userName ?? string.Empty,
                    MachineComponentPathMaps = new List<MachineComponentPathMap>(),
                    active = true,
                    relative_path = component.relative_path ?? string.Empty,
                    ConfigFiles = new List<EFDataModel.DevOps.ConfigFile>(),
                    ConfigVariables = new List<EFDataModel.DevOps.ConfigVariable>(),
                    Applications = appManager.GetEfApplication(component.Applications) ?? new List<EFDataModel.DevOps.Application>(),
                });
            else
            {
                if ((efComp.component_name != component.component_name)
                    || efComp.active != component.active
                    || efComp.relative_path != component.relative_path)
                {
                    efComp.component_name = component.component_name;
                    efComp.modify_date = component.modify_date ?? DateTime.Now;
                    efComp.active = component.active;
                    efComp.relative_path = component.relative_path;
                }
                var efApps = appManager.GetEfApplication(component.Applications);
                var oldEfApps = efComp.Applications.Where(x => !efApps.Any(y => y.id == x.id)).ToList();
                if (oldEfApps.Count() > 0)
                {
                    foreach (var oldApp in oldEfApps)
                    {
                        efComp.Applications.Remove(oldApp);
                    }
                    efComp.modify_date = component.modify_date ?? DateTime.Now;
                    efComp.last_modify_user = this.userName;
                }
                var newEfApps = efApps.Where(x => !efComp.Applications.Any(y => y.id == x.id)).ToList();
                if (newEfApps.Count() > 0)
                {
                    foreach (var newApp in newEfApps)
                    {
                        efComp.Applications.Add(newApp);
                    }
                    efComp.modify_date = component.modify_date ?? DateTime.Now;
                    efComp.last_modify_user = this.userName;
                }
            }
            if (publish ?? false)
            {

            }
            DevOpsContext.SaveChanges();
            return compManager.GetComponent(component.component_name, true);
        }

        public ViewModel.Application AddUpdateApplication(ViewModel.ApplicationDto appDto)
        {
            ViewModel.Application newApp = new ViewModel.Application(appDto);
            return AddUpdateApplication(newApp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an update application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="application">  The application. </param>
        ///
        /// <returns>   A ViewModel.Application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Application AddUpdateApplication(ViewModel.Application application)
        {
            EFDataModel.DevOps.Application efApp;
            if (application.id != null && application.id != 0)
                efApp = DevOpsContext.Applications.Where(x => x.id == application.id).FirstOrDefault();
            else
                efApp = DevOpsContext.Applications.Where(x => x.application_name == application.application_name).FirstOrDefault();
            if (efApp == null)
                DevOpsContext.Applications.Add(new EFDataModel.DevOps.Application()
                {
                    application_name = application.application_name,
                    create_date = DateTime.Now,
                    modify_date = application.modify_date ?? DateTime.Now,
                    last_modify_user = application.last_modify_user ?? this.userName ?? string.Empty,
                    active = true,
                    release = application.release ?? string.Empty,
                    Components = compManager.GetEfComponent(application.Components),
                });
            else
            {
                if ((efApp.application_name != application.application_name)
                    || (efApp.active != application.active)
                    || (efApp.release != application.release && !string.IsNullOrWhiteSpace(application.release)))
                {
                    efApp.application_name = application.application_name;
                    efApp.modify_date = DateTime.Now;
                    efApp.last_modify_user = application.last_modify_user ?? this.userName ?? string.Empty;
                    efApp.active = application.active;
                    efApp.release = application.release ?? string.Empty;
                };
                var efComps = compManager.GetEfComponent(application.Components);
                var oldEfComps = efApp.Components.Where(x => !efComps.Any(y => y.id == x.id)).ToList();
                if (oldEfComps.Count() > 0)
                {
                    foreach (var oldComp in oldEfComps)
                    {
                        efApp.Components.Remove(oldComp);
                    }
                    efApp.modify_date = DateTime.Now;
                    efApp.last_modify_user = application.last_modify_user ?? this.userName ?? string.Empty;
                }
                var newEfComps = efComps.Where(x => !efApp.Components.Any(y => y.id == x.id)).ToList();
                if (newEfComps.Count() > 0)
                {
                    foreach (var newComp in newEfComps)
                    {
                        efApp.Components.Add(newComp);
                    }
                    efApp.modify_date = DateTime.Now;
                    efApp.last_modify_user = application.last_modify_user ?? this.userName ?? string.Empty;
                }
            }
            DevOpsContext.SaveChanges();
            return appManager.GetApplication(application.application_name);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets drop down values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>   The drop down values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<NameValuePair> GetDropDownValues(string type)
        {
            List<NameValuePair> values = new List<NameValuePair>();
            //switch (type.ToLower())
            //{
            //    case "comp":
            //        break;
            //    case "env":
            //        break;
            //    case "app":
            //        break;
            //    case "machine":
            //        break;
            //    case "usage":
            //        break;
            //    case "location":
            //        break;
            //    default:
            //        throw new ArgumentException("Invalid value type requested.");
            //}
            if (type.ToLower().StartsWith("comp"))
            {
                var components = DevOpsContext.Components.ToList();
                foreach (var c in components)
                    values.Add(new NameValuePair()
                    {
                        id = c.id,
                        name = c.component_name,
                        value = c.component_name,
                        active = c.active
                    });
            }
            else if (type.ToLower().StartsWith("env"))
            {
                var environments = DevOpsContext.Enum_EnvironmentType.OrderBy(x => x.name).ToList();
                foreach (var e in environments)
                    values.Add(new NameValuePair()
                    {
                        id = null,
                        name = e.name,
                        value = e.name,
                        active = e.active
                    });
            }
            else if (type.ToLower().StartsWith("app"))
            {
                var apps = DevOpsContext.Applications.ToList();
                foreach (var a in apps)
                    values.Add(new NameValuePair()
                    {
                        id = a.id,
                        name = a.application_name,
                        value = a.application_name,
                        active = a.active
                    });
            }
            else if (type.ToLower().StartsWith("machine"))
            {
                var machines = DevOpsContext.Machines.ToList();
                foreach (var m in machines)
                {
                    values.Add(new NameValuePair()
                    {
                        id = m.id,
                        name = m.machine_name,
                        value = m.machine_name,
                        active = m.active,
                    });
                }
            }
            else if (type.ToLower().StartsWith("location"))
            {
                var locations = DevOpsContext.Enum_Locations.ToList();
                foreach (var l in locations)
                {
                    values.Add(new NameValuePair()
                    {
                        id = 0,
                        name = l.name,
                        value = l.value,
                        active = l.active,
                    });
                }
            }
            else
                throw new ArgumentException("Invalid value type requested.");
            return values;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all machine configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/17/2017. </remarks>
        ///
        /// <returns>   all machine configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.AppVar> GetAllConfigVariables()
        {
            List<ViewModel.AppVar> allVars = new List<ViewModel.AppVar>();
            var allConfigVars = DevOpsContext.ConfigVariables.ToList();
            foreach (var appVar in allConfigVars)
            {
                var varComponents = appVar.Components.ToList();

                foreach (var vc in varComponents)
                {
                    ViewModel.ConfigVariable configVar = ReturnConfigVariable(appVar);
                    if (!string.IsNullOrWhiteSpace(configVar.parent_element))
                    {
                        AppVar appVarModel = new AppVar(configVar);
                        appVarModel.componentId = vc.id;
                        appVarModel.componentName = vc.component_name ?? string.Empty;

                        foreach (var app in vc.Applications)
                        {
                            if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                                appVarModel.applicationNames = app.application_name;
                            else
                                appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                        }
                        if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                            appVarModel.applicationNames = string.Empty;
                        appVarModel.values.AddRange(configVar.ConfigVariableValues);
                        if (DevOpsContext.Notes.Where(x => x.note_id == appVarModel.configvar_id && x.note_type == "ConfigVariables").Any())
                            appVarModel.hasNotes = true;
                        allVars.Add(appVarModel);
                    }
                }
            }
            return allVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariable> GetAllAppConfigVariables()
        {
            var configVars = new List<ViewModel.ConfigVariable>();
            var allConfigVars = (from vars in DevOpsContext.ConfigVariables
                                 select vars).ToList();

            foreach (var config in allConfigVars)
            {
                configVars.Add(ReturnConfigVariable(config));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariableValue> GetConfigValues()
        {
            List<ViewModel.ConfigVariableValue> configVars = new List<ViewModel.ConfigVariableValue>();
            List<EFDataModel.DevOps.ConfigVariableValue> efConfigVals = DevOpsContext.ConfigVariableValues.ToList();
            foreach (EFDataModel.DevOps.ConfigVariableValue efVal in efConfigVals)
            {
                configVars.Add(ReturnConfigValue(efVal));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="configvar_id"> Identifier for the configvar. </param>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariableValue> GetConfigValues(int configvar_id)
        {
            List<ViewModel.ConfigVariableValue> configVars = new List<ViewModel.ConfigVariableValue>();
            List<EFDataModel.DevOps.ConfigVariableValue> efConfigVals = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == configvar_id).ToList();
            foreach (EFDataModel.DevOps.ConfigVariableValue efVal in efConfigVals)
            {
                configVars.Add(ReturnConfigValue(efVal));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="varId">    Identifier for the variable. </param>
        /// <param name="envType">  Type of the environment. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar GetVariable(int varId, string envType = null)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the value described by value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/28/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ViewModel.ConfigVariableValue. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.ConfigVariableValue UpdateValue(ViewModel.ConfigVariableValue value)
        {
            EFDataModel.DevOps.ConfigVariableValue efValue = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == value.configvar_id && x.environment_type == value.environment).FirstOrDefault();
            if (efValue == null)
            {
                EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(y => y.id == value.configvar_id).FirstOrDefault();
                if (efConfigVar == null)
                    throw new KeyNotFoundException("Config Variable not found");
                efValue = new EFDataModel.DevOps.ConfigVariableValue()
                {
                    configvar_id = value.configvar_id,
                    environment_type = value.environment,
                    value = value.value,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    last_modify_user = this.userName,
                };
                efConfigVar.ConfigVariableValues.Add(efValue);
            }
            else
            {
                if (efValue.value != value.value)
                {
                    efValue.value = value.value;
                    efValue.modify_date = DateTime.Now;
                    efValue.last_modify_user = this.userName;
                }
            }
            DevOpsContext.SaveChanges();
            return GetConfigValues(value.configvar_id, value.environment);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the variable described by appValue. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar UpdateVariable(AppVar appValue)
        {
            return AddUpdateVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar AddVariable(AppVar appValue)
        {
            return AddUpdateVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the variable described by ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar DeleteVariable(int id)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        //  <section> Begin Private Methods </section>
        ///-------------------------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/28/2017. </remarks>
        ///
        /// <exception cref="KeyNotFoundException"> Thrown when a Key Not Found error condition occurs. </exception>
        ///
        /// <param name="configvar_id"> Identifier for the configvar. </param>
        /// <param name="environment">  The environment. </param>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariableValue GetConfigValues(int configvar_id, string environment)
        {
            EFDataModel.DevOps.ConfigVariableValue efValue = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == configvar_id && x.environment_type == environment).FirstOrDefault();
            if (efValue == null)
            {
                EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(y => y.id == configvar_id).FirstOrDefault();
                if (efConfigVar == null)
                    throw new KeyNotFoundException("Configuration Variable not found, please add / save new variable");
                throw new KeyNotFoundException(string.Format("Config Variable {0} does not contain a value for {1} environment."));
            }
            return new ViewModel.ConfigVariableValue()
            {
                id = efValue.id,
                configvar_id = efValue.configvar_id,
                environment = efValue.environment_type,
                create_date = efValue.create_date,
                modify_date = efValue.modify_date,
                last_modify_user = efValue.last_modify_user,
                publish_date = efValue.published_date,
                published = efValue.published
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/17/2017. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        ///
        /// <returns>   The configuration variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariable ReturnConfigVariable(EFDataModel.DevOps.ConfigVariable config)
        {
            var environments = DevOpsContext.Enum_EnvironmentType.OrderBy(x => x.name).ToList();
            var file = config.ConfigFile;
            if (file == null)
                file = new EFDataModel.DevOps.ConfigFile();
            List<ViewModel.ConfigVariableValue> configVars = new List<ViewModel.ConfigVariableValue>();
            if (config.ConfigVariableValues != null && config.ConfigVariableValues.Count() > 0)
                configVars = EfToVmConverter.EfConfigValueListToVm(config.ConfigVariableValues, environments);
            else
                configVars = EfToVmConverter.EfConfigValueListToVm(new List<EFDataModel.DevOps.ConfigVariableValue>(), environments, config.configfile_id);
            var configVariable = new ViewModel.ConfigVariable()
            {
                id = config.id,
                active = config.active,
                attribute = config.attribute,
                create_date = config.create_date,
                element = config.element,
                key = config.key,
                modify_date = config.modify_date,
                last_modify_user = config.last_modify_user,
                value_name = config.value_name ?? "",
                parent_element = config.parent_element,
                full_element = config.full_element,
                ConfigFile = EfToVmConverter.EfConfigFileToVm(file),
                ConfigVariableValues = configVars,
                //ConfigVariableValues = EfToVmConverter.EfConfigValueListToVm(configVars, environments),
                Components = EfToVmConverter.EfComponentListToVm(config.Components)
            };
            return configVariable;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="configVal">    The configuration value. </param>
        ///
        /// <returns>   The configuration value. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariableValue ReturnConfigValue(EFDataModel.DevOps.ConfigVariableValue configVal)
        {
            return new ViewModel.ConfigVariableValue()
            {
                configvar_id = configVal.configvar_id,
                id = configVal.id,
                environment = configVal.environment_type,
                value = configVal.value,
                create_date = configVal.create_date,
                modify_date = configVal.modify_date,
                last_modify_user = configVal.last_modify_user,
                publish_date = configVal.published_date,
                published = configVal.published
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<AppVar> GetVariable(int value)
        {
            List<ViewModel.AppVar> appVars = new List<ViewModel.AppVar>();
            EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.id == value).FirstOrDefault();
            List<EFDataModel.DevOps.Component> varComponents = efConfigVar.Components.ToList();
            foreach (EFDataModel.DevOps.Component vc in varComponents)
            {
                ViewModel.ConfigVariable configVar = ReturnConfigVariable(efConfigVar);
                AppVar appVarModel = new AppVar(configVar);
                appVarModel.componentId = vc.id;
                appVarModel.componentName = vc.component_name ?? string.Empty;

                foreach (var app in vc.Applications)
                {
                    if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                        appVarModel.applicationNames = app.application_name;
                    else
                        appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                }
                appVarModel.values.AddRange(configVar.ConfigVariableValues.OrderBy(x => x.environment));
                appVars.Add(appVarModel);
            }
            return appVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the variable described by appValue. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        private AppVar AddUpdateVariable(AppVar appValue)
        {
            EFDataModel.DevOps.ConfigVariable efConfig = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.configvar_id).FirstOrDefault();
            if (efConfig == null)
            {
                efConfig = DevOpsContext.ConfigVariables.Where(x => x.attribute == appValue.attribute
                                                                        && x.ConfigFile.file_name == appValue.fileName
                                                                        && x.element == appValue.configElement
                                                                        && x.key == appValue.key
                                                                        && x.value_name == appValue.valueName
                                                                        && x.parent_element == appValue.configParentElement).FirstOrDefault();
            }

            List<EFDataModel.DevOps.ConfigVariableValue> efConfigValueList = new List<EFDataModel.DevOps.ConfigVariableValue>();
            EFDataModel.DevOps.Component efComponent;
            EFDataModel.DevOps.ConfigFile efConfigFile = new EFDataModel.DevOps.ConfigFile();

            if (appValue.componentId != null)
            {
                efComponent = DevOpsContext.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            }
            else if (!string.IsNullOrWhiteSpace(appValue.componentName))
            {
                efComponent = DevOpsContext.Components.Where(y => y.component_name == appValue.componentName).FirstOrDefault();
            }
            else
                throw new ArgumentNullException("No Component provided to add variable.");
            if (!string.IsNullOrWhiteSpace(appValue.fileName))
                efConfigFile = DevOpsContext.ConfigFiles.Where(x => x.file_name == appValue.fileName).FirstOrDefault();
            if (efConfig == null)
            {
                if (string.IsNullOrWhiteSpace(appValue.configElement))
                    appValue.configElement = appValue.key;
                efConfig = new EFDataModel.DevOps.ConfigVariable()
                {
                    active = true,
                    element = appValue.configElement,

                    // ToDo: Create XML element based on AppVar
                    // full_element = appValue.fullElement,
                    // full_element = getFullElement(appValue),

                    key = appValue.key ?? appValue.configElement,
                    attribute = appValue.attribute ?? "",
                    parent_element = appValue.configParentElement ?? "",
                    value_name = appValue.valueName ?? "",
                    full_element = appValue.fullElement ?? "",
                    ConfigFile = efConfigFile ?? new EFDataModel.DevOps.ConfigFile(),
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    last_modify_user = this.userName,
                };
                if (appValue.values.Count > 0)
                {
                    foreach (var val in appValue.values)
                    {
                        efConfig.ConfigVariableValues.Add(new EFDataModel.DevOps.ConfigVariableValue()
                        {
                            configvar_id = val.configvar_id,
                            create_date = val.create_date ?? DateTime.Now,
                            modify_date = val.modify_date ?? DateTime.Now,
                            last_modify_user = this.userName,
                            environment_type = val.environment,
                            published = val.published ?? false,
                            published_date = val.publish_date,
                            value = val.value
                        });
                    }
                }
                else
                {
                    //efConfig.ConfigVariableValues = new List<EFDataModel.DevOps.ConfigVariableValue>();
                    //efConfig.ConfigVariableValues.Add(new EFDataModel.DevOps.ConfigVariableValue()
                    //{
                    //    create_date = DateTime.Now,
                    //    modify_date = DateTime.Now,
                    //    last_modify_user = this.userName,
                    //    environment_type = "development",
                    //    published = false,
                    //    value = "",
                    //});
                }
                efConfig.Components.Add(efComponent);
                DevOpsContext.ConfigVariables.Add(efConfig);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appValue.fileName))
                    //efConfigFile = efConfig.ConfigFile;
                    efConfigFile = null;
                efConfigValueList = efConfig.ConfigVariableValues.ToList();
                if (!(efConfig.element == appValue.configElement &&
                    efConfig.key == appValue.key &&
                    efConfig.attribute == appValue.attribute &&
                    efConfig.parent_element == appValue.configParentElement &&
                    efConfig.value_name == appValue.valueName))
                {
                    efConfig.element = appValue.configElement;
                    efConfig.attribute = appValue.attribute ?? "";
                    efConfig.parent_element = appValue.configParentElement ?? "";
                    efConfig.full_element = appValue.fullElement.Replace(efConfig.key, appValue.key) ?? "";
                    efConfig.value_name = appValue.valueName ?? "";
                    efConfig.key = appValue.key ?? appValue.configElement;

                    // ToDo: Create XML element based on AppVar
                    // efConfig.full_element = getFullElement(appValue),

                    efConfig.ConfigFile = efConfigFile;
                    //efConfig.create_date = efConfig.create_date;
                    efConfig.modify_date = DateTime.Now;
                    efConfig.last_modify_user = this.userName;
                }
                foreach (var val in appValue.values)
                {
                    var efConfigValue = efConfigValueList.Where(x => x.configvar_id == val.configvar_id
                        && x.environment_type == val.environment).FirstOrDefault();
                    if (efConfigValue == null)
                    {
                        if (!string.IsNullOrWhiteSpace(val.value))
                        {
                            efConfigValue = new EFDataModel.DevOps.ConfigVariableValue()
                            {
                                configvar_id = appValue.configvar_id.Value,
                                environment_type = val.environment,
                                value = val.value,
                                create_date = DateTime.Now,
                                modify_date = DateTime.Now,
                                last_modify_user = this.userName,
                                published_date = val.publish_date
                            };
                            DevOpsContext.ConfigVariableValues.Add(efConfigValue);
                        }
                    }
                    else
                    {
                        if (efConfigValue.value != val.value)
                        {
                            efConfigValue.value = val.value;
                            efConfigValue.modify_date = DateTime.Now;
                            efConfigValue.last_modify_user = this.userName;
                        }
                    }
                }
            }
            try
            {
                DevOpsContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error saving to Database", e);
            }
            return GetVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private AppVar GetVariable(AppVar appValue)
        {
            EFDataModel.DevOps.ConfigVariable efConfigVar;
            if (appValue.configvar_id != null)
                efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.configvar_id).FirstOrDefault();
            else
                efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.attribute == appValue.attribute
                                                                        && x.ConfigFile.file_name == appValue.fileName
                                                                        && x.element == appValue.configElement
                                                                        && x.key == appValue.key
                                                                        && x.value_name == appValue.valueName
                                                                        && x.parent_element == appValue.configParentElement)
                                                           .FirstOrDefault();
            var varComponent = efConfigVar.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            ViewModel.ConfigVariable configVar = ReturnConfigVariable(efConfigVar);
            AppVar appVar = new AppVar(configVar);
            appVar.componentId = varComponent.id;
            appVar.componentName = varComponent.component_name ?? string.Empty;

            foreach (var app in varComponent.Applications)
            {
                if (string.IsNullOrWhiteSpace(appVar.applicationNames))
                    appVar.applicationNames = app.application_name;
                else
                    appVar.applicationNames = string.Concat(appVar.applicationNames, ", ", app.application_name);
            }
            appVar.values.AddRange(configVar.ConfigVariableValues);
            return appVar;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns application variable. </summary>
        ///
        /// <remarks>   Patman, 5/10/2017. </remarks>
        ///
        /// <param name="a">    The Application to process. </param>
        ///
        /// <returns>   The application variable. </returns>
        ///-------------------------------------------------------------------------------------------------

        private ViewModel.Application ReturnApplicationVariable(EFDataModel.DevOps.Application a)
        {
            return new ViewModel.Application()
            {
                active = a.active,
                application_name = a.application_name,
                create_date = a.create_date,
                id = a.id,
                modify_date = a.modify_date,
                last_modify_user = a.last_modify_user,
                release = a.release,
                Components = EfToVmConverter.EfComponentListToVm(a.Components).ToList(),
            };
        }
    }
}
