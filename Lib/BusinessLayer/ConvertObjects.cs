using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModel.DevOps;
using ViewModel;
using System.Collections;

namespace BusinessLayer
{
    public class ConvertObjects
    {
        public List<ViewModel.Application> EfAppListToVm(ICollection<EFDataModel.DevOps.Application> apps)
        {
            var applications = new List<ViewModel.Application>();
            foreach (var app in apps)
            {
                applications.Add(AppEfToVm(app));
            }
            return applications;
        }

        private ViewModel.Application AppEfToVm(EFDataModel.DevOps.Application app)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.Application>(app);
        }

        public List<ViewModel.Machine> EfMachineListToVm(ICollection<EFDataModel.DevOps.Machine> EFmachines)
        {
            var machines = new List<ViewModel.Machine>();
            foreach (var machine in EFmachines)
            {
                machines.Add(MachineEfToVm(machine));
            }
            return machines;
        }

        private ViewModel.Machine MachineEfToVm(EFDataModel.DevOps.Machine machine)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.Machine>(machine);
        }

        public List<ViewModel.ConfigVariable> EfConfigListToVm(ICollection<EFDataModel.DevOps.ConfigVariable> EF)
        {
            var vm = new List<ViewModel.ConfigVariable>();
            foreach (var x in EF)
            {
                vm.Add(ConfigEfToVm(x));
            }
            return vm;
        }

        private ViewModel.ConfigVariable ConfigEfToVm(EFDataModel.DevOps.ConfigVariable EF)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.ConfigVariable>(EF);
        }

        public List<ViewModel.EnvironmentDtoVariable> EfEnVarListToVm(ICollection<EFDataModel.DevOps.EnvironmentVariable> EF)
        {
            var vm = new List<ViewModel.EnvironmentDtoVariable>();
            foreach (var x in EF)
            {
                vm.Add(EnVarEfToVm(x));
            }
            return vm;
        }

        private ViewModel.EnvironmentDtoVariable EnVarEfToVm(EFDataModel.DevOps.EnvironmentVariable EF)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.EnvironmentDtoVariable>(EF);
        }
    }
}
