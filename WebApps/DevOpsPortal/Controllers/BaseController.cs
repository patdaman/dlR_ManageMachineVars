using CommonUtils.Logging;
using System;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class BaseController : Controller
    {
        private static readonly log4net.ILog iLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoggingPatternUser logger = new LoggingPatternUser() { Logger = iLog };

        public BaseController()
        {
            logger.Info(String.Format("BaseController getting called. Page Load at {0}", DateTime.Now.ToString()));
            ViewData["ApiUri"] = System.Configuration.ConfigurationManager.AppSettings["ApiUri"];
            ViewData["SignalRPath"] = System.Configuration.ConfigurationManager.AppSettings["SignalRPath"];

            if (ViewData["ApiUri"] == null ||
                ViewData["SignalRPath"] == null)
            {
                throw new Exception("Error required AppSettings data was null: ApiUri or SignalRPath");
            }
        }
    }
}