using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using CommonUtils.IISAdmin;

namespace BusinessLayer
{
    public class ManageIIS
    {
        public string machineName { get; set; }

        public List<IISMonitor> GetAllApplications()
        {
            throw new NotImplementedException();
        }

        public IISMonitor GetApplication(string machineName, string appName)
        {
            SiteTools iisSiteProcessor = new SiteTools(machineName);
            ConfigTools iisConfigProcessor = new ConfigTools(machineName)
            {
                webAppName = appName,
            };
            iisSiteProcessor.
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
