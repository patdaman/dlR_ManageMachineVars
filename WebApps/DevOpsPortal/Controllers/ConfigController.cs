using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class ConfigController : BaseController
    {
#if DEBUG
        //[Authorize(Roles = "Engineers")]
#endif
#if RELEASE
        [Authorize(Roles = "Engineers")]
#endif
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}