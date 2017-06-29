using ApiLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
                return Request.CreateResponse<List<ViewModel.ApplicationInsights>>(HttpStatusCode.OK, appinsightProcessor.GetAllInsights());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Machine
        [HttpGet]
        public HttpResponseMessage Get(string applicationName, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                //return client.GetAsync(@"beta/apps/f2dc9f8e-0e73-4555-ab74-d7a54f0ee9fb/metrics/performanceCounters/memoryAvailableBytes?timespan=P1D&interval=PT30M").Result;
                return Request.CreateResponse<ViewModel.ApplicationInsights>(HttpStatusCode.OK, ClientApi<ViewModel.ApplicationInsights>.GetSingleAsync(ClientApi<ViewModel.ApplicationInsights>.ArgType.TypeExactString, @"beta/apps/f2dc9f8e-0e73-4555-ab74-d7a54f0ee9fb/metrics/performanceCounters/memoryAvailableBytes?timespan=P1D&interval=PT30M").Result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Machine/5
        [HttpGet]
        public HttpResponseMessage Get(int machineId)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.ApplicationInsights>>(HttpStatusCode.OK, ClientApi<List<ViewModel.ApplicationInsights>>.GetAsync(@"beta/apps/f2dc9f8e-0e73-4555-ab74-d7a54f0ee9fb/metrics/performanceCounters/memoryAvailableBytes?timespan=P1D&interval=PT30M").Result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
