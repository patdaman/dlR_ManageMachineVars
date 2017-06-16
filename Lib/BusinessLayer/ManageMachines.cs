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

        public List<ViewModel.Event> GetAllMachineInfo(string machineName)
        {
            throw new NotImplementedException();
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
        public ViewModel.Machine UpdateMachine(ViewModel.Machine value)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        private ViewModel.Machine ReturnVmMachine(EFDataModel.DevOps.Machine efMachine)
        {
            List<ViewModel.Application> vmApplications = new List<ViewModel.Application>();
            foreach (var app in efMachine.Applications)
            {
                vmApplications.Add(appManager.ReturnApplicationVariable(app));
            }
            return new ViewModel.Machine()
            {
                id = efMachine.id,
                machine_name = efMachine.machine_name,
                ip_address = efMachine.ip_address,
                location = efMachine.location,
                environment = efMachine.usage,
                create_date = efMachine.create_date,
                modify_date = efMachine.modify_date,
                active = efMachine.active,
                Applications = vmApplications,
            };
        }
    }
}
