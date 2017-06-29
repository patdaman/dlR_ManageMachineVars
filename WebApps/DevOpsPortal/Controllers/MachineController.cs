using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class MachineController : BaseController
    {
#if RELEASE
        [Authorize(Roles = "Engineers")]
#endif
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}