using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
    public class ApplicationApiController : BaseController
    {
//#if RELEASE
//    [Authorize(Roles = "Engineers")]
//#endif
        BusinessLayer.ManageConfig_ComplexVariables configProcessor = new BusinessLayer.ManageConfig_ComplexVariables();
        BusinessLayer.ManageApplications appProcessor = new BusinessLayer.ManageApplications();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the get. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        ///  GET: api/ConfigApi
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Application>>(HttpStatusCode.OK, appProcessor.GetApplication());
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
        public HttpResponseMessage Get(int applicationId)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Application>(HttpStatusCode.OK, appProcessor.GetApplication(applicationId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a HTTP response message using the given component name. </summary>
        ///
        /// <remarks>   Patman, 4/14/2017. </remarks>
        ///
        /// <param name="componentName">    The component name to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get(string applicationName)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Application>(HttpStatusCode.OK, appProcessor.GetApplication(applicationName));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/20/2017. </remarks>
        ///
        /// <param name="componentName">    The component name to get. </param>
        /// <param name="filePath">         Full pathname of the file. </param>
        /// <param name="applicationNames"> List of names of the applications. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.ApplicationDto applicationModel)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(applicationModel.name))
                    throw new ArgumentNullException("Application Name must be provided.");
                configProcessor.userName = applicationModel.last_modify_user;
                return Request.CreateResponse<ViewModel.Application>(HttpStatusCode.OK, configProcessor.AddUpdateApplication(applicationModel));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}

