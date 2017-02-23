using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ManageAppConfig.Controllers
{
    public class MachineApiController : ApiController
    {
        BusinessLayer.ManageMachines machineProcessor = new BusinessLayer.ManageMachines();

        // GET: api/Machine
        //public HttpResponseMessage Get()
        //{
        //    try
        //    {
        //        return Request.CreateResponse<List<ViewModel.Machine>>(HttpStatusCode.OK, machineProcessor.GetAllMachines());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        public List<ViewModel.Machine> Get()
        {
            try
            {
                return machineProcessor.GetAllMachines();
            }
            catch (Exception ex)
            {
                return new List<ViewModel.Machine>();
            }
        }

        // GET: api/Machine/5
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
