using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SignalrWebService.Performance
{
    public class MyProcess
    {
        public static string processName { get; set; }
        public static List<string> processNames { get; set; }
        public static string machineIp { get; set; }
        public static List<string> machineIps { get; set; }
        public static string machineName { get; set; }
        public static List<string> machineNames { get; set; }
        public static int? processId { get; set; }
        public static List<int> processIds { get; set; }

        public Process GetCurrentProcess()
        {
            return Process.GetCurrentProcess();
        }

        public Process[] GetAllProcesses()
        {
            // Get all processes running on the local computer.
            return Process.GetProcesses();
        }

        // Get all instances of Notepad running on the local computer.
        // This will return an empty array if notepad isn't running.
        public Process[] GetProcessByName(string inputProcessName)
        {
            if (string.IsNullOrWhiteSpace(processName) && !string.IsNullOrWhiteSpace(inputProcessName))
                processName = inputProcessName;
            if (!string.IsNullOrWhiteSpace(processName))
                return Process.GetProcessesByName(processName);
            else
                throw new InvalidOperationException("No Process Name Supplied for GetProcessByName");
        }

        public Process[] GetProcessByName(List<string> inputProcessNames)
        {
            List<Process> processList = new List<Process>();
            foreach (var proc in processNames)
            {
                processList.AddRange(Process.GetProcessesByName(proc));
            }
            return processList.ToArray();
        }

        // Get a process on the local computer, using the process id.
        // This will throw an exception if there is no such process.
        public Process GetProcessById(int inputProcessId)
        {
            if ((processId == null || processId == 0) && inputProcessId != 0)
                processId = inputProcessId;
            return Process.GetProcessById(processId.Value);
        }

        public Process[] GetProcessById(List<int> inputProcessIds)
        {
            List<Process> processList = new List<Process>();
            foreach (var proc in processIds)
            {
                processList.Add(GetProcessById(proc));
            }
            return processList.ToArray();
        }

        // Get processes running on a remote computer. Note that this
        // and all the following calls will timeout and throw an exception
        // if "myComputer" and 169.0.0.0 do not exist on your local network.

        // Get all processes on a remote computer.
        public Process[] GetAllRemoteProcesses()
        {
            return Process.GetProcesses(machineName);
        }

        // Get all instances of Notepad running on the specific computer, using machine name.
        public Process[] GetRemoteProcessByName()
        {
            return Process.GetProcessesByName(processName, machineName);
        }

        // Get all instances of Notepad running on the specific computer, using IP address.
        public Process[] GetRemoteProcessByNameAndIp()
        {
            return Process.GetProcessesByName(processName, machineIp);
        }

        // Get a process on a remote computer, using the process id and machine name.
        public Process GetRemoteProcessByIdMachineName()
        {
            if (processId != null && processId != 0)
                return Process.GetProcessById(processId.Value, machineName);
            else
                throw new InvalidOperationException("No process id supplied for GetRemoteProcessByIdMachineName");
        }
    }
}