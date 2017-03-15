using EFDataModel.DevOps;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ManageLogging
    {
        public string machineName { get; set; }
        public List<string> machineList { get; set; }
        private DevOpsEntities devOpsContext { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageLogging()
        {
            devOpsContext = new DevOpsEntities();
            if (machineList == null)
                machineList = new List<string>();
            if (machineList.Count() == 0 && !string.IsNullOrWhiteSpace(machineName))
                machineList.Add(machineName);
        }

        public ManageLogging(string conn)
        {
            devOpsContext = new DevOpsEntities(conn);
            if (machineList == null)
                machineList = new List<string>();
            if (machineList.Count() == 0 && !string.IsNullOrWhiteSpace(machineName))
                machineList.Add(machineName);
        }

        public ManageLogging(DevOpsEntities devOps)
        {
            devOpsContext = devOps;
            if (machineList == null)
                machineList = new List<string>();
            if (machineList.Count() == 0 && !string.IsNullOrWhiteSpace(machineName))
                machineList.Add(machineName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all logs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <returns>   all logs. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Event> GetAllLogs()
        {
            return GetAllLogs(DateTime.MinValue, DateTime.MaxValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all logs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <param name="startDate">    The start date. </param>
        /// <param name="endDate">      The end date. </param>
        ///
        /// <returns>   all logs. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Event> GetAllLogs(DateTime startDate, DateTime endDate)
        { 
            List<ViewModel.Event> events = new List<ViewModel.Event>();
            List<EFDataModel.DevOps.Event> efEvents = devOpsContext.Events
                                                        //.Where(x => x.Date > startDate && x.Date < endDate)
                                                        //.Select(y => y)
                                                        .ToList();
            foreach (EFDataModel.DevOps.Event efEvent in efEvents)
            {
                events.Add(new ViewModel.Event()
                {
                    id = efEvent.id,
                    device_id = efEvent.device_id,
                    Hostname = efEvent.Hostname,
                    Date = efEvent.Date,
                    Time = efEvent.Time,
                    Priority = efEvent.Priority,
                    Message = efEvent.Message,
                });
            }
            return events;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets machine logs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <returns>   The machine logs. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Event> GetMachineLogs()
        {
            return GetMachineLogs(DateTime.MinValue, DateTime.MaxValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets machine logs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <param name="startDate">    The start date. </param>
        /// <param name="endDate">      The end date. </param>
        ///
        /// <returns>   The machine logs. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Event> GetMachineLogs(DateTime startDate, DateTime endDate)
        { 
            List<ViewModel.Event> events = new List<ViewModel.Event>();
            List<EFDataModel.DevOps.Event> efEvents = devOpsContext.Events
                                                        .Where(x => machineList.Contains(x.Hostname))
                                                        .Where(y => y.Date > startDate && y.Date < endDate)
                                                        .Select(z => z).ToList();
            foreach (EFDataModel.DevOps.Event efEvent in efEvents)
            {
                events.Add(new ViewModel.Event()
                {
                    id = efEvent.id,
                    device_id = efEvent.device_id,
                    Hostname = efEvent.Hostname,
                    Date = efEvent.Date,
                    Time = efEvent.Time,
                    Priority = efEvent.Priority,
                    Message = efEvent.Message,
                });
            }
            return events;
        }


    }
}
