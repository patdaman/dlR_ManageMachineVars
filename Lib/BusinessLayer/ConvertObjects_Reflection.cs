﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModel.DevOps;
using ViewModel;
using System.Collections;

namespace BusinessLayer
{
    public class ConvertObjects_Reflection
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef application list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="apps"> The apps. </param>
        ///
        /// <returns>   A List&lt;ViewModel.Application&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Application> EfAppListToVm(ICollection<EFDataModel.DevOps.Application> apps)
        {
            var applications = new List<ViewModel.Application>();
            foreach (var app in apps)
            {
                //applications.Add(AppEfToVm(app));
                applications.Add(new ViewModel.Application()
                {
                    id = app.id,
                    application_name = app.application_name,
                    release = app.release,
                    create_date = app.create_date,
                    modify_date = app.modify_date,
                    active = app.active,
                });
            }
            return applications;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Application ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="app">  The application. </param>
        ///
        /// <returns>   A ViewModel.Application. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.Application AppEfToVm(EFDataModel.DevOps.Application app)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.Application>(app);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef machine list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EFmachines">   The fmachines. </param>
        ///
        /// <returns>   A List&lt;ViewModel.Machine&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Machine> EfMachineListToVm(ICollection<EFDataModel.DevOps.Machine> EFmachines)
        {
            var machines = new List<ViewModel.Machine>();
            foreach (var machine in EFmachines)
            {
                //machines.Add(MachineEfToVm(machine));
                //machines.Add(new ViewModel.Machine(MachineEfToVm(machine)));
                machines.Add(new ViewModel.Machine()
                {
                    id = machine.id,
                    machine_name = machine.machine_name,
                    ip_address = machine.ip_address,
                    location = machine.location,
                    usage = machine.usage,
                    create_date = machine.create_date,
                    modify_date = machine.modify_date,
                    active = machine.active,
                });
            }
            return machines;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Machine ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="machine">  The machine. </param>
        ///
        /// <returns>   A ViewModel.Machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.Machine MachineEfToVm(EFDataModel.DevOps.Machine machine)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.Machine>(machine);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef configuration list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A list of. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ICollection<ViewModel.ConfigVariable> EfConfigListToVm(ICollection<EFDataModel.DevOps.ConfigVariable> EF)
        {
            var vm = new List<ViewModel.ConfigVariable>();
            foreach (var x in EF)
            {
                vm.Add(ConfigEfToVm(x));
            }
            return vm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef configuration value list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A list of. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ICollection<ViewModel.ConfigVariableValue> EfConfigValueListToVm(ICollection<EFDataModel.DevOps.ConfigVariableValue> EF, List<EFDataModel.DevOps.Enum_EnvironmentType> environments = null)
        {
            var vm = new List<ViewModel.ConfigVariableValue>();
            foreach (var x in EF)
            {
                //vm.Add(ConfigValueEfToVm(x));
                //vm.Add(new ViewModel.ConfigVariableValue(ConfigValueEfToVm(x)));
                vm.Add(new ViewModel.ConfigVariableValue()
                {
                    id = x.id,
                    configvar_id = x.configvar_id,
                    environment = x.environment_type,
                    value = x.value,
                    create_date = x.create_date,
                    modify_date = x.modify_date,
                    publish_date = x.published_date,
                    published = x.published
                });
            }
            if (environments != null && EF.Count > 0)
            {
                var defaultVal = EF.FirstOrDefault();
                foreach (var e in environments)
                {
                    var check = vm.Where(z => z.environment == e.name.ToString()).FirstOrDefault();
                    if (check == null)
                        vm.Add(new ViewModel.ConfigVariableValue()
                        {
                            id = null,
                            configvar_id = defaultVal.configvar_id,
                            environment = e.name.ToString(),
                            value = string.Empty,
                            create_date = null,
                            modify_date = null,
                            publish_date = null,
                            published = false
                        });
                }
            }
            return vm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef component list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A list of. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ICollection<ViewModel.Component> EfComponentListToVm(ICollection<EFDataModel.DevOps.Component> EF)
        {
            var vm = new List<ViewModel.Component>();
            foreach (var x in EF)
            {
                //vm.Add(ComponentEfToVm(x));
                vm.Add(new ViewModel.Component()
                {
                    id = x.id,
                    component_name = x.component_name,
                    relative_path = x.relative_path,
                    create_date = x.create_date,
                    modify_date = x.modify_date,
                    active = x.active,
                });
            }
            return vm;
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef machine component path list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/17/2017. </remarks>
        ///
        /// <param name="EFmachineComponentPathMaps">   The fmachine component path maps. </param>
        ///
        /// <returns>   A List&lt;MachineComponentPath&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        internal List<MachineComponentPath> EfMachineComponentPathListToVm(ICollection<MachineComponentPathMap> EFmachineComponentPathMaps)
        {
            var MachineComponentPaths = new List<ViewModel.MachineComponentPath>();

            foreach (var machine in EFmachineComponentPathMaps)
            {
                //machines.Add(MachineEfToVm(machine));
                //machines.Add(new ViewModel.Machine(MachineEfToVm(machine)));
                MachineComponentPaths.Add(new ViewModel.MachineComponentPath()
                {
                    machine_id = machine.machine_id,
                    component_id = machine.component_id,
                    config_path = machine.config_path,
                });
            }
            return MachineComponentPaths;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configuration ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A ViewModel.ConfigVariable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariable ConfigEfToVm(EFDataModel.DevOps.ConfigVariable EF)
        {
            ViewModel.ConfigVariable vmConfigVar = new ViewModel.ConfigVariable();
            return new ViewModel.ConfigVariable()
            {
                active = EF.active,
                attribute = EF.attribute,
                create_date = EF.create_date,
                element = EF.element,
                full_element = EF.full_element,
                id = EF.id,
                key = EF.key,
                modify_date = EF.modify_date,
                parent_element = EF.parent_element,
                value_name = EF.value_name,
                ConfigVariableValues = EfConfigValueListToVm(EF.ConfigVariableValues),
            };
            //return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.ConfigVariable>(EF);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configuration value ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A ViewModel.ConfigVariableValue. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariableValue ConfigValueEfToVm(EFDataModel.DevOps.ConfigVariableValue EF)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.ConfigVariableValue>(EF);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Component ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A ViewModel.Component. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.Component ComponentEfToVm(EFDataModel.DevOps.Component EF)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.Component>(EF);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ef en variable list to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A List&lt;ViewModel.EnvironmentDtoVariable&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.EnvironmentDtoVariable> EfEnVarListToVm(ICollection<EFDataModel.DevOps.EnvironmentVariable> EF)
        {
            var vm = new List<ViewModel.EnvironmentDtoVariable>();
            foreach (var x in EF)
            {
                vm.Add(EnVarEfToVm(x));
            }
            return vm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   En variable ef to view model. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/15/2017. </remarks>
        ///
        /// <param name="EF">   The ef. </param>
        ///
        /// <returns>   A ViewModel.EnvironmentDtoVariable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.EnvironmentDtoVariable EnVarEfToVm(EFDataModel.DevOps.EnvironmentVariable EF)
        {
            return CommonUtils.Reflection.ReflectionUtils.CreateNewObjectAndCopyProperties<ViewModel.EnvironmentDtoVariable>(EF);
        }

        public ViewModel.ConfigFile EfConfigFileToVm(EFDataModel.DevOps.ConfigFile configFile)
        {

            return new ViewModel.ConfigFile()
            {
                //
                // The following is ghetto, refactor someday!!
                // 
                file_name = configFile.file_name,
                create_date = configFile.create_date,
                modify_date = configFile.modify_date,
                component_id = configFile.component_id,
                id = configFile.id,
                root_element = configFile.root_element,
                xml_declaration = configFile.xml_declaration,
            };
        }
    }
}
