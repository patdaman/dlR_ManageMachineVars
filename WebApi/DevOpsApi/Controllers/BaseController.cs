using CommonUtils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DevOpsApi.Controllers
{
    //#if RELEASE
    //    [Authorize(Roles = "Engineers")]
    //#endif
    [EnableCors("*", "*", "*","*")]
    public class BaseController : ApiController
    {
        private static readonly log4net.ILog iLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoggingPatternUser logger = new LoggingPatternUser() { Logger = iLog };
        
        public BaseController()
        {
            logger.Info(String.Format("BaseController getting called. Page Load at {0}", DateTime.Now.ToString()));
        }
    }
}
