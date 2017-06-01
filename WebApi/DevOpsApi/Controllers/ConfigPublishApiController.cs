using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
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
        private static string ConfigFilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];

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
                fileName = fileName,
            };
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                ConfigXml configFile = fileProcessor.GetConfigXml();
                fileName = configFile.fileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = Path.GetFileName(configFile.path);
                if (!fileName.ToLower().EndsWith(".config") && (!fileName.ToLower().EndsWith(".xml")))
                    fileName = fileName + ".config";
                if (configFile != null)
                {
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    using (StreamWriter writer = new StreamWriter(memoryStream))
                    //    {
                            writer.Write(configFile.text);
                            writer.Flush();
                            memoryStream.Position = 0;
                            httpResponseMessage.Content = new ByteArrayContent(memoryStream.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = fileName,
                                //CreationDate = DateTime.Now,
                            };
                            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                    //    }
                    //}
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
        public async Task<IHttpActionResult> PostConfigFile(string componentName, string environment, string applications, string userName)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var fullFileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var sections = fullFileName.Split('\\');
                var fileName = sections[sections.Length - 1];
                var fileExt = fileName.Split('.')[1];
                var buffer = await file.ReadAsStreamAsync();
                if (string.IsNullOrWhiteSpace(componentName))
                    componentName = fileName.Split('.')[0];
                var saveFilePath = ConfigFilePath + @"\" + componentName + @"\" + environment + @"\";
                Directory.CreateDirectory(saveFilePath);
                saveFilePath = saveFilePath + fileName;
                BusinessLayer.ManageConfig_Files fileProcessor = new BusinessLayer.ManageConfig_Files()
                {
                    componentName = componentName,
                    environment = environment,
                    appName = applications.ToString().Replace("\"","").Replace("\'","").Replace("[","").Replace("]",""),
                    outputPath = saveFilePath,
                    userName = userName,
                };
                try
                {
                    XDocument configFile = XDocument.Load(buffer);
                    ConfigXml newConfigObject = new ConfigXml();
                    if (fileExt.ToLower() != "config" && fileExt.ToLower() != "xml")
                    {
                        throw new Exception("File type not valid: " + fileExt);
                    }
                    fileProcessor.UploadConfigFile(configFile);
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
        /// <summary>   Puts. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/26/2017. </remarks>
        ///
        /// <param name="applicationId">    Identifier for the application. </param>
        /// <param name="environment">      (Optional)
        ///                                                            The environment to get. </param>
        /// <param name="userName">         (Optional) Name of the user. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Put (int applicationId, string environment, string userName = null)
        {
            try
            {
                return Post(applicationId, environment, userName);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/26/2017. </remarks>
        ///
        /// <param name="applicationId">    Identifier for the application. </param>
        /// <param name="environment">      (Optional)
        ///                                                            The environment to get. </param>
        /// <param name="userName">         (Optional) Name of the user. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        public HttpResponseMessage Post(int applicationId, string environment, string userName = null)
        {
            try
            {
                configProcessor = new BusinessLayer.ManageConfig_Files()
                {
                    appId = applicationId,
                    environment = environment,
                    userName = userName,
                };
                var response = Request.CreateResponse<ViewModel.ApplicationDto>(HttpStatusCode.OK, configProcessor.PublishApplicationFiles(applicationId, environment, userName));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
