using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class WebAppDto
    {
        public WebAppDto()
        { }
        public Nullable<int> machine_id { get; set; }
        public string machine_name { get; set; }
        public string location { get; set; }
        public string environment { get; set; }
        public System.DateTime create_date { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public bool machineActive { get; set; }
        public List<IISMonitor> Monitors { get; set; } 
        public List<Application> Applications {get; set; }

        public WebAppDto(WebAppDto x)
        {

        }
    }

    public class IISMonitor
    {
        public IISMonitor()
        { }


        public IISMonitor(IISMonitor x)
        {

        }
    }

    public class IISAppSettings
    {
        public IISAppSettings()
        { }


        public IISAppSettings(IISAppSettings x)
        {

        }
    }

    public class IISAppPool
    {
        public bool autoStart { get; set; }
        public string runtime { get; set; }
        public string appPoolName { get; set; }
        public ProcessIdentityType identityType { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public IISWorkerProcess workerProcess { get; set; }
    }

    public class IISWorkerProcess
    {
        public IISWorkerProcess()
        { }
        public string appPoolName { get; set; }
        public string processName { get; set; }
        public Nullable<int> processId { get; set; }
        public double avgCpu { get; set; }
        public double avgMem { get; set; }

        public IISWorkerProcess(IISWorkerProcess x)
        {

        }
    }

    public enum ProcessIdentityType
    {
        LocalSystem = 0,
        LocalService = 1,
        NetworkService = 2,
        SpecificUser = 3,
        ApplicationPoolIdentity = 4
    }
}
