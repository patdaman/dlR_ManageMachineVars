using SignalrWebService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalrWebService.Controllers
{
    public class PerformanceApiController : ApiControllerWithHub<PerformanceHub>
    {
        BusinessLayer.ManageMachines machineProcessor = new BusinessLayer.ManageMachines();

        public HttpResponseMessage GetSystemInfo()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.SystemInfo>>(HttpStatusCode.OK, machineProcessor.GetAllMachineInfo());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage GetSystemInfo(string machineName)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Event>>(HttpStatusCode.OK, machineProcessor.GetAllMachineInfo(machineName));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
