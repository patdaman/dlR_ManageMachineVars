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
        public string key { get; set; }
        public string value { get; set; }
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
        public List<Binding> bindings { get; set; }
        public List<WebApplication> webApplications { get; set; }

        public WebSite(Site site)
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
            //state = site.State.ToString();
            siteId = site.Id.ToString();
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
