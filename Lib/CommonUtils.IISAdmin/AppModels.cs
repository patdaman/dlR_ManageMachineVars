using Microsoft.Web.Administration;

namespace CommonUtils.IISAdmin
{
    public class WebAppPoolModel
    {
        //get the AutoStart boolean value
        public bool autoStart { get; set; }
        //get the name of the ManagedRuntimeVersion
        public string runtime { get; set; }
        //get the name of the ApplicationPool
        public string appPoolName { get; set; }
        public ProcessModelIdentityType identityType { get; set; }
        //get the username for the identity under which the pool runs
        public string userName { get; set; }
        //get the password for the identity under which the pool runs
        public string password { get; set; }
        public WorkerProcessCollection workers { get; set; }
    }

    public class ConfigKeyValue
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
