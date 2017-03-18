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
            List<ViewModel.MachineAppVars_Complete> allVars_Complete = new List<MachineAppVars_Complete>();
            List<ViewModel.MachineAppVars> allVars = new List<ViewModel.MachineAppVars>();
            List<ViewModel.ConfigVariable> configVars = new List<ViewModel.ConfigVariable>();
            List<ViewModel.EnvironmentDtoVariable> enVars = new List<ViewModel.EnvironmentDtoVariable>();

            configVars = GetAllConfigVariables();
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
        public List<ViewModel.MachineAppVars> GetAllMachineConfigVariables()
        {
            List<ViewModel.MachineAppVars> allVars = new List<ViewModel.MachineAppVars>();
            var allConfigVars = DevOpsContext.ConfigVariables.ToList();
            foreach (var appVar in allConfigVars)
            {
                var varComponents = appVar.Components.ToList();
                var varValues = appVar.ConfigVariableValues.ToList();

                foreach (var vc in varComponents)
                {
                    foreach (var path in vc.MachineComponentPaths)
                    {
                        var machine = DevOpsContext.Machines.Where(x => x.id == path.machine_id).FirstOrDefault();
                        foreach (var app in vc.Applications)
                        {
                            var configVar = ReturnConfigVariable(appVar);
                            foreach (var val in configVar.ConfigVariableValues)
                            {
                                MachineAppVars appVarModel = new MachineAppVars(ReturnConfigVariable(appVar));

                                appVarModel.varModify_date = val.modify_date;
                                appVarModel.varCreate_date = val.create_date;
                                appVarModel.value = val.value;

                                appVarModel.machineId = machine.id;
                                appVarModel.machine_name = machine.machine_name ?? string.Empty;
                                appVarModel.location = machine.location ?? string.Empty;
                                appVarModel.usage = machine.usage ?? string.Empty;

                                appVarModel.componentId = vc.id;
                                appVarModel.componentName = vc.component_name ?? string.Empty;

                                appVarModel.applicationId = app.id;
                                appVarModel.applicationName = app.application_name ?? string.Empty;
                                appVarModel.applicationRelease = app.release ?? string.Empty;

                                appVarModel.varPath = path.config_path ?? string.Empty;
                                allVars.Add(appVarModel);
                            }
                        }
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
        public List<ViewModel.ConfigVariable> GetAllConfigVariables()
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
                value_name = config.value_name,
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

        public MachineAppVars GetVariable(int machineId, int varId, string varType)
        {
            throw new NotImplementedException();
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
