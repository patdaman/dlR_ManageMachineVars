using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class ManageMachines
    {
        DevOpsEntities DevOpsContext = new DevOpsEntities();

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
                machineModels.Add(new ViewModel.Machine()
                {
                    id = machine.id,
                    machine_name = machine.machine_name,
                    ip_address = machine.ip_address,
                    location = machine.location,
                    usage = machine.usage,
                    create_date = machine.create_date,
                    modify_date = machine.modify_date,
                    active = machine.active
                });
            }
            return machineModels;
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
            var machineModel = new ViewModel.Machine()
            {
                id = EFMachine.id,
                machine_name = EFMachine.machine_name,
                ip_address = EFMachine.ip_address,
                location = EFMachine.location,
                usage = EFMachine.usage,
                create_date = EFMachine.create_date,
                modify_date = EFMachine.modify_date,
                active = EFMachine.active
            };
            return machineModel;
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
    }
}
