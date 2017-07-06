using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using CommonUtils.IISAdmin;
using EFDataModel.DevOps;
using System.Configuration;
using System.Web.Configuration;

namespace BusinessLayer
{
    public class ManageIIS
    {
        public string machineName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        protected static SiteTools _siteTools;

        public ManageIIS(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all applications. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="environment">  (Optional) The environment. </param>
        ///
        /// <returns>   all applications. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<IISAppSettings> GetAllApplications(string environment = null)
        {
            DevOpsEntities devOpsContext = new DevOpsEntities();
            List<EFDataModel.DevOps.Machine> efMachines = devOpsContext.Machines.ToList();
            List<IISAppSettings> machineApps = new List<IISAppSettings>();
            foreach (var machine in efMachines)
            {
                try
                {
                    machineApps.AddRange(GetMachineApps(machine.machine_name));
                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                    if (ex.InnerException != null)
                        message = message + Environment.NewLine + ex.InnerException.Message.ToString() ?? "";
                    machineApps.Add(new IISAppSettings()
                    {
                        active = null,
                        appPoolName = "unknown",
                        ipAddress = machine.ip_address ?? "unknown",
                        keepAlive = null,
                        bindings = new List<SiteBinding>(),
                        configKeys = new List<ConfigKeyVal>(),
                        hostName = string.Empty,
                        message = message,
                        name = "Unknown",
                        serverName = machine.machine_name,
                        physicalPath = "n/a",
                        state = "Unknown",
                        siteId = "n/a",
                    });
                }
            }
            return machineApps;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets machine apps. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="machineName">  (Optional) Name of the machine. </param>
        ///
        /// <returns>   The machine apps. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<IISAppSettings> GetMachineApps(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            List<IISAppSettings> machineApps = new List<IISAppSettings>();
            List<WebSite> machineSites;
            WindowsUser _windowsUser;
            if (!string.IsNullOrWhiteSpace(this.userName))
            {
                _windowsUser = new WindowsUser()
                {
                    userName = this.userName,
                };
                if (string.IsNullOrWhiteSpace(this.domain))
                    _windowsUser.domain = Environment.UserDomainName;
                else
                    _windowsUser.domain = this.domain;
                if (string.IsNullOrWhiteSpace(this.password))
                    _windowsUser.password = string.Empty;
                else
                    _windowsUser.password = this.password;
            }
            else
            {
                DomainUser _domainUser = GetDomainUser(this.machineName);
                _windowsUser = new WindowsUser()
                {
                    userName = _domainUser.username.Value,
                    password = _domainUser.password.Value,
                    domain = _domainUser.domain.Value,
                };
            }

            try
            {
                _siteTools = new SiteTools(this.machineName)
                {
                    userName = _windowsUser.userName,
                    password = _windowsUser.password,
                    domain = _windowsUser.domain,
                };
                machineSites = _siteTools.GetAllSites(machineName);
                foreach (WebSite site in machineSites)
                {
                    IISAppSettings app = GetApplication(site);
                    machineApps.Add(app);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _siteTools.Dispose(true);
            }
            return machineApps;
        }

        public IISAppSettings GetApplication(string machine, string site, string app)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets an application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="machineName">  (Optional) Name of the machine. </param>
        /// <param name="appName">      Name of the application. </param>
        ///
        /// <returns>   The application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public IISAppSettings GetApplication(string machineName, string appName)
        {
            WebSite siteProperties = _siteTools.GetSite(appName, true);
            return GetApplication(siteProperties);
        }

        public IISAppSettings GetApplication(WebSite siteProperties)
        {
            IISAppSettings machineApps = new IISAppSettings()
            {
                active = siteProperties.active,
                keepAlive = siteProperties.keepAlive,
                appPoolName = siteProperties.appPoolName,
                hostName = siteProperties.hostName,
                ipAddress = siteProperties.ipAddress,
                name = siteProperties.name,
                message = siteProperties.message,
                physicalPath = siteProperties.physicalPath,
                serverName = siteProperties.serverName,
                siteId = siteProperties.siteId,
                state = siteProperties.state,
                bindings = ConvertBindings(siteProperties.bindings),
                configKeys = ConvertConfigValues(siteProperties.configKeys),
            };
            return machineApps;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the application settings in this collection. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="machineName">  (Optional) Name of the machine. </param>
        /// <param name="appName">      (Optional)
        ///                             Name of the application. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process the application settings in this
        /// collection.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IEnumerable<ConfigKeyVal> GetAppSettings(string machineName, string appName = null)
        {
            List<IISAppSettings> newSettings = new List<IISAppSettings>();
            if (string.IsNullOrWhiteSpace(appName))
                newSettings.Add(GetApplication(machineName, appName));
            else
                newSettings.AddRange(GetMachineApps(machineName));
            List<ConfigKeyVal> newKeys = new List<ConfigKeyVal>();
            foreach (var keys in newSettings)
            {
                if (keys.configKeys != null && keys.configKeys.Count() > 0)
                {
                    newKeys.AddRange(keys.configKeys);
                }
            }
            return newKeys;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets domain user. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="machineName">  (Optional) Name of the machine. </param>
        ///
        /// <returns>   The domain user. </returns>
        ///-------------------------------------------------------------------------------------------------
        private DomainUser GetDomainUser(string machineName)
        {
            var l_configSettings = (DomainUserSection)WebConfigurationManager.GetSection("DomainUserSection");
            List<DomainUser> rootUsers = new List<DomainUser>();
            foreach (var l_element in l_configSettings.DomainUsers.AsEnumerable())
            {
                rootUsers.Add(l_element);
            }
            return rootUsers.Where(x => machineName.EndsWith(x.uri.Value)).FirstOrDefault();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert bindings. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="iisBindings">  The iis bindings. </param>
        ///
        /// <returns>   The bindings converted. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<SiteBinding> ConvertBindings(List<Binding> iisBindings)
        {
            List<SiteBinding> vmBindings = new List<SiteBinding>();
            foreach (var binding in iisBindings)
            {
                vmBindings.Add(new SiteBinding()
                {
                    bindingInformation = binding.bindingInformation,
                    bindingProtocol = binding.bindingProtocol,
                    host = binding.host,
                });
            };
            return vmBindings;
        }

        private List<ConfigKeyVal> ConvertConfigValues(List<ConfigKeyValue> iisConfig, bool? detail = null)
        {
            List<ConfigKeyVal> vmConfig = new List<ConfigKeyVal>();
            foreach (var config in iisConfig)
            {
                vmConfig.Add(new ConfigKeyVal()
                {
                    key = config.key,
                    value = config.value,
                });
            }
            return vmConfig;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enumerates update application settings in this collection. </summary>
        ///
        /// <remarks>   Pdelosreyes, 7/6/2017. </remarks>
        ///
        /// <param name="_machineAppRequests">  The machine application requests. </param>
        /// <param name="configAction">         (Optional) The configuration action. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process update application settings in this
        /// collection.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IEnumerable<ConfigKeyVal> UpdateAppSettings(List<IISAppSettings> _machineAppRequests, string configAction = null)
        {
            List<IISAppSettings> newSettings = UpdateApplicationSetting(_machineAppRequests, configAction);
            List<ConfigKeyVal> keyNames = new List<ConfigKeyVal>();
            List<ConfigKeyVal> newKeys = new List<ConfigKeyVal>();
            foreach (var keys in newSettings)
            {
                var updateKey = _machineAppRequests.Where(x => x.name == keys.name).Select(y => y.configKeys).FirstOrDefault();
                if (keys.configKeys != null && keys.configKeys.Count() > 0)
                {
                    newKeys.AddRange(keys.configKeys.Where(x => updateKey.Select(y => y.key).Contains(x.key)));
                }
            }
            return newKeys;
        }

        public IISAppSettings UpdateApplicationSetting(IISAppSettings value, string configAction = null)
        {
            //WebSite siteProperties = _siteTools.GetSite(value.name, true);
            //WebSite updateSiteProperties = _siteTools.AddUpdateWebSite(siteProperties);

            WebSite dto = new WebSite()
            {
                active = value.active,
                appPoolName = value.appPoolName,
                bindings = GetIISBindings(value.bindings),
                configKeys = GetIISConfigKeys(value.configKeys),
                hostName = value.hostName,
                ipAddress = value.ipAddress,
                keepAlive = value.keepAlive,
                message = value.message,
                name = value.name,
                physicalPath = value.physicalPath,
                serverName = value.serverName,
                siteId = value.siteId,
                state = value.state,
            };
            WebSite updateSiteProperties = _siteTools.AddUpdateWebSite(dto);
            return GetApplication(updateSiteProperties);
        }

        public List<IISAppSettings> UpdateApplicationSetting(List<IISAppSettings> value, string configAction = null)
        {
            List<WebSite> dto = new List<WebSite>();
            List <IISAppSettings> _iisAppSettings = new List<IISAppSettings>();
            foreach (var site in value)
            {
                dto.Add(new WebSite()
                {
                    active = site.active,
                    appPoolName = site.appPoolName,
                    bindings = GetIISBindings(site.bindings),
                    configKeys = GetIISConfigKeys(site.configKeys),
                    hostName = site.hostName,
                    ipAddress = site.ipAddress,
                    keepAlive = site.keepAlive,
                    message = site.message,
                    recycle = site.recycle,
                    name = site.name,
                    physicalPath = site.physicalPath,
                    serverName = site.serverName,
                    siteId = site.siteId,
                    state = site.state,
                });
            }
            List<WebSite> updateSiteProperties = _siteTools.AddUpdateWebSite(dto, configAction);
            foreach (var siteProperties in updateSiteProperties)
            {
                _iisAppSettings.Add(ConvertWebSiteToIISAppSettings(siteProperties));
            }
            return _iisAppSettings;
        }

        private List<Binding> GetIISBindings(List<SiteBinding> vmBindings)
        {
            List<Binding> iisBindings = new List<Binding>();
            if (vmBindings != null && vmBindings.Count > 0)
            {
                foreach (var b in vmBindings)
                {
                    iisBindings.Add(new Binding()
                    {
                        bindingInformation = b.bindingInformation,
                        bindingProtocol = b.bindingProtocol,
                        host = b.host,
                    });
                }
            }
            return iisBindings;
        }

        private List<SiteBinding> GetVmBindings(List<Binding> iisBindings)
        {
            List<SiteBinding> vmBindings = new List<SiteBinding>();
            if (iisBindings != null && iisBindings.Count > 0)
            {
                foreach (var b in vmBindings)
                {
                    vmBindings.Add(new SiteBinding()
                    {
                        bindingInformation = b.bindingInformation,
                        bindingProtocol = b.bindingProtocol,
                        host = b.host,
                    });
                }
            }
            return vmBindings;
        }

        private List<ConfigKeyValue> GetIISConfigKeys(List<ConfigKeyVal> vmConfigKeys)
        {
            List<ConfigKeyValue> IISConfigKeys = new List<ConfigKeyValue>();
            if (vmConfigKeys != null && vmConfigKeys.Count > 0)
            {
                foreach (var c in vmConfigKeys)
                {
                    IISConfigKeys.Add(new ConfigKeyValue()
                    {
                        key = c.key,
                        value = c.value,
                    });
                }
            }
            return IISConfigKeys;
        }

        private List<ConfigKeyVal> GetVmConfigKeys(List<ConfigKeyValue> iisConfigKeys)
        {
            List<ConfigKeyVal> vmConfigKeys = new List<ConfigKeyVal>();
            if (iisConfigKeys != null && iisConfigKeys.Count > 0)
            {
                foreach (var c in vmConfigKeys)
                {
                    vmConfigKeys.Add(new ConfigKeyVal()
                    {
                        key = c.key,
                        value = c.value,
                    });
                }
            }
            return vmConfigKeys;
        }

        public IISAppSettings CreateApplicationSetting(IISAppSettings value)
        {
            throw new NotImplementedException();
        }

        public IISAppSettings DeleteApplicationSetting(int id)
        {
            throw new NotImplementedException();
        }

        private IISAppSettings ConvertWebSiteToIISAppSettings(WebSite site)
        {
            return new IISAppSettings()
            {
                active = site.active,
                appPoolName = site.appPoolName,
                bindings = GetVmBindings(site.bindings),
                configKeys = GetVmConfigKeys(site.configKeys),
                hostName = site.hostName,
                ipAddress = site.ipAddress,
                keepAlive = site.keepAlive,
                message = site.message,
                name = site.name,
                physicalPath = site.physicalPath,
                serverName = site.serverName,
                siteId = site.siteId,
                state = site.state,
            };
        }
    }
}
