using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class MachineController : BaseController
    {
        #if RELEASE
        [Authorize(Roles = "Engineers")]
#endif
        // GET: Machine
        public ActionResult Index()
        {
            return View();
        }
    }
}