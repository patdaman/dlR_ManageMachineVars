using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
    public class ApplicationInsightController : BaseController
    {
        BusinessLayer.ManageApplicationInsights appinsightProcessor = new BusinessLayer.ManageApplicationInsights();

        // GET: api/Machine
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.RootObject>>(HttpStatusCode.OK, appinsightProcessor.GetAllInsights());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Machine
        [HttpGet]
        public HttpResponseMessage Get(DateTime startDate, DateTime? endDate = null)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.RootObject>>(HttpStatusCode.OK, appinsightProcessor.GetAllInsights(startDate, endDate));
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
                return Request.CreateResponse<ViewModel.RootObject>(HttpStatusCode.OK, appinsightProcessor.GetMachineData(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPut]
        public HttpResponseMessage Put(ViewModel.RootObject value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.RootObject>(HttpStatusCode.OK, appinsightProcessor.UpdateInsight(value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/Machine
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.RootObject value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.RootObject>(HttpStatusCode.OK, appinsightProcessor.CreateInsight(value));
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
                return Request.CreateResponse<ViewModel.RootObject>(HttpStatusCode.OK, appinsightProcessor.DeleteInsight(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
