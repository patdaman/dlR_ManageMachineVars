using CommonUtils.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppConfigurationWebApp.Controllers
{
    public class ConfigApiController : ApiController
    {
        BusinessLayer.ManageComplexConfigVariables configProcessor = new BusinessLayer.ManageComplexConfigVariables();

        // GET: api/Machine
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

        public HttpResponseMessage Get(int machineId)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.AppVar>>(HttpStatusCode.OK, configProcessor.GetMachineVariables(machineId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(int varId, string envType)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
