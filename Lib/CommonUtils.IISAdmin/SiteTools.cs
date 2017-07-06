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
    public class SiteTools/* : Base*/
    {
        public string machineName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string domain { get; set; }

        private WindowsUser _impersonateUser { get; set; }
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
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
        }

        public SiteTools(WindowsUser user)
        {
            _impersonateUser = user;
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
                this.machineName = machineName;
            }
            if (string.IsNullOrWhiteSpace(this.machineName))
            {
                machineName = Environment.MachineName;
            }
            IPAddress[] ips = Dns.GetHostAddresses(this.machineName);
            List<WebSite> sites = new List<WebSite>();
            using (server = GetServerManager(this.machineName))
            {
                foreach (var site in server.Sites)
                {
                    sites.Add(new WebSite(site)
                    {
                        serverName = machineName ?? Environment.MachineName,
                        ipAddress = ips[0].ToString(),
                    });
                };
            }
            return sites;
        }

        public WebSite GetSite(string siteName, bool detail = false)
        {
            IPAddress[] ips = Dns.GetHostAddresses(this.machineName);
            WebSite site = new WebSite();
            using (server = GetServerManager(this.machineName))
            {
                site = new WebSite(server.Sites.Where(x => x.Name == siteName).FirstOrDefault(), detail)
                {
                    serverName = this.machineName ?? Environment.MachineName,
                    ipAddress = ips[0].ToString(),
                };
            }
            return site;
        }

        public WebSite AddUpdateWebSite(WebSite site, string configAction = null)
        {
            WebSite newSite = new WebSite();
            using (server = GetServerManager(this.machineName))
            {
                Site mySite = server.Sites.Where(x => x.Name == site.name).FirstOrDefault();
                if (mySite == null)
                {
                    mySite = CreateSite(site.serverName, site.ipAddress, site.siteId, site.name, site.hostName, site.physicalPath);
                }
                else
                {
                    if (site.active == false)
                        mySite.Stop();
                    if (site.active == true)
                        mySite.Start();
                    Configuration config = mySite.GetWebConfiguration();
                    try
                    {
                        if (config.GetSection("appSettings") != null)
                        {
                            ConfigurationSection appSettings = config.GetSection("appSettings");
                            ConfigurationElementCollection appSettingsCollection = appSettings.GetCollection();
                            foreach (var key in appSettingsCollection)
                            {
                                if (key.Attributes[1] != null)
                                {
                                    var setting = site.configKeys.Where(k => k.key == key.Attributes[0].Value.ToString()).FirstOrDefault();
                                    if (key.Attributes[0].Value.ToString().EndsWith("KeepAlive"))
                                    {
                                        if (site.keepAlive == true)
                                            key.Attributes[1].Value = "true";
                                        if (site.keepAlive == false)
                                            key.Attributes[1].Value = "false";
                                    }
                                    if (setting != null)
                                    {
                                        key.Attributes[1].Value = setting.value;
                                    }
                                }
                            }
                            if (configAction.ToLower() == "add")
                            {
                                List<string> newKeys = site.configKeys.Select(s => s.key).Except(appSettingsCollection.Select(k => k.Attributes[0].Value.ToString())).ToList();
                                List<ConfigKeyValue> keys = site.configKeys.Where(x => newKeys.Contains(x.key)).ToList();
                                foreach (var newKey in keys)
                                {
                                    ConfigurationElement addElement = appSettingsCollection.CreateElement("add");
                                    addElement["key"] = newKey.key;
                                    addElement["value"] = newKey.value;
                                    appSettingsCollection.Add(addElement);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        newSite.message = ex.Message;
                    }
                }
                server.CommitChanges();
                //newSite = new WebSite(server.Sites.Where(x => x.Name == site.name).FirstOrDefault(), true);
                newSite = new WebSite(server.Sites.Where(x => x.Name == site.name).FirstOrDefault());
            }
            return newSite;
        }

        public List<WebSite> AddUpdateWebSite(List<WebSite> sites, string configAction = null)
        {
            List<WebSite> newSites = new List<WebSite>();
            List<Site> mySites = new List<Site>();
            List<string> siteNames = new List<string>();
            using (server = GetServerManager(this.machineName))
            {
                foreach (var site in sites)
                {
                    if (string.IsNullOrWhiteSpace(site.name) && site.siteId == null)
                    {
                        mySites.AddRange(server.Sites.ToList());
                        break;
                    }
                    else
                    {
                        Site mySite = server.Sites.Where(x => x.Name == site.name).FirstOrDefault();
                        mySites.Add(mySite);
                        if (mySites == null || mySites.Count == 0)
                        {
                            mySites.Add(CreateSite(site.serverName, site.ipAddress, site.siteId, site.name, site.hostName, site.physicalPath));
                        }
                    };
                }
                foreach (var site in sites)
                {
                    List<Site> iisSites = mySites.Where(x => x.Name == site.name).ToList();
                    if (iisSites == null || iisSites.Count() == 0)
                        iisSites = mySites;
                    foreach (var iisSite in iisSites)
                    {
                        siteNames.Add(iisSite.Name);
                        if (site.active == false)
                            iisSite.Stop();
                        if (site.active == true)
                            iisSite.Start();
                        Configuration config = iisSite.GetWebConfiguration();
                        try
                        {
                            if (config.GetSection("appSettings") != null)
                            {
                                ConfigurationSection appSettings = config.GetSection("appSettings");
                                ConfigurationElementCollection appSettingsCollection = appSettings.GetCollection();
                                foreach (var key in appSettingsCollection)
                                {
                                    if (key.Attributes[1] != null && key.Attributes[0].Value.ToString().EndsWith("KeepAlive"))
                                    {
                                        if (site.keepAlive == true)
                                            key.Attributes[1].Value = "true";
                                        if (site.keepAlive == false)
                                            key.Attributes[1].Value = "false";
                                    }
                                }
                                if (configAction.ToLower() == "add")
                                {
                                    List<string> newKeys = site.configKeys.Select(s => s.key).Except(appSettingsCollection.Select(k => k.Attributes[0].Value.ToString())).ToList();
                                    List<ConfigKeyValue> keys = site.configKeys.Where(x => newKeys.Contains(x.key)).ToList();
                                    foreach (var newKey in keys)
                                    {
                                        ConfigurationElement addElement = appSettingsCollection.CreateElement("add");
                                        addElement["key"] = newKey.key;
                                        addElement["value"] = newKey.value;
                                        appSettingsCollection.Add(addElement);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            newSites.Add(new WebSite()
                            {
                                name = site.name,
                                active = site.active,
                                hostName = site.hostName,
                                appPoolName = site.appPoolName,
                                ipAddress = site.ipAddress,
                                keepAlive = site.keepAlive,
                                state = site.state,
                                siteId = site.siteId,
                                serverName = site.serverName,
                                message = ex.Message,
                            });
                        }
                    }
                }
                server.CommitChanges();
                foreach (var siteName in siteNames)
                {
                    //newSites.Add(new WebSite(server.Sites.Where(x => x.Name == siteName).FirstOrDefault(), true));
                    newSites.Add(new WebSite(server.Sites.Where(x => x.Name == siteName).FirstOrDefault()));
                }
            }
            return newSites;
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
            {
                if (domainUser != null)
                    domainUser.Dispose();
                handle.Dispose();
            }
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
            this._impersonateUser = new WindowsUser()
            {
                userName = this.userName,
                password = this.password,
                domain = this.domain,
            };
            if (string.IsNullOrWhiteSpace(this._impersonateUser.domain))
                this._impersonateUser.domain = Environment.UserDomainName;
            return GetWindowsUser(this._impersonateUser);
        }

        private WindowsImpersonationContext GetWindowsUser(WindowsUser windowsUser)
        {
            var user = new ImpersonateUser();
            return user.GetUser(windowsUser.userName, windowsUser.password, windowsUser.domain);
        }
    }
}
