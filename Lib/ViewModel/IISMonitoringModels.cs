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
        public List<Application> Applications { get; set; }

        public WebAppDto(WebAppDto x)
        {

        }
    }

    public class IISAppSettings
    {
        public IISAppSettings() { }
        public string siteId { get; set; }
        public string name { get; set; }
        public string serverName { get; set; }
        public string ipAddress { get; set; }
        public string hostName { get; set; }
        public string appPoolName { get; set; }
        public string physicalPath { get; set; }
        public string state { get; set; }
        public Nullable<bool> active { get; set; }
        public List<SiteBinding> bindings { get; set; }
        public List<ConfigKeyVal> configKeys { get; set; }

        public IISAppSettings(IISAppSettings x)
        {

        }
    }

    public class ConfigKeyVal
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class SiteBinding
    {
        public SiteBinding() { }
        public string host { get; set; }
        public string bindingInformation { get; set; }
        public string bindingProtocol { get; set; }
    }

    public class IISMonitor
    {
        public IISMonitor()
        { }


        public IISMonitor(IISAppSettings x)
        {

        }
    }

    public class IISAppPool
    {
        public bool autoStart { get; set; }
        public string runtime { get; set; }
        public string appPoolName { get; set; }
        public string identityType { get; set; }
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
}
