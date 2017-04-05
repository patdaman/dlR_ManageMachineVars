using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppConfigurationWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.DeveloperRole = "Engineers";
            return View();
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}