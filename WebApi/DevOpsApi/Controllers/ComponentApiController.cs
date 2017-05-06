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
    public class ComponentApiController : ApiController
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
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Component>>(HttpStatusCode.OK, configProcessor.GetComponent());
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
        public HttpResponseMessage Get(int componentId)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Component>(HttpStatusCode.OK, configProcessor.GetComponent(componentId));
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
        /// <param name="excludeValues">    (Optional) True to exclude, false to include the values. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get(string componentName, bool excludeValues = true)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Component>(HttpStatusCode.OK, configProcessor.GetComponent(componentName, excludeValues));
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
        public HttpResponseMessage Post(ViewModel.ComponentDto componentModel)
        {

            try
            {
                ViewModel.Component newComp = new ViewModel.Component(componentModel);
                newComp.Applications = configProcessor.GetApplication(componentModel.applications);
                //return Post(newComp);
                return Request.CreateResponse<ViewModel.Component>(HttpStatusCode.OK, configProcessor.AddUpdateComponent(newComp));
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
        /// <param name="component">    . </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        //public HttpResponseMessage Put(ViewModel.Component component)
        //{
        //    try
        //    {
        //        return Post(component);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="component">    . </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///
        /// ### <param name="value">    . </param>
        ///-------------------------------------------------------------------------------------------------
        //public HttpResponseMessage Post(ViewModel.Component component)
        //{
        //    try
        //    {
        //        var response = Request.CreateResponse<ViewModel.Component>(HttpStatusCode.OK, configProcessor.AddUpdateComponent(component));
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the given ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">   The Identifier to delete. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Delete(int componentId)
        {
            try
            {
                return Request.CreateResponse<ViewModel.Component>(HttpStatusCode.OK, configProcessor.DeleteComponent(componentId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
