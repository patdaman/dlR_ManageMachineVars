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

        public List<IISAppSettings> GetAllApplications()
        {
            DevOpsEntities devOpsContext = new DevOpsEntities();
            List<EFDataModel.DevOps.Machine> efMachines = devOpsContext.Machines.ToList();
            List<IISAppSettings> machineApps = new List<IISAppSettings>();
            //foreach (var machine in efMachines)
            //{
            //    machineApps.AddRange(GetMachineApps(machine.machine_name));
            //}
            try
            {
                //machineApps.AddRange(GetMachineApps("hal9000"));
                //machineApps.AddRange(GetMachineApps("hqdev08.dev.corp.printable.com"));
                //machineApps.AddRange(GetMachineApps("sdapp02.dc.pti.com"));
                machineApps.AddRange(GetMachineApps("sdmgr02.dc.pti.com"));
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                if (ex.InnerException != null)
                    message = message + Environment.NewLine + ex.InnerException.Message.ToString() ?? "";
            }
            return machineApps;
        }

        public List<IISAppSettings> GetMachineApps(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            List<IISAppSettings> machineApps = new List<IISAppSettings>();
            List<WebSite> machineSites;
            string userName;
            string password;
            string domain;
            try
            {

                /// For Testing purposes
                ///  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                ///  !! Repair / Remove when deployed !!
                ///  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                userName = "pdelosreyes";
                password = "Patman7474!";
                domain = "printable";
                //this._impersonateUser = new WindowsUser()
                //{
                //    userName = "patman",
                //    password = "Patman7474!",
                //    domain = "hal9000",
                //};
                //userName = "root";
                //password = "ok2m40tK!";
                //domain = "dc";
                ///  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                ///  !! Repair / Remove when deployed !!
                ///  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                var l_configSettings = (DomainUserSection)WebConfigurationManager.GetSection("DomainUserSection");

                foreach (var l_element in l_configSettings.DomainUsers.AsEnumerable())
                {
                    Console.WriteLine(l_element.Key);

                    foreach (var l_subElement in l_element.UserDetails.AsEnumerable())
                    {
                        Console.WriteLine(l_subElement.Key);
                    }

                }

                _siteTools = new SiteTools(this.machineName)
                {
                    userName = userName,
                    password = password,
                    domain = domain,
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
