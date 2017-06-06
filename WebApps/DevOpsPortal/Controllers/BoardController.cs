using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class BoardController : BaseController
    {
#if DEBUG
        //[Authorize(Roles = "Engineers")]
#endif
#if RELEASE
        [Authorize(Roles = "Engineers")]
#endif
        // GET: Board
        public ActionResult Index()
        {
            return View();
        }
    }
}