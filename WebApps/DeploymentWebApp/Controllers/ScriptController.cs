using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeploymentWebApp.Controllers
{
    public class ScriptController : Controller
    {
        // GET: Script
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(ViewModel.PowershellScriptExecution execScript)
        {
            BusinessLayer.ManagePowershellScripts scriptProcessor = new BusinessLayer.ManagePowershellScripts();
            execScript.Output = String.Join(Environment.NewLine, scriptProcessor.ExecuteScript(execScript.ScriptText, execScript.MachineName));
            return Results(execScript);
        }

        public ActionResult Results(ViewModel.PowershellScriptExecution execScript)
        {
            return View();
        }
    }
}