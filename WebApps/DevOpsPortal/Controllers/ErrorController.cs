using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.DeveloperRole = "ManageAppConfig_Developer";
            return View();
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}