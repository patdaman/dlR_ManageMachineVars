using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using CommonUtils.IISAdmin;
using EFDataModel.DevOps;

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
            string currentDomain = Environment.UserDomainName;
            string currentUser = Environment.UserName;
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
            machineApps.AddRange(GetMachineApps("hqdev08.dev.corp.printable.com"));
            return machineApps;
        }

        public List<IISAppSettings> GetMachineApps(string machineName = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                    this.machineName = machineName;
                if (_siteTools == null)
                    _siteTools = new SiteTools(this.machineName);
                List<IISAppSettings> machineApps = new List<IISAppSettings>();
                List<WebSite> machineSites = _siteTools.GetAllSites(machineName);
                foreach (WebSite site in machineSites)
                {
                    machineApps.Add(GetApplication(site.name, machineName));
                }
                return machineApps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _siteTools.Dispose();
            }
        }

        public IISAppSettings GetApplication(string appName, string machineName = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(machineName))
                {
                    if ((!string.IsNullOrWhiteSpace(this.machineName) && this.machineName != machineName)
                        || _siteTools == null)
                    {
                        if (!string.IsNullOrWhiteSpace(this.machineName) && this.machineName != machineName)
                        {
                            this.machineName = machineName;
                        }
                        _siteTools = new SiteTools(this.machineName);
                    }
                }
                else if (_siteTools == null)
                {
                    if (string.IsNullOrWhiteSpace(this.machineName))
                        _siteTools = new SiteTools();
                    else
                        _siteTools = new SiteTools(this.machineName);
                }
                WebSite siteProperties = _siteTools.GetSite(appName);
                IISAppSettings machineApps = new IISAppSettings()
                {
                    active = siteProperties.active,
                    appPoolName = siteProperties.appPoolName,
                    hostName = siteProperties.hostName,
                    ipAddress = siteProperties.ipAddress,
                    name = siteProperties.name,
                    physicalPath = siteProperties.physicalPath,
                    serverName = siteProperties.serverName,
                    siteId = siteProperties.siteId,
                    state = siteProperties.state,
                    bindings = new List<SiteBinding>(),
                };
                foreach (var binding in siteProperties.bindings)
                {
                    machineApps.bindings.Add(new SiteBinding()
                    {
                        bindingInformation = binding.bindingInformation,
                        bindingProtocol = binding.bindingProtocol,
                        host = binding.host,
                    });
                };
                return machineApps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _siteTools.Dispose();
            }
        }

        public IISAppSettings UpdateApplicationSetting(IISAppSettings value)
        {
            throw new NotImplementedException();
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
