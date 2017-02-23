using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer
{
    public class ManageConfig
    {
        DevOpsEntities DevOpsContext = new DevOpsEntities();
        ConvertObjects EfToVmConverter = new ConvertObjects();

        public List<ViewModel.MachineAppVars> GetAllVariables()
        {
            List<ViewModel.MachineAppVars_Complete> allVars_Complete = new List<MachineAppVars_Complete>();
            List<ViewModel.MachineAppVars> allVars = new List<ViewModel.MachineAppVars>();
            List<ViewModel.ConfigVariable> configVars = new List<ViewModel.ConfigVariable>();
            List<ViewModel.EnvironmentVariable> enVars = new List<ViewModel.EnvironmentVariable>();

            configVars = GetAllConfigVariables();
            enVars = GetAllEnVariables();

            return allVars;
        }

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
                    attribute = config.attribute,
                    config_path = config.config_path,
                    create_date = config.create_date,
                    element = config.element,
                    key = config.key,
                    modify_date = config.modify_date,
                    value = config.value,
                    value_name = config.value_name,
                    Applications = EfToVmConverter.EfAppListToVm(config.Applications),
                    Machines = EfToVmConverter.EfMachineListToVm(config.Machines)
                });
            }
            return configVars;
        }

        public List<ViewModel.EnvironmentVariable> GetAllEnVariables()
        {
            var enVars = new List<ViewModel.EnvironmentVariable>();
            var allEnVars = (from vars in DevOpsContext.EnvironmentVariables
                                 select vars).ToList();

            foreach (var env in allEnVars)
            {
                enVars.Add(new ViewModel.EnvironmentVariable()
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
