using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.IISAdmin
{
    public class IISTools
    {
        public ServerManager server = new ServerManager();

        public IISTools()
        { }

        public void GetAllSites()
        {
            SiteCollection sites = server.Sites;
            foreach (Site site in sites)
            {
                ApplicationDefaults defaults = site.ApplicationDefaults;

                //get the name of the ApplicationPool under which the Site runs
                string appPoolName = defaults.ApplicationPoolName;

                ConfigurationAttributeCollection attributes = defaults.Attributes;
                foreach (ConfigurationAttribute configAttribute in attributes)
                {
                    //put code here to work with each ConfigurationAttribute
                }

                ConfigurationAttributeCollection attributesCollection = site.Attributes;
                foreach (ConfigurationAttribute attribute in attributesCollection)
                {
                    //put code here to work with each ConfigurationAttribute
                }

                //Get the Binding objects for this Site
                BindingCollection bindings = site.Bindings;
                foreach (Microsoft.Web.Administration.Binding binding in bindings)
                {
                    //put code here to work with each Binding
                }

                //retrieve the State of the Site
                ObjectState siteState = site.State;

                //Get the list of all Applications for this Site
                ApplicationCollection applications = site.Applications;
                foreach (Microsoft.Web.Administration.Application application in applications)
                {
                    //put code here to work with each Application
                }
            }
        }

        public void GetApplicationPools()
        {
            ApplicationPoolCollection applicationPools = server.ApplicationPools;
            foreach (ApplicationPool pool in applicationPools)
            {
                //get the AutoStart boolean value
                bool autoStart = pool.AutoStart;

                //get the name of the ManagedRuntimeVersion
                string runtime = pool.ManagedRuntimeVersion;

                //get the name of the ApplicationPool
                string appPoolName = pool.Name;

                //get the identity type
                ProcessModelIdentityType identityType = pool.ProcessModel.IdentityType;

                //get the username for the identity under which the pool runs
                string userName = pool.ProcessModel.UserName;

                //get the password for the identity under which the pool runs
                string password = pool.ProcessModel.Password;
            }
        }
    }
}
