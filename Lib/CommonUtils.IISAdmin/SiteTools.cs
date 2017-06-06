using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtils.IISAdmin
{
    public class SiteTools
    {
        public string machineName { get; set; }
        public static ServerManager server;

        public SiteTools(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            if (string.IsNullOrWhiteSpace(this.machineName))
                server = new ServerManager();
            else
                server = ServerManager.OpenRemote(this.machineName);
        }

        public static SiteCollection GetAllSites(string machineName)
        {
            return server.Sites;
        }

        public string GetAppPoolName(Site site)
        {
            ApplicationDefaults defaults = site.ApplicationDefaults;
            return defaults.ApplicationPoolName;
        }

        public static BindingCollection GetSiteBindings(Site site)
        {
            return site.Bindings;
        }

        public static ObjectState GetSiteState(Site site)
        {
            return site.State;
        }

        public static ApplicationCollection GetSiteApplications(Site site)
        { 
            return site.Applications;
        }

        public static List<WebAppPoolModel> GetApplicationPoolSettings()
        {
            List<WebAppPoolModel> pools = new List<WebAppPoolModel>();
            ApplicationPoolCollection applicationPools = server.ApplicationPools;
            foreach (ApplicationPool pool in applicationPools)
            {
                pools.Add(new WebAppPoolModel()
                {
                    autoStart = pool.AutoStart,
                    workers = pool.WorkerProcesses,
                    runtime = pool.ManagedRuntimeVersion,
                    appPoolName = pool.Name,
                    identityType = pool.ProcessModel.IdentityType,
                    userName = pool.ProcessModel.UserName,
                    password = pool.ProcessModel.Password,
                });
            }
            return pools;
        }

        public ApplicationPool AddUpdateApplicationPool(string appPoolName, string managedRuntimeVersion = "v4.0", ProcessModelIdentityType identityType = ProcessModelIdentityType.NetworkService)
        {
            ApplicationPool myApplicationPool = null;
            if (server.ApplicationPools != null && server.ApplicationPools.Count > 0)
            {
                if (server.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName) == null)
                {
                    //if we find the pool already there, we will get a reference to it for update
                    myApplicationPool = server.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
                }
                else
                {
                    //if the pool is not already there we will create it
                    myApplicationPool = server.ApplicationPools.Add(appPoolName);
                }
            }
            else
            {
                //if the pool is not already there we will create it
                myApplicationPool = server.ApplicationPools.Add(appPoolName);
            }

            if (myApplicationPool != null)
            {
                myApplicationPool.ProcessModel.IdentityType = identityType;
                myApplicationPool.ManagedRuntimeVersion = managedRuntimeVersion;
                server.CommitChanges();
            }
            return server.ApplicationPools.Where(x => x.Name == appPoolName).FirstOrDefault();
        }
    }
}
