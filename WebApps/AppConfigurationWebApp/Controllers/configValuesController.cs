using CommonUtils.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppConfigurationWebApp.Controllers
{
    public class configValuesController : ApiController
    {
        BusinessLayer.ManageComplexConfigVariables configProcessor = new BusinessLayer.ManageComplexConfigVariables();
        public AppConfigFunctions appConfigVars { get; private set; }

        // GET: api/Machine
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

        // GET: api/configValues/5
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


        public HttpResponseMessage Post(ViewModel.ConfigVariableValue value)
        {
            try
            {
                var response = Request.CreateResponse<ViewModel.ConfigVariableValue>(HttpStatusCode.OK, configProcessor.UpdateValue(value));
                //appConfigVars = new AppConfigFunctions();
                //var updateConfig = appConfigVars.UpdateOrCreateAppSetting(value.keyName, value.key, value.valueName, value.value, value.configParentElement, value.configAttribute);
                //if (updateConfig == ViewModel.Enums.ModifyResult.Failed || updateConfig == ViewModel.Enums.ModifyResult.AccessDenied)
                //    return Request.CreateResponse<ViewModel.AppVar>(HttpStatusCode.ExpectationFailed, value, "Config stored in database, but not in file" + Environment.NewLine + "Key Update Code: " + updateConfig.ToString());
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
