using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;

namespace CommonUtils.IISAdmin
{
    public class SiteTools
    {
        public string machineName { get; set; }
        private static ServerManager server;

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
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            if (string.IsNullOrWhiteSpace(this.machineName))
                server = new ServerManager();
            else
                server = ServerManager.OpenRemote(this.machineName);
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
                    server = new ServerManager(machineName);
                    this.machineName = machineName;
                }
            }
            if (string.IsNullOrWhiteSpace(this.machineName))
                machineName = Environment.MachineName;
            IPAddress[] ips = Dns.GetHostAddresses(machineName);
            List<WebSite> sites = new List<WebSite>();
            foreach (var site in server.Sites)
            {
                sites.Add(new WebSite(site)
                {
                    serverName = machineName ?? Environment.MachineName,
                    ipAddress = ips[1].ToString(),
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
    }
}
