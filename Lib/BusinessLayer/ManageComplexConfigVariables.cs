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
                configVars.Add(new ViewModel.ConfigVariable()
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
                });
            }
            return configVars;
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
