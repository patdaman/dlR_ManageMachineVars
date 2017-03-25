using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModel.DevOps;
using ViewModel;

namespace BusinessLayer
{
    public class ManageComplexConfigVariables
    {
        DevOpsEntities DevOpsContext;
        ConvertObjects EfToVmConverter = new ConvertObjects();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageComplexConfigVariables()
        {
            DevOpsContext = new DevOpsEntities();
            // EfToVmConverter = new ConvertObjects();
        }

        public ManageComplexConfigVariables(DevOpsEntities entities)
        {
            DevOpsContext = entities;
        }

        public ManageComplexConfigVariables(string conn)
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
            enVars = GetAllEnVariables();

            return allVars;
        }

        public ViewModel.ConfigVariableValue UpdateValue(ViewModel.ConfigVariableValue value)
        {
            throw new NotImplementedException();
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
        /// <summary>   Returns configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/17/2017. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        ///
        /// <returns>   The configuration variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.ConfigVariable ReturnConfigVariable(EFDataModel.DevOps.ConfigVariable config)
        {
            return new ViewModel.ConfigVariable()
            {
                id = config.id,
                active = config.active,
                key_name = config.key_name,
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
        public ViewModel.ConfigVariableValue ReturnConfigValue(EFDataModel.DevOps.ConfigVariableValue configVal)
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
        /// <summary>   Gets all en variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all en variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.EnvironmentDtoVariable> GetAllEnVariables()
        {
            var enVars = new List<ViewModel.EnvironmentDtoVariable>();
            var allEnVars = (from vars in DevOpsContext.EnvironmentVariables
                             select vars).ToList();

            foreach (var env in allEnVars)
            {
                enVars.Add(new ViewModel.EnvironmentDtoVariable()
                {
                    id = env.id,
                    active = env.active,
                    create_date = env.create_date,
                    key = env.key,
                    modify_date = env.modify_date,
                    path = env.path,
                    type = env.type,
                    value = env.value,
                    Applications = EfToVmConverter.EfAppListToVm(env.Applications),
                    Machines = EfToVmConverter.EfMachineListToVm(env.Machines)
                });
            }
            return enVars;
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

        public AppVar GetVariable(int varId, string envType)
        {
            throw new NotImplementedException();
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
        public AppVar UpdateVariable(AppVar appValue)
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
                    //key_name = appValue.keyName,
                    parent_element = appValue.configParentElement,
                    //value_name = appValue.valueName,
                    modify_date = DateTime.Now
                };
                DevOpsContext.ConfigVariables.Add(efConfig);
            }
            else
            {
                //efConfig.active = appValue.varActive;
                efConfig.element = appValue.configElement;
                efConfig.key = appValue.key;
                //efConfig.key_name = appValue.keyName;
                efConfig.parent_element = appValue.configParentElement;
                //efConfig.value_name = appValue.valueName;
                efConfig.modify_date = DateTime.Now;
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
                    efConfigValue.value = val.value;
                    efConfigValue.modify_date = DateTime.Now;
                    efConfigValue.published_date = val.publish_date;
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

        public AppVar AddVariable(AppVar value)
        {
            throw new NotImplementedException();
        }

        public AppVar DeleteVariable(int id)
        {
            throw new NotImplementedException();
        }

        public List<AppVar> GetMachineVariables(int machineId)
        {
            throw new NotImplementedException();
        }
    }
}
