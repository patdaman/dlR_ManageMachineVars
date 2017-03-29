using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppConfigurationWebApp.Controllers
{
    public class ConfigPublishApiController : ApiController
    {
        private BusinessLayer.ManageAppConfigVariables configProcessor { get; set; }

        public HttpResponseMessage Get()
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables();
                return Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.GetPublishValues());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(string environment)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables()
                {
                    environment = environment ?? string.Empty,
                };
                return Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.GetPublishValues());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/configValues/5
        public HttpResponseMessage Get(int id, string environment = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables()
                {
                    environment = environment ?? string.Empty,
                };
                return Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.GetPublishValues(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

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

        public HttpResponseMessage Post(ViewModel.AppVar value, string environment = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables()
                {
                    environment = environment ?? string.Empty,
                };
                var response = Request.CreateResponse<ViewModel.AttributeKeyValuePair>(HttpStatusCode.OK, configProcessor.PublishValue(value));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(List<ViewModel.AppVar> value)
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

        public HttpResponseMessage Post(List<ViewModel.AppVar> value, string environment = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables()
                {
                    environment = environment ?? string.Empty,
                };
                var response = Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.PublishValue(value));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(string environment)
        {
            try
            {
                return Post(environment);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }


        public HttpResponseMessage Post(string environment)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageAppConfigVariables()
                {
                    environment = environment ?? string.Empty,
                };
                var response = Request.CreateResponse<ViewModel.AttributeKeyValuePair>(HttpStatusCode.OK, configProcessor.PublishValue());
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/configValues/5
        //  Rollback to previous publish...
        //  Todo!!
        public void Delete(ViewModel.AppVar value)
        {
        }
    }
}
