using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModel;

namespace DeploymentWebApp.Controllers
{
    public class ScriptApiController : ApiController
    {
        BusinessLayer.ManagePowershellScripts scriptProcessor = new BusinessLayer.ManagePowershellScripts();

        // GET: api/ScriptApi
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<PowershellScript>>(HttpStatusCode.OK, scriptProcessor.GetAllScripts());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/ScriptApi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse<PowershellScript>(HttpStatusCode.OK, scriptProcessor.GetScript(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/ScriptApi
        public void Post([FromBody]string value)
        {

        }

        // POST: api/ScriptApi
        public HttpResponseMessage Post([FromBody]PowershellScript myScript)
        {
            try
            {
                // return Request.CreateResponse<PowershellScript>(HttpStatusCode.OK, scriptProcessor.    );
                return Request.CreateResponse<PowershellScript>(HttpStatusCode.OK, new PowershellScript());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/ScriptApi
        public HttpResponseMessage Post([FromBody]PowershellScriptExecution execScript)
        {
            try
            {
                return Request.CreateResponse<List<String>>(HttpStatusCode.OK, scriptProcessor.ExecuteScript(execScript.ScriptText, execScript.MachineName));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/ScriptApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ScriptApi/5
        public void Delete(int id)
        {
        }
    }
}
