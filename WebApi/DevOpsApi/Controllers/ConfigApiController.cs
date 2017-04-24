using CommonUtils.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
#if RELEASE
    [Authorize(Roles = "Engineers")]
#endif
    public class ConfigApiController : ApiController
    {
        BusinessLayer.ManageConfig_ComplexVariables configProcessor = new BusinessLayer.ManageConfig_ComplexVariables();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the get. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        ///  GET: api/ConfigApi
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.AppVar>>(HttpStatusCode.OK, configProcessor.GetAllConfigVariables());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="varId">    Identifier for the variable. </param>
        /// <param name="envType">  (Optional) Type of the environment. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        ///   GET: api/ConfigApi/varId=x,envType=yyyy
        [HttpGet]
        public HttpResponseMessage Get(int varId, string envType = null)
        {
            try
            {
                return Request.CreateResponse<ViewModel.AppVar>(HttpStatusCode.OK, configProcessor.GetVariable(varId, envType));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a HTTP response message using the given component name. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/3/2017. </remarks>
        ///
        /// <param name="componentName">    The component name to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(string componentName, string environment)
        {
            BusinessLayer.ManageConfig_Files configFileProcessor = new BusinessLayer.ManageConfig_Files()
            {
                componentName = componentName,
                environment = environment
            };
            try
            {
                return Request.CreateResponse<ViewModel.ConfigXml>(HttpStatusCode.OK, configFileProcessor.GetConfigXml());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts the given value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    . </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPut]
        public HttpResponseMessage Put(ViewModel.AppVar value)
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
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    . </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.AppVar value)
        {
            try
            {
                var response = Request.CreateResponse<ViewModel.AppVar>(HttpStatusCode.OK, configProcessor.UpdateVariable(value));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the given ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">   The Identifier to delete. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                return Request.CreateResponse<ViewModel.AppVar>(HttpStatusCode.OK, configProcessor.DeleteVariable(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
