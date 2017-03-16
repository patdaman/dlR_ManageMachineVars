using SignalrWebService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalrWebService.Controllers
{
    public class LogApiController : ApiControllerWithHub<EventHub>
    {
        BusinessLayer.ManageLogging logProcessor = new BusinessLayer.ManageLogging();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   GET: api/Log. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage GetAllLogs()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Event>>(HttpStatusCode.OK, logProcessor.GetAllLogs());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets api/Log/ </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <param name="startDate">    The start date. </param>
        /// <param name="endDate">      The end date. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage GetLogsByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Event>>(HttpStatusCode.OK, logProcessor.GetAllLogs(startDate, endDate));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   GET: api/Log/ </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/14/2017. </remarks>
        ///
        /// <param name="machineName">  Name of the machine. </param>
        /// <param name="startDate">    (Optional)
        ///                             The start date. </param>
        /// <param name="endDate">      (Optional)
        ///                             The end date. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage GetMachineLogs(string machineName, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (startDate == null)
                    startDate = DateTime.MinValue;
                if (startDate == null)
                    endDate = DateTime.MaxValue;
                logProcessor.machineName = machineName;
                logProcessor.machineList = new List<string>();
                logProcessor.machineList.Add(machineName);
                return Request.CreateResponse<List<ViewModel.Event>>(HttpStatusCode.OK, logProcessor.GetAllLogs(startDate.Value, endDate.Value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
