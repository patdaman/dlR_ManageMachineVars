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
        public static SiteTools _siteTools;

        public ManageIIS(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
        }

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

        public List<IISAppSettings> GetMachineApps(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            List<IISAppSettings> machineApps = new List<IISAppSettings>();
            List<WebSite> machineSites;
            try
            {
                var l_configSettings = (DomainUserSection)WebConfigurationManager.GetSection("DomainUserSection");
                List<DomainUser> rootUsers = new List<DomainUser>();
                foreach (var l_element in l_configSettings.DomainUsers.AsEnumerable())
                {
                    rootUsers.Add(l_element);
                }
                DomainUser _domainUser = rootUsers.Where(x => this.machineName.Contains(x.uri.Value)).FirstOrDefault();
                _siteTools = new SiteTools(this.machineName)
                {
                    userName = _domainUser.username.Value,
                    password = _domainUser.password.Value,
                    domain = _domainUser.domain.Value,
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

        public IISAppSettings GetApplication(string machineName, string appName)
        {
            WebSite siteProperties = _siteTools.GetSite(appName, true);
            return GetApplication(siteProperties);
        }

        public IISAppSettings GetApplication(WebSite siteProperties)
        {
            //bool keepAlive = false;
            //if (siteProperties.configKeys.Contains(new ConfigKeyValue("Application.KeepAlive", "true")))
            //{
            //    keepAlive = true;
            //}
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

        public IISAppSettings UpdateApplicationSetting(IISAppSettings value)
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

        private List<Binding> GetIISBindings(List<SiteBinding> vmBindings)
        {
            List<Binding> iisBindings = new List<Binding>();
            foreach (var b in vmBindings)
            {
                iisBindings.Add(new Binding()
                {
                    bindingInformation = b.bindingInformation,
                    bindingProtocol = b.bindingProtocol,
                    host = b.host,
                });
            }
            return iisBindings;
        }

        private List<ConfigKeyValue> GetIISConfigKeys(List<ConfigKeyVal> vmConfigKeys)
        {
            List<ConfigKeyValue> IISConfigKeys = new List<ConfigKeyValue>();
            foreach (var c in vmConfigKeys)
            {
                IISConfigKeys.Add(new ConfigKeyValue()
                {
                    key = c.key,
                    value = c.value,
                });
            }
            return IISConfigKeys;
        }

        public IISAppSettings CreateApplicationSetting(IISAppSettings value)
        {
            throw new NotImplementedException();
        }

        public IISAppSettings DeleteApplicationSetting(int id)
        {
            throw new NotImplementedException();
        }
    }
}
