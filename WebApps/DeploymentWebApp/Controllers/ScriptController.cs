using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeploymentWebApp.Controllers
{
    public class ScriptController : Controller
    {
        BusinessLayer.ManagePowershell_Scripts scriptProcessor = new BusinessLayer.ManagePowershell_Scripts();
        public ActionResult Index()
        {
            List<ViewModel.PowershellScript> scripts = new List<ViewModel.PowershellScript>();
            scripts.AddRange(scriptProcessor.GetAllScripts());
            //PopulateScriptsDropDown(scripts);
            //ViewBag.scriptList = new SelectList(scripts, "id", "ScriptName", null);
            //return View(scripts);

            ViewModel.PowershellScript script = new ViewModel.PowershellScript();
            script = scriptProcessor.GetAllScripts().FirstOrDefault();
            ViewBag.scriptText = HttpUtility.HtmlEncode(script.ScriptText).ToString();
            ViewBag.scriptList = new SelectList(scripts, "ScriptId", "ScriptName", script.ScriptId);
            return View(script);
        }

        public ActionResult Execute(ViewModel.PowershellScript execScript)
        {
            var scriptResult = new ViewModel.PowershellScriptExecution(execScript);
            scriptResult.Output = String.Join(Environment.NewLine, scriptProcessor.ExecuteScript(execScript.ScriptText, Environment.MachineName));

            return Results(scriptResult);
        }

        public ActionResult Results(ViewModel.PowershellScriptExecution execScript)
        {
            return View();
        }
    }
}