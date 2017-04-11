using CommonUtils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ViewData["AppBuildTag"] = System.Configuration.ConfigurationManager.AppSettings["AppBuildTag"];
            ViewData["ApiUri"] = System.Configuration.ConfigurationManager.AppSettings["ApiUri"];
            ViewData["TitleTag"] = System.Configuration.ConfigurationManager.AppSettings["TitleTag"];

            if (ViewData["AppBuildTag"] == null ||
                ViewData["ApiUri"] == null ||
                ViewData["TitleTag"] == null)
            {
                throw new Exception("Error required AppSettings data was null. E.g., AppBuildTag,  ApiUri, etc.");
            }
        }
    }
}