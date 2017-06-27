using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
            EFDataModel.DevOps.Machine efMachine;
            if (vmMachine.id != null && vmMachine.id != 0)
                efMachine = DevOpsContext.Machines.Where(x => x.machine_name == vmMachine.machine_name).FirstOrDefault();
            else
                efMachine = DevOpsContext.Machines.Where(x => x.id == vmMachine.id).FirstOrDefault();
            List<EFDataModel.DevOps.MachineApplicationMap> efApplicationMaps = new List<MachineApplicationMap>();
            List<EFDataModel.DevOps.Application> efApps = new List<EFDataModel.DevOps.Application>();
            List<EFDataModel.DevOps.Enum_EnvironmentType> environments = DevOpsContext.Enum_EnvironmentType.ToList();
            List<EFDataModel.DevOps.Enum_Locations> locations = DevOpsContext.Enum_Locations.ToList();
            string ipString = vmMachine.ip_address ?? string.Empty;
            string machineUrl;
            string newEnvironment = null;
            string newLocation = null;
            if (string.IsNullOrWhiteSpace(vmMachine.uri))
                machineUrl = vmMachine.machine_name;
            else
                machineUrl = vmMachine.machine_name + "." + vmMachine.uri;
            if (!string.IsNullOrWhiteSpace(vmMachine.environment))
                newEnvironment = environments.Where(x => x.value == vmMachine.environment).FirstOrDefault().value;
            if (!string.IsNullOrWhiteSpace(vmMachine.location))
                newLocation = locations.Where(x => x.value == vmMachine.location).FirstOrDefault().value;
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(machineUrl);
                foreach (var ip in ips)
                {
                    Match match = Regex.Match(ip.ToString(), @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
                    if (match.Success)
                        ipString = match.Value;
                    break;
                }
            }
            catch
            {
                ipString = vmMachine.ip_address ?? string.Empty;
            }
            if (efMachine == null)
            {
                if (vmMachine.Applications != null)
                {
                    foreach (var app in vmMachine.Applications)
                    {
                        efApplicationMaps.Add(new MachineApplicationMap()
                        {
                            active = app.active,
                            application_id = app.id,
                        });
                    }
                }
                return new EFDataModel.DevOps.Machine()
                {
                    id = 0,
                    machine_name = vmMachine.machine_name,
                    ip_address = ipString,
                    location = newLocation,
                    environment = newEnvironment,
                    uri = vmMachine.uri,
                    create_date = vmMachine.create_date,
                    modify_date = vmMachine.modify_date ?? DateTime.Now,
                    active = vmMachine.active,
                    MachineApplicationMaps = efApplicationMaps,
                };
            }
            else
            {
                if (vmMachine.Applications != null)
                {
                    List<string> vmApps = vmMachine.Applications.Select(x => x.application_name).ToList();
                    efApps = DevOpsContext.Applications.Where(x => vmApps.Contains(x.application_name)).ToList();
                    efApplicationMaps = ReturnEfMachineAppPathMap(efMachine.id);
                    foreach (var app in efApps.Where(x => !efApplicationMaps.Select(y => y.application_id).ToList().Contains(x.id)).ToList())
                    {
                        efMachine.MachineApplicationMaps.Add(new MachineApplicationMap()
                        {
                            application_id = app.id,
                            active = vmMachine.Applications.Where(x => x.application_name == app.application_name).Select(y => y.active).FirstOrDefault(),
                        });
                        efApps.Remove(app);
                    }
                }
                foreach (var app in efApplicationMaps.Where(x => efApps.Any(y => y.id == x.application_id)).ToList())
                {
                    efMachine.MachineApplicationMaps.Remove(app);
                    efApplicationMaps.Remove(app);
                }
                foreach (var app in efApps)
                {
                    app.active = vmMachine.Applications.Where(x => x.application_name == app.application_name).Select(y => y.active).FirstOrDefault();
                }
                efMachine.location = newLocation;
                efMachine.environment = newEnvironment;
                efMachine.uri = vmMachine.uri;
                efMachine.modify_date = DateTime.Now;
                efMachine.active = vmMachine.active;
                efMachine.ip_address = ipString;
                if (string.IsNullOrWhiteSpace(vmMachine.last_modify_user))
                    efMachine.last_modify_user = Environment.UserName;
                else
                    efMachine.last_modify_user = vmMachine.last_modify_user;
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
