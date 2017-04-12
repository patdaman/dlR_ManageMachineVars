using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using ViewModel;

namespace DevOpsApi.Controllers
{
    public class ConfigPublishApiController : ApiController
    {
        private BusinessLayer.ManageConfig_Files configProcessor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the get. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get()
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files();
                return Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.GetPublishValues());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a HTTP response message using the given environment. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="environment">  The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Get(string environment)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files()
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">           The identifier. </param>
        /// <param name="environment">  (Optional)
        ///                             The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpGet]
        [Route("ConfigPublishApi/download")]
        public HttpResponseMessage Download(string componentName, string environment)
        {
            BusinessLayer.ManageConfig_Files fileProcessor = new BusinessLayer.ManageConfig_Files()
            {
                componentName = componentName,
                environment = environment,
            };
            try
            {
                ConfigXml configFile = fileProcessor.GetConfigXml();
                string fileName = Path.GetFileName(configFile.path);
                if (configFile != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (StreamWriter writer = new StreamWriter(memoryStream))
                        {
                            writer.Write(configFile.text);
                            writer.Flush();
                            memoryStream.Position = 0;
                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(memoryStream.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
            //try
            //{
            //    configProcessor = new BusinessLayer.ManageConfig_Files()
            //    {
            //        environment = environment ?? string.Empty,
            //    };
            //    return Request.CreateResponse<List<ViewModel.AttributeKeyValuePair>>(HttpStatusCode.OK, configProcessor.GetPublishValues(id));
            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            //}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts the given value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    The value to put. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
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
        /// <param name="value">        The value to put. </param>
        /// <param name="environment">  (Optional)
        ///                             The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Post(ViewModel.AppVar value, string environment = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files()
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts the given value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    The value to put. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">        The value to put. </param>
        /// <param name="environment">  (Optional)
        ///                                                        The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Post(List<ViewModel.AppVar> value, string environment = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files()
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="environment">  The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="environment">  (Optional)
        ///                                                        The environment to get. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Post(string environment)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files()
                {
                    environment = environment ?? string.Empty,
                };
                var response = Request.CreateResponse(HttpStatusCode.OK);
                //var response = Request.CreateResponse<ViewModel.AttributeKeyValuePair>(HttpStatusCode.OK, configProcessor.PublishValue());
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
