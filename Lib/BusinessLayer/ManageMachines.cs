using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;

namespace BusinessLayer
{
    public class ManageMachines
    {
        public string userName { get; set; }
        DevOpsEntities DevOpsContext;
        ManageApplications appManager;

        public ManageMachines()
        {
            DevOpsContext = new DevOpsEntities();
            ManageApplications appManager = new ManageApplications(DevOpsContext);
        }
        public ManageMachines(DevOpsEntities devOpsContext)
        {
            DevOpsContext = devOpsContext;
            ManageApplications appManager = new ManageApplications(DevOpsContext);
        }
        public ManageMachines(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
            ManageApplications appManager = new ManageApplications(DevOpsContext);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all machines. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/13/2017. </remarks>
        ///
        /// <returns>   all machines. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Machine> GetAllMachines()
        {
            List<ViewModel.Machine> machineModels = new List<ViewModel.Machine>();
            var allMachines = (from machines in DevOpsContext.Machines
                               select machines).ToList();
            foreach (var machine in allMachines)
            {
                machineModels.Add(ReturnVmMachine(machine));
            }
            return machineModels;
        }

        public List<ViewModel.Event> GetAllMachineInfo(string machineName = null)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/19/2017. </remarks>
        ///
        /// <param name="machine_name"> Name of the machine. </param>
        ///
        /// <returns>   The machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Machine GetMachine(string machine_name)
        {
            var EFMachine = (from machine in DevOpsContext.Machines
                             where machine.machine_name == machine_name
                             select machine).FirstOrDefault();
            return ReturnVmMachine(EFMachine);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/13/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   The machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Machine GetMachine(int id)
        {
            var EFMachine = (from machine in DevOpsContext.Machines
                             where machine.id == id
                             select machine).FirstOrDefault();
            return ReturnVmMachine(EFMachine);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the machine described by value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/13/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ViewModel.Machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Machine UpdateMachine(ViewModel.Machine machine)
        {
            EFDataModel.DevOps.Machine efMachine = ReturnEfMachine(machine);
            if (efMachine.id == 0)
            {
                DevOpsContext.Machines.Add(efMachine);
            }
            DevOpsContext.SaveChanges();
            return GetMachine(machine.machine_name);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/13/2017. </remarks>
        ///
        /// <param name="machine">  The machine. </param>
        ///
        /// <returns>   The new machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Machine CreateMachine(ViewModel.Machine machine)
        {
            EFDataModel.DevOps.Machine efMachine = ReturnEfMachine(machine);
            if (efMachine.id == 0)
            {
                DevOpsContext.Machines.Add(efMachine);
            }
            DevOpsContext.SaveChanges();
            return GetMachine(machine.machine_name);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the machine described by ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/13/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   A ViewModel.Machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Machine DeleteMachine(int id)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns view model machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/19/2017. </remarks>
        ///
        /// <param name="efMachine">    The ef machine. </param>
        ///
        /// <returns>   The view model machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.Machine ReturnVmMachine(EFDataModel.DevOps.Machine efMachine)
        {
            List<ViewModel.Application> vmApplications = new List<ViewModel.Application>();
            bool machineHasNote = DevOpsContext.Notes.Where(x => x.note_id == efMachine.id).ToList().Count() > 0;
            foreach (var app in efMachine.MachineApplicationMaps.Where(x => x.active == true).ToList().Select(y => y.Application).ToList())
            {
                vmApplications.Add(appManager.ReturnApplicationVariable(app));
            }
            return new ViewModel.Machine()
            {
                id = efMachine.id,
                machine_name = efMachine.machine_name,
                ip_address = efMachine.ip_address,
                location = efMachine.location,
                environment = efMachine.environment,
                uri = efMachine.uri,
                create_date = efMachine.create_date,
                modify_date = efMachine.modify_date,
                active = efMachine.active,
                hasNotes = machineHasNote,
                Applications = vmApplications,
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns ef machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/19/2017. </remarks>
        ///
        /// <param name="vmMachine">    The view model machine. </param>
        ///
        /// <returns>   The ef machine. </returns>
        ///-------------------------------------------------------------------------------------------------
        private EFDataModel.DevOps.Machine ReturnEfMachine(ViewModel.Machine vmMachine)
        {
            EFDataModel.DevOps.Machine efMachine = DevOpsContext.Machines.Where(x => x.machine_name == vmMachine.machine_name).FirstOrDefault();
            List<EFDataModel.DevOps.MachineApplicationMap> efApplicationMaps = new List<MachineApplicationMap>();
            if (efMachine == null)
            {
                foreach (var app in vmMachine.Applications)
                {
                    efApplicationMaps.Add(new MachineApplicationMap()
                    {
                        active = app.active,
                        application_id = app.id,
                    });
                }
                return new EFDataModel.DevOps.Machine()
                {
                    id = 0,
                    machine_name = vmMachine.machine_name,
                    ip_address = vmMachine.ip_address,
                    location = vmMachine.location,
                    environment = vmMachine.environment,
                    uri = vmMachine.uri,
                    create_date = vmMachine.create_date,
                    modify_date = vmMachine.modify_date ?? DateTime.Now,
                    active = vmMachine.active,
                    MachineApplicationMaps = efApplicationMaps,
                };
            }
            else
            {
                List<EFDataModel.DevOps.Application> efApps = DevOpsContext.Applications.Where(x => vmMachine.Applications.Select(y => y.application_name).ToList().Contains(x.application_name)).ToList();
                efApplicationMaps = ReturnEfMachineAppPathMap(efMachine.id);
                foreach (var app in efApplicationMaps.Where(x => efApps.Any(y => y.id == x.application_id)).ToList())
                {
                    efMachine.MachineApplicationMaps.Remove(app);
                    efApplicationMaps.Remove(app);
                }
                foreach (var app in efApps.Where(x => !efApplicationMaps.Select(y => y.application_id).ToList().Contains(x.id)).ToList())
                {
                    efMachine.MachineApplicationMaps.Add(new MachineApplicationMap()
                    {
                        application_id = app.id,
                        active = vmMachine.Applications.Where(x => x.application_name == app.application_name).Select(y => y.active).FirstOrDefault(),
                    });
                    efApps.Remove(app);
                }
                foreach (var app in efApps)
                {
                    app.active = vmMachine.Applications.Where(x => x.application_name == app.application_name).Select(y => y.active).FirstOrDefault();
                }
            }
            return efMachine;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns ef machine application path map. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/19/2017. </remarks>
        ///
        /// <param name="machine_id">   Identifier for the machine. </param>
        ///
        /// <returns>   The ef machine application path map. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<EFDataModel.DevOps.MachineApplicationMap> ReturnEfMachineAppPathMap(int machine_id)
        {
            return DevOpsContext.MachineApplicationMaps.Where(x => x.machine_id == machine_id).ToList();
        }
    }
}
