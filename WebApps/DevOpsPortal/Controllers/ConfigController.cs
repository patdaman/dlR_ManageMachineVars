using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    //public class ConfigController : Controller
    public class ConfigController : BaseController
    {
        // GET: Config
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> GetAppVar()
        {
            IEnumerable<ViewModel.AppVar> appVars = null;
            try
            {
                appVars = await ApiLib.ClientApi<ViewModel.AppVar>.GetAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(appVars.ToList<ViewModel.AppVar>());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> GetAppVar(int id)
        {
            ViewModel.AppVar apiAppVar = null;
            try
            {
                apiAppVar = await ApiLib.ClientApi<ViewModel.AppVar>.GetAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(apiAppVar);
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public async Task<JsonResult> UpdateAppVar(ViewModel.AppVar pp)
        {
            try
            {
                pp = await ApiLib.ClientApi<ViewModel.AppVar>.PutAsync(pp); //put is for updates
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdateError", ex.Message);
            }
            return Json(new[] { pp });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> InsertAppVars(ViewModel.AppVar pp)
        {
            try
            {
                pp = await ApiLib.ClientApi<ViewModel.AppVar>.PostAsync(pp); //post is for inserts
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CreationError", ex.Message);
            }
            return Json(new[] { pp });
        }
    }
}