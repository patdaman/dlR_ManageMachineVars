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
                var varValues = appVar.ConfigVariableValues.ToList();

                foreach (var vc in varComponents)
                {
                    ViewModel.ConfigVariable configVar = ReturnConfigVariable(appVar);
                    AppVar appVarModel = new AppVar(ReturnConfigVariable(appVar));
                    appVarModel.componentId = vc.id;
                    appVarModel.componentName = vc.component_name ?? string.Empty;

                    foreach (var app in vc.Applications)
                    {
                        if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                            appVarModel.applicationNames = app.application_name;
                        else
                            appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                    }
                    foreach (ViewModel.ConfigVariableValue val in configVar.ConfigVariableValues)
                    {
                        ConfigVarValues appVarValueModel = new ConfigVarValues()
                        {
                            id = val.id,
                            configvar_id = val.configvar_id,
                            environment = val.environment_type,
                            modify_date = val.modify_date,
                            create_date = val.create_date,
                            publish_date = val.publish_date,
                            value = val.value,
                        };
                        appVarModel.values.Add(appVarValueModel);
                    }
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

        private AppVar GetVariable(int value)
        {
            throw new NotImplementedException();
        }

        public AppVar GetVariable(int varId, string envType)
        {
            throw new NotImplementedException();
        }

        public AppVar UpdateVariable(AppVar appValue)
        {
            EFDataModel.DevOps.ConfigVariable efConfig = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.varId).FirstOrDefault();
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
                        configvar_id = appValue.varId.Value,
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

            return appValue;
            return GetVariable(appValue.varId.Value);
        }

        public MachineAppVars AddVariable(MachineAppVars value)
        {
            throw new NotImplementedException();
        }

        public MachineAppVars DeleteVariable(int id)
        {
            throw new NotImplementedException();
        }

        public List<MachineAppVars> GetMachineVariables(int machineId)
        {
            throw new NotImplementedException();
        }

        public MachineAppVars GetGlobalVariable(int varId, string varType)
        {
            throw new NotImplementedException();
        }
    }
}
