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
                return Request.CreateResponse<List<ViewModel.ConfigVariable>>(HttpStatusCode.OK, configProcessor.GetAllConfigVariables());
                //return Request.CreateResponse<List<ViewModel.MachineAppVars>>(HttpStatusCode.OK, configProcessor.GetAllVariables());
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
                return Request.CreateResponse<List<ViewModel.MachineAppVars>>(HttpStatusCode.OK, configProcessor.GetMachineVariables(machineId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(int varId, string varType)
        {
            try
            {
                return Request.CreateResponse<ViewModel.MachineAppVars>(HttpStatusCode.OK, configProcessor.GetGlobalVariable(varId, varType));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(int machineId, int varId, string varType)
        {
            try
            {
                return Request.CreateResponse<ViewModel.MachineAppVars>(HttpStatusCode.OK, configProcessor.GetVariable(machineId, varId, varType));
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
        public HttpResponseMessage Post(ViewModel.MachineAppVars value)
        {
            try
            {
                return Request.CreateResponse<ViewModel.MachineAppVars>(HttpStatusCode.OK, configProcessor.AddVariable(value));
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
                return Request.CreateResponse<ViewModel.MachineAppVars>(HttpStatusCode.OK, configProcessor.DeleteVariable(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
