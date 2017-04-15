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
        DevOpsEntities DevOpsContext;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_ComplexVariables()
        {
            DevOpsContext = new DevOpsEntities();
            // EfToVmConverter = new ConvertObjects();
        }

        public ManageConfig_ComplexVariables(DevOpsEntities entities)
        {
            DevOpsContext = entities;
        }

        public ManageConfig_ComplexVariables(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
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
        /// <summary>   Gets the component. </summary>
        ///
        /// <remarks>   Patman, 4/14/2017. </remarks>
        ///
        /// <returns>   The component. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<ViewModel.Component> GetComponent()
        {
            List<ViewModel.Component> components = new List<ViewModel.Component>();
            var efComponents = DevOpsContext.Components.ToList();
            foreach (var c in efComponents)
            {
                components.Add(ReturnComponentVariable(c));
            }
            return components;
        }

        public ViewModel.Component GetComponent(int componentId)
        {
            var efComponent = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();
            return ReturnComponentVariable(efComponent);
        }

        public ViewModel.Component GetComponent(string componentName)
        {
            var efComponent = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault();
            return ReturnComponentVariable(efComponent);
        }

        public ViewModel.Component AddUpdateComponent(ViewModel.Component component)
        {
            throw new NotImplementedException();
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
                var environments = DevOpsContext.Enum_EnvironmentType.ToList();
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
                    AppVar appVarModel = new AppVar(configVar);
                    appVarModel.componentId = vc.id;
                    appVarModel.componentName = vc.component_name ?? string.Empty;

                    foreach (var app in vc.Applications)
                    {
                        if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                            appVarModel.applicationNames = app.application_name;
                        else
                            //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                            appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                    }
                    appVarModel.values.AddRange(configVar.ConfigVariableValues);
                    allVars.Add(appVarModel);
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
                };
                efConfigVar.ConfigVariableValues.Add(efValue);
            }
            else
            {
                if (efValue.value != value.value)
                {
                    efValue.value = value.value;
                    efValue.modify_date = DateTime.Now;
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
        /// <summary>   Deletes the component described by componentId. </summary>
        ///
        /// <remarks>   Patman, 4/14/2017. </remarks>
        ///
        /// <param name="componentId">  Identifier for the component. </param>
        ///
        /// <returns>   A ViewModel.Component. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Component DeleteComponent(int componentId)
        {
            var comp = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();
            DevOpsContext.Components.Remove(comp);
            DevOpsContext.SaveChanges();
            return new ViewModel.Component();
        }

        ///-------------------------------------------------------------------------------------------------
        //  <section> Begin Private Methods </section>
        ///-------------------------------------------------------------------------------------------------


        private ViewModel.Component ReturnComponentVariable(EFDataModel.DevOps.Component component)
        {
            return new ViewModel.Component()
            {
                id = component.id,
                active = component.active,
                component_name = component.component_name,
                create_date = component.create_date,
                relative_path = component.relative_path,
                modify_date = component.modify_date,
                MachineComponentPaths = EfToVmConverter.EfMachineComponentPathListToVm(component.MachineComponentPathMaps),
                Applications = EfToVmConverter.EfAppListToVm(component.Applications),
                ConfigVariables = EfToVmConverter.EfConfigListToVm(component.ConfigVariables).ToList(),
            };
        }

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
            return new ViewModel.ConfigVariable()
            {
                id = config.id,
                active = config.active,
                attribute = config.attribute,
                create_date = config.create_date,
                element = config.element,
                key = config.key,
                modify_date = config.modify_date,
                value_name = config.value_name ?? "",
                parent_element = config.parent_element,
                ConfigVariableValues = EfToVmConverter.EfConfigValueListToVm(config.ConfigVariableValues),
                Components = EfToVmConverter.EfComponentListToVm(config.Components)
            };
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
                        //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                        appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                }
                appVarModel.values.AddRange(configVar.ConfigVariableValues);
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
            List<EFDataModel.DevOps.ConfigVariableValue> efConfigValueList = efConfig.ConfigVariableValues.ToList();
            EFDataModel.DevOps.Component efComponent = DevOpsContext.Components.Where(y => y.component_name == appValue.componentName).FirstOrDefault();

            if (efConfig == null)
            {
                efConfig = new EFDataModel.DevOps.ConfigVariable()
                {
                    active = true,
                    element = appValue.configElement,
                    key = appValue.key,
                    attribute = appValue.attribute ?? "key",
                    parent_element = appValue.configParentElement,
                    value_name = appValue.valueName ?? "value",
                    modify_date = DateTime.Now
                };
                DevOpsContext.ConfigVariables.Add(efConfig);
            }
            else
            {
                if (!(efConfig.element == appValue.configElement &&
                    efConfig.key == appValue.key &&
                    efConfig.attribute == appValue.attribute &&
                    efConfig.parent_element == appValue.configParentElement &&
                    efConfig.value_name == appValue.valueName))
                {

                    efConfig.element = appValue.configElement;
                    efConfig.key = appValue.key;
                    efConfig.attribute = appValue.attribute;
                    efConfig.parent_element = appValue.configParentElement;
                    efConfig.value_name = appValue.valueName;
                    efConfig.modify_date = DateTime.Now;
                }
            }
            foreach (var val in appValue.values)
            {
                var efConfigValue = efConfigValueList.Where(x => x.configvar_id == val.configvar_id
                    && x.environment_type == val.environment).FirstOrDefault();
                if (efConfigValue == null)
                {
                    efConfigValue = new EFDataModel.DevOps.ConfigVariableValue()
                    {
                        configvar_id = appValue.configvar_id.Value,
                        environment_type = val.environment,
                        value = val.value,
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                        published_date = val.publish_date
                    };
                    DevOpsContext.ConfigVariableValues.Add(efConfigValue);
                }
                else
                {
                    if (efConfigValue.value != val.value)
                    {
                        efConfigValue.value = val.value;
                        efConfigValue.modify_date = DateTime.Now;
                        efConfigValue.published_date = val.publish_date;
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
            EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.configvar_id).FirstOrDefault();
            EFDataModel.DevOps.Component varComponent = efConfigVar.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            ViewModel.ConfigVariable configVar = ReturnConfigVariable(efConfigVar);
            AppVar appVar = new AppVar(configVar);
            appVar.componentId = varComponent.id;
            appVar.componentName = varComponent.component_name ?? string.Empty;

            foreach (var app in varComponent.Applications)
            {
                if (string.IsNullOrWhiteSpace(appVar.applicationNames))
                    appVar.applicationNames = app.application_name;
                else
                    //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                    appVar.applicationNames = string.Concat(appVar.applicationNames, ", ", app.application_name);
            }
            appVar.values.AddRange(configVar.ConfigVariableValues);
            return appVar;
        }
    }
}
