using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using ViewModel;

namespace DevOpsApi.Controllers
{
#if RELEASE
    [Authorize(Roles = "Engineers")]
#endif
    public class ConfigPublishApiController : ApiController
    {
        private BusinessLayer.ManageConfig_Files configProcessor { get; set; }
        private static string ConfigFilePath = @"D:\ConfigFiles\";
        // private static string ConfigFilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];

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
        public HttpResponseMessage Download(string componentName, string environment, string fileName = null)
        {
            BusinessLayer.ManageConfig_Files fileProcessor = new BusinessLayer.ManageConfig_Files()
            {
                componentName = componentName,
                environment = environment,
                fileName = fileName ?? "",
            };
            try
            {
                ConfigXml configFile = fileProcessor.GetConfigXml();
                fileName = configFile.fileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = Path.GetFileName(configFile.path);
                if (!fileName.EndsWith(".config") && (!fileName.EndsWith(".xml")))
                    fileName = fileName + ".config";
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
                            //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
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
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Posts the configuration file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/11/2017. </remarks>
        ///
        /// <exception cref="HttpResponseException">    Thrown when a HTTP Response error condition
        ///                                             occurs. </exception>
        /// <exception cref="Exception">                Thrown when an exception error condition occurs. </exception>
        ///
        /// <returns>   A Task&lt;IHttpActionResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<IHttpActionResult> PostConfigFile(string componentName, string environment, string applications, string action = null)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            if (string.IsNullOrWhiteSpace(action))
                action = "import";
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var fullFileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var sections = fullFileName.Split('\\');
                var fileName = sections[sections.Length - 1];
                var fileExt = fileName.Split('.')[1];
                var buffer = await file.ReadAsStreamAsync();
                var saveFilePath = ConfigFilePath + @"\" + componentName + @"\" + environment + @"\";
                Directory.CreateDirectory(saveFilePath);
                saveFilePath = saveFilePath + fileName;
                BusinessLayer.ManageConfig_Files fileProcessor = new BusinessLayer.ManageConfig_Files()
                {
                    componentName = componentName,
                    environment = environment,
                    appName = applications.ToString(),
                    outputPath = saveFilePath,
                };
                try
                {
                    XDocument configFile = XDocument.Load(buffer);
                    ConfigXml newConfigObject = new ConfigXml();
                    if (fileExt.ToLower() != "config" && fileExt.ToLower() != "xml")
                    {
                        throw new Exception("File type not valid: " + fileExt);
                    }
                    if (action.Equals("publish"))
                    {
                        if (File.Exists(saveFilePath))
                            try
                            {
                                File.Delete(saveFilePath);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("File Name already exists: " + fileName, ex);
                            }
                        using (var fileStream = File.Create(saveFilePath))
                        {
                            buffer.CopyTo(fileStream);
                        }
                        fileProcessor.PublishFile(configFile);
                    }
                    else
                    {
                        fileProcessor.UploadConfigFile(configFile);
                    }
                }
                catch (Exception ex)
                {
                    var response = Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
                    return ResponseMessage(response);
                }
            }
            return Ok();
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
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPost]
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
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPut]
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
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpPost]
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
        [HttpPut]
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
        [HttpPost]
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
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpDelete]
        public void Delete(ViewModel.AppVar value)
        {
        }
    }
}
