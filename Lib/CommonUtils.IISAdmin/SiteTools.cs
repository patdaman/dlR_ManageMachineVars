using Microsoft.Web.Administration;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace CommonUtils.IISAdmin
{
    public class SiteTools : IDisposable
    {
        public string machineName { get; set; }
        private static ServerManager server;
        private static ConfigTools configTool = new ConfigTools();

        private bool disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private static WindowsImpersonationContext domainUser;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/7/2017. </remarks>
        ///
        /// <param name="machineName">  (Optional)
        ///                             Name of the machine. </param>
        ///-------------------------------------------------------------------------------------------------
        public SiteTools(string machineName = null)
        {
            server = GetServerManager(machineName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all sites. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/7/2017. </remarks>
        ///
        /// <param name="machineName">  Name of the machine. </param>
        ///
        /// <returns>   all sites. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<WebSite> GetAllSites(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
            {
                if (this.machineName != machineName)
                {
                    this.machineName = machineName;
                }
                if (server == null)
                    server = GetServerManager(this.machineName);
            }
            if (string.IsNullOrWhiteSpace(this.machineName))
            {
                machineName = Environment.MachineName;
                if (server == null)
                    server = GetServerManager();
            }
            IPAddress[] ips = Dns.GetHostAddresses(this.machineName);
            List<WebSite> sites = new List<WebSite>();
            foreach (var site in server.Sites)
            {
                sites.Add(new WebSite(site)
                {
                    serverName = machineName ?? Environment.MachineName,
                    ipAddress = ips[0].ToString(),
                });
            };
            return sites;
        }

        public WebSite GetSite(string siteName)
        {
            return new WebSite(server.Sites.Where(x => x.Name == siteName).FirstOrDefault());
        }

        public WebSite AddUpdateWebSite(WebSite site)
        {
            Site mySite = server.Sites.Where(x => x.Name == site.name).FirstOrDefault();
            if (mySite == null)
            {
                mySite = CreateSite(site.serverName, site.ipAddress, site.siteId, site.name, site.hostName, site.physicalPath);
            }
            else
            {
                if (site.state.ToLower().StartsWith("stop"))
                    mySite.Stop();
                if (site.state.ToLower().StartsWith("start"))
                    mySite.Start();
                if (site.active == true)
                    mySite.Attributes.Where(x => x.Name.EndsWith("KeepAlive")).FirstOrDefault().Value = "true";
                if (site.active == false)
                    mySite.Attributes.Where(x => x.Name.EndsWith("KeepAlive")).FirstOrDefault().Value = "false";
            }

            return new WebSite(server.Sites.Where(x => x.Name == site.name).FirstOrDefault());
        }

        public Site CreateSite(string computerName,
                                         string computerIp,
                                         string siteID,
                                         string siteName,
                                         string hostName,
                                         string physicalPath,
                                         string port = "443",
                                         bool ssl = true,
                                         string username = null,
                                         string password = null,
                                         string loggingDir = null,
                                         string applicationPool = "DefaultAppPool")
        {
            try
            {
                string http = "http";
                if (ssl)
                    http = "https";
                string bindinginfo = computerIp + ":" + port + ":" + hostName;

                if (server.Sites.Where(x => x.Name == siteName).FirstOrDefault() == null)
                {
                    Site mySite = server.Sites.Add(siteName.ToString(), http, bindinginfo, physicalPath);
                    mySite.ApplicationDefaults.ApplicationPoolName = applicationPool;
                    mySite.TraceFailedRequestsLogging.Enabled = true;
                    mySite.TraceFailedRequestsLogging.Directory = loggingDir;
                    server.CommitChanges();
                    server.Sites.Where(x => x.Name == siteName).FirstOrDefault().Start();
                }
                else
                {
                    throw new Exception("Name should be unique, " + siteName + " already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed in CreateSite", ex);
            }
            return server.Sites.Where(x => x.Name == siteName).FirstOrDefault();
            //return GetSite(siteName);
        }

        public WebAppPoolModel GetAppPool(string appPoolName)
        {
            return new WebAppPoolModel(server.ApplicationPools.Where(x => x.Name == appPoolName).FirstOrDefault());
        }

        public List<WebAppPoolModel> GetApplicationPools()
        {
            List<WebAppPoolModel> pools = new List<WebAppPoolModel>();
            foreach (ApplicationPool pool in server.ApplicationPools)
            {
                pools.Add(new WebAppPoolModel(pool));
            }
            return pools;
        }

        public WebAppPoolModel AddUpdateApplicationPool(string appPoolName, string managedRuntimeVersion = "v4.0", string identityType = "NetworkService", bool autoStart = true)
        {
            return AddUpdateApplicationPool(new WebAppPoolModel()
            {
                name = appPoolName,
                runtimeVersion = managedRuntimeVersion,
                identityType = identityType,
                autoStart = autoStart,
            });
        }

        public WebAppPoolModel AddUpdateApplicationPool(WebAppPoolModel appPoolModel)
        {
            ApplicationPool myApplicationPool = null;
            if (server.ApplicationPools != null && server.ApplicationPools.Count > 0)
            {
                myApplicationPool = server.ApplicationPools.FirstOrDefault(p => p.Name == appPoolModel.name);
                if (myApplicationPool != null)
                {
                    myApplicationPool = server.ApplicationPools.FirstOrDefault(p => p.Name == appPoolModel.name);
                }
                else
                {
                    myApplicationPool = server.ApplicationPools.Add(appPoolModel.name);
                }
            }
            else
            {
                myApplicationPool = server.ApplicationPools.Add(appPoolModel.name);
            }
            if (myApplicationPool != null)
            {
                ProcessModelIdentityType identityEnum = new ProcessModelIdentityType();
                Enum.TryParse<ProcessModelIdentityType>(appPoolModel.identityType, true, out identityEnum);
                if (myApplicationPool.ProcessModel.IdentityType != identityEnum
                    || myApplicationPool.ManagedRuntimeVersion != appPoolModel.runtimeVersion
                    || myApplicationPool.AutoStart != appPoolModel.autoStart
                    || myApplicationPool.State.ToString().ToLower() != appPoolModel.state.ToLower())
                {
                    myApplicationPool.ProcessModel.IdentityType = identityEnum;
                    myApplicationPool.ManagedRuntimeVersion = appPoolModel.runtimeVersion;
                    myApplicationPool.AutoStart = appPoolModel.autoStart;
                    if (appPoolModel.state.ToLower().StartsWith("stop"))
                        myApplicationPool.Stop();
                    if (appPoolModel.state.ToLower().StartsWith("start"))
                        myApplicationPool.Start();
                    server.CommitChanges();
                }
            }
            return new WebAppPoolModel(server.ApplicationPools.Where(x => x.Name == appPoolModel.name).FirstOrDefault());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
                handle.Dispose();
            disposed = true;
        }

        private ServerManager GetServerManager(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;

            if (string.IsNullOrWhiteSpace(this.machineName))
                return new ServerManager();
            else
            {

                domainUser = GetWindowsUser();
                return ServerManager.OpenRemote(this.machineName);
            }
        }

        private WindowsImpersonationContext GetWindowsUser()
        {
            //WindowsUser user = new WindowsUser()
            //{
            //    userName = "pdelosreyes",
            //    password = "Patman7474!",
            //    domain = "printable",
            //};
            WindowsUser user = new WindowsUser()
            {
                userName = "patman",
                password = "Patman7474!",
                domain = "hal9000",
            };
            return GetWindowsUser(user);
        }

        private WindowsImpersonationContext GetWindowsUser(WindowsUser windowsUser)
        {
            var user = new ImpersonateUser();
            return user.GetUser(windowsUser.userName, windowsUser.password, windowsUser.domain);
        }
    }
}
