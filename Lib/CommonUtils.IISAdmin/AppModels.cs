using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtils.IISAdmin
{
    public class WebAppPoolModel
    {
        public WebAppPoolModel()
        { }
        public string name { get; set; }
        public bool autoStart { get; set; }
        public string state { get; set; }
        public string runtimeVersion { get; set; }
        public string identityType { get; set; }
        public List<WorkerProcess> workers { get; set; }

        public WebAppPoolModel(Microsoft.Web.Administration.ApplicationPool x)
        {
            name = x.Name;
            autoStart = x.AutoStart;
            state = x.State.ToString();
            runtimeVersion = x.ManagedRuntimeVersion;
            workers = new List<WorkerProcess>();
            foreach (var worker in x.WorkerProcesses)
            {
                workers.Add(new WorkerProcess(worker));
            }
            identityType = x.ProcessModel.IdentityType.ToString();
        }
    }

    public class WorkerProcess
    {
        public WorkerProcess() { }
        public string appPoolName { get; set; }
        public string processGuid { get; set; }
        public int processId { get; set; }
        public string state { get; set; }

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess x)
        {
            appPoolName = x.AppPoolName;
            processGuid = x.ProcessGuid;
            processId = x.ProcessId;
            state = x.State.ToString();
        }
    }

    public class ConfigKeyValue
    {
        public ConfigKeyValue() { }
        public string key { get; set; }
        public string value { get; set; }
        public ConfigKeyValue(ConfigurationElement key)
        {
            if (key.Attributes[0] != null)
                this.key = key.Attributes[0].Value.ToString();
            if (key.Attributes[1] != null)
                this.value = key.Attributes[1].Value.ToString();
        }
        public ConfigKeyValue(string newKey, string newValue)
        {
            key = newKey;
            value = newValue;
        }
    }

    public class WebSite
    {
        public WebSite() { }
        public string siteId { get; set; }
        public string name { get; set; }
        public string serverName { get; set; }
        public string ipAddress { get; set; }
        public string hostName { get; set; }
        public string appPoolName { get; set; }
        public string physicalPath { get; set; }
        public string state { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<bool> keepAlive { get; set; }
        public string message { get; set; }
        public List<Binding> bindings { get; set; }
        public List<ConfigKeyValue> configKeys { get; set; }
        public List<WebApplication> webApplications { get; set; }

        public WebSite(Site site, bool? detail = null)
        {
            name = site.Name;
            appPoolName = site.ApplicationDefaults.ApplicationPoolName;
            bindings = new List<Binding>();
            foreach (var binding in site.Bindings)
            {
                bindings.Add(new Binding(binding));
            }
            webApplications = new List<WebApplication>();
            foreach (var app in site.Applications)
            {
                webApplications.Add(new WebApplication(app));
            }
            configKeys = new List<ConfigKeyValue>();
            Configuration config = site.GetWebConfiguration();
            try
            {
                List<ConfigurationSection> configSections = new List<ConfigurationSection>();
                foreach (var section in config.RootSectionGroup.SectionGroups)
                {
                    //configSections.Add(config.GetSection(section.Name));
                }
                bool keeps = false;
                if (config.GetSection("appSettings") != null)
                {
                    ConfigurationSection appSettings = config.GetSection("appSettings");
                    ConfigurationElementCollection appSettingsCollection = appSettings.GetCollection();

                    foreach (var key in appSettingsCollection)
                    {
                        configKeys.Add(new ConfigKeyValue(key));
                        if (key.Attributes[1] != null && key.Attributes[0].Value.ToString() == "Application.KeepAlive")
                        {
                            keeps = true;
                            if (key.Attributes[1].Value.ToString() == "true")
                            {
                                this.keepAlive = true;
                            }
                        }
                    }
                    if (this.keepAlive == null && keeps)
                        this.keepAlive = false;
                }
            }
            catch (Exception ex)
            {
                configKeys.Add(new ConfigKeyValue()
                {
                    key = "Exception Occured",
                    value = ex.Message,
                });
            }
            if (detail != null && detail.Value)
            {
                try
                {
                    var temp = new List<ConfigurationElement>();
                    if (config.GetSection("configSections") != null)
                    {
                        ConfigurationSection ConfigSections = config.GetSection("configSections");
                        foreach (var configFiles in ConfigSections.GetCollection())
                        {
                            temp.Add(configFiles);
                            var whatever = configFiles.ChildElements;
                        }
                    }
                }
                catch (Exception ex)
                {
                    configKeys.Add(new ConfigKeyValue()
                    {
                        key = "Exception Occured",
                        value = ex.Message,
                    });
                }
            }
            state = site.State.ToString();
            if (this.state == "Started" || this.state == "Starting")
                this.active = true;
            else
                this.active = false;
            siteId = site.Id.ToString();
            message = "Successfully retrieved at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            physicalPath = webApplications.FirstOrDefault().VirtualDirectories.FirstOrDefault().PhysicalPath;
        }
    }

    public class WebApplication
    {
        public WebApplication() { }
        string appPoolName { get; set; }
        string path { get; set; }
        public List<VirtualDirectory> VirtualDirectories { get; set; }

        public WebApplication(Microsoft.Web.Administration.Application x)
        {
            appPoolName = x.ApplicationPoolName;
            path = x.Path;
            VirtualDirectories = new List<VirtualDirectory>();
            foreach (var v in x.VirtualDirectories)
            {
                VirtualDirectories.Add(new VirtualDirectory(v));
            }
        }
    }

    public class VirtualDirectory
    {
        public string LogonMethod { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
        public string PhysicalPath { get; set; }
        public string UserName { get; set; }

        public VirtualDirectory(Microsoft.Web.Administration.VirtualDirectory x)
        {
            LogonMethod = x.LogonMethod.ToString();
            UserName = x.UserName;
            Password = x.Password;
            Path = x.Path;
            PhysicalPath = x.PhysicalPath;
        }
    }

    public class Binding
    {
        public Binding() { }
        public string host { get; set; }
        public string bindingInformation { get; set; }
        public string bindingProtocol { get; set; }

        public Binding(Microsoft.Web.Administration.Binding x)
        {
            bindingInformation = x.BindingInformation;
            bindingProtocol = x.Protocol;
            host = x.Host;
        }
    }

    public class WindowsUser
    {
        public WindowsUser() { }
        public string userName { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public WindowsUser(WindowsUser x)
        {
            userName = x.userName;
            password = x.password;
            domain = x.domain;
        }
    }
}
