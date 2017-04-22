using CommonUtils.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
    public class ConfigValuesApiController : ApiController
    {
        BusinessLayer.ManageConfig_ComplexVariables configProcessor = new BusinessLayer.ManageConfig_ComplexVariables();
        public AppConfigFunctions appConfigVars { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   GET: api/Machine. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.ConfigVariableValue>>(HttpStatusCode.OK, configProcessor.GetConfigValues());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   GET: api/configValues/5. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <param name="id">   The Identifier to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.ConfigVariableValue>>(HttpStatusCode.OK, configProcessor.GetConfigValues(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(string type)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.NameValuePair>>(HttpStatusCode.OK, configProcessor.GetDropDownValues(type));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts the given value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <param name="value">    The value to put. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Put(ViewModel.ConfigVariableValue value)
        {
            try
            {
                return Post(value);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <param name="value">    The value to put. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Post(ViewModel.ConfigVariableValue value)
        {
            try
            {
                var response = Request.CreateResponse<ViewModel.ConfigVariableValue>(HttpStatusCode.OK, configProcessor.UpdateValue(value));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/configValues/5
        public void Delete(int id)
        {
        }
    }
}
