using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ManageApplications
    {
        public string userName { get; set; }
        DevOpsEntities DevOpsContext;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        public ManageApplications()
        {
            DevOpsContext = new DevOpsEntities();
        }
        public ManageApplications(DevOpsEntities entities)
        {
            DevOpsContext = entities;
        }

        public ManageApplications(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <returns>   The application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Application> GetApplication()
        {
            List<ViewModel.Application> applications = new List<ViewModel.Application>();
            var efApplications = DevOpsContext.Applications.ToList();
            foreach (var c in efApplications)
            {
                applications.Add(ReturnApplicationVariable(c));
            }
            return applications;
        }

        public ViewModel.Application GetApplication(int id)
        {
            var efApplication = DevOpsContext.Applications.Where(x => x.id == id).FirstOrDefault();
            return ReturnApplicationVariable(efApplication);
        }

        public ViewModel.Application GetApplication(string applicationName)
        {
            var efApplication = DevOpsContext.Applications.Where(x => x.application_name == applicationName).FirstOrDefault();
            return ReturnApplicationVariable(efApplication);
        }

        public List<ViewModel.Application> GetApplicationsFromCsv(string apps)
        {
            List<string> appList = new List<string>();
            List<ViewModel.Application> vmAppList = new List<ViewModel.Application>();
            if (!string.IsNullOrWhiteSpace(apps))
            {
                appList = apps.Split(',').ToList();
            }
            foreach (string app in appList)
            {
                vmAppList.Add(GetApplication(app));
            }
            return vmAppList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets ef application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/20/2017. </remarks>
        ///
        /// <param name="vmApps">   The view model apps. </param>
        ///
        /// <returns>   The ef application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EFDataModel.DevOps.Application> GetEfApplication(List<ViewModel.Application> vmApps)
        {
            List<EFDataModel.DevOps.Application> efApps = new List<EFDataModel.DevOps.Application>();
            foreach (var vmApp in vmApps)
            {
                efApps.Add(GetEfApplication(vmApp));
            }
            return efApps;
        }

        public EFDataModel.DevOps.Application GetEfApplication(ViewModel.Application vmApp)
        {
            EFDataModel.DevOps.Application app;
            app = DevOpsContext.Applications.Where(x => x.application_name == vmApp.application_name).FirstOrDefault();
            if (app == null)
            {
                app = new EFDataModel.DevOps.Application()
                {
                    application_name = vmApp.application_name,
                    active = true,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    last_modify_user = this.userName,
                    release = vmApp.release ?? string.Empty,
                    EnvironmentVariables = new List<EFDataModel.DevOps.EnvironmentVariable>(),
                };
            }
            else
            {
                app.id = vmApp.id;
                app.active = vmApp.active;
                app.application_name = vmApp.application_name;
                app.modify_date = vmApp.modify_date ?? DateTime.Now;
                app.last_modify_user = vmApp.last_modify_user ?? app.last_modify_user;
                app.release = vmApp.release ?? app.release;
            }
            return app;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns application variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/15/2017. </remarks>
        ///
        /// <param name="a">    The Application to process. </param>
        ///
        /// <returns>   The application variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Application ReturnApplicationVariable(EFDataModel.DevOps.Application a)
        {
            return new ViewModel.Application()
            {
                active = a.active,
                application_name = a.application_name,
                create_date = a.create_date,
                id = a.id,
                modify_date = a.modify_date,
                last_modify_user = a.last_modify_user,
                release = a.release,
                Components = EfToVmConverter.EfComponentListToVm(a.Components).ToList(),
            };
        }
    }
}
