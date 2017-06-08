using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
    public class IISApiController : BaseController
    {
        BusinessLayer.ManageIIS iisProcessor = new BusinessLayer.ManageIIS();

        // GET: api/IISApi
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.IISMonitor>>(HttpStatusCode.OK, iisProcessor.GetAllApplications());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/IISApi/5
        [HttpGet]
        public HttpResponseMessage Get(int machineId, string appName)
        {
            try
            {
                return Request.CreateResponse<ViewModel.IISMonitor>(HttpStatusCode.OK, iisProcessor.GetApplication(machineId, appName));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/IISApi
        //#if RELEASE
        //    [Authorize(Roles = "DevOps")]
        //#endif
        [HttpPut]
        public HttpResponseMessage Put(ViewModel.IISAppSettings value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.IISAppSettings>(HttpStatusCode.OK, iisProcessor.UpdateApplicationSetting(value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/IISApi
        //#if RELEASE
        //    [Authorize(Roles = "DevOps")]
        //#endif
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.IISAppSettings value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.IISAppSettings>(HttpStatusCode.OK, iisProcessor.CreateApplicationSetting(value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/IISApi/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                return Request.CreateResponse<ViewModel.IISAppSettings>(HttpStatusCode.OK, iisProcessor.DeleteApplicationSetting(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
