using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;

namespace DevOpsApi.Controllers
{
//#if RELEASE
//    [Authorize(Roles = "Engineers")]
//#endif
    public class MachineApiController : BaseController
    {
        BusinessLayer.ManageMachines machineProcessor = new BusinessLayer.ManageMachines();

        //// GET: api/Machine
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Machine>>(HttpStatusCode.OK, machineProcessor.GetAllMachines());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Machine/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Machine>(HttpStatusCode.OK, machineProcessor.GetMachine(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

//#if RELEASE
//    [Authorize(Roles = "DevOps")]
//#endif
        [HttpPut]
        public HttpResponseMessage Put(ViewModel.Machine value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Machine>(HttpStatusCode.OK, machineProcessor.UpdateMachine(value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/Machine
//#if RELEASE
//    [Authorize(Roles = "DevOps")]
//#endif
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.Machine value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Machine>(HttpStatusCode.OK, machineProcessor.CreateMachine(value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/Machine/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Machine>(HttpStatusCode.OK, machineProcessor.DeleteMachine(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
