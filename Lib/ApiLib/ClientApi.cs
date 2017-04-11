using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using ViewModel;
using System.Diagnostics;

namespace ApiLib
{
    public class ClientApi<TEntity> where TEntity : class
    {
        public enum ArgType
        {
            TypeID,
            TypeParamsFromURI,
            TypeExactString
        };

        // private static readonly string UriString = @"http://localhost:43767/api/";
        private static readonly string UriString = System.Configuration.ConfigurationManager.AppSettings["ApiUri"];

        private static Dictionary<System.Type, String> typeToString = new Dictionary<Type, string>()
        {
            {typeof(ViewModel.ConfigXml), "ConfigFile" },
        };

        static List<MediaTypeFormatter> formatters = new List<MediaTypeFormatter>()
        {
            new JsonMediaTypeFormatter()
        };

        private static HttpClient SetupClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            HttpClient client = new HttpClient(handler, true);
            client.BaseAddress = new Uri(UriString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));

            return client;
        }

        //private static async Task<CustomException> HandleException(HttpResponseMessage response, string message_default = "Unknown Error")
        //{
        //    CustomException rexp = new CustomException(CustomExceptionTypes.UnknownFailure_Error, message_default);
        //    try
        //    {
        //        rexp = await response.Content.ReadAsAsync<CustomException>(formatters);
        //    }
        //    catch (Exception ex)
        //    {
        //        rexp = new CustomException(CustomExceptionTypes.NonSignalException_Error, message_default, ex);
        //    }

        //    return rexp;
        //}

        private static async Task<Exception> HandleException(HttpResponseMessage response, string message_default = "Unknown Error")
        {
            Exception rexp = new Exception(message_default);
            return rexp;
        }


        public static async Task<IEnumerable<TEntity>> GetAsync()
        {
            using (HttpClient client = SetupClient())
            {
                HttpResponseMessage response = await client.GetAsync((typeToString[typeof(TEntity)]));

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<TEntity>>(formatters);
                }
                else // we should have received an Exception
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }

        }

        public static async Task<IEnumerable<TEntity>> GetAsync(ArgType at, string qstring)
        {

            using (HttpClient client = SetupClient())
            {
                string addstr = "";
                switch (at)
                {
                    case ArgType.TypeExactString:
                        addstr = qstring;
                        break;
                    case ArgType.TypeParamsFromURI:
                        addstr = "?" + qstring;
                        break;
                    case ArgType.TypeID:
                        break;
                }
                Debug.WriteLine("GET " + client.BaseAddress + typeToString[typeof(TEntity)] + addstr);
                HttpResponseMessage response = await client.GetAsync((typeToString[typeof(TEntity)] + addstr));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<TEntity>>(formatters);
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }
        }

        public static async Task<TEntity> GetSingleAsync(ArgType at, string qstring)
        {

            using (HttpClient client = SetupClient())
            {
                string addstr = "";
                switch (at)
                {
                    case ArgType.TypeExactString:
                        addstr = qstring;
                        break;
                    case ArgType.TypeParamsFromURI:
                        addstr = "?" + qstring;
                        break;
                    case ArgType.TypeID:
                        break;
                }
                Debug.WriteLine("GET " + client.BaseAddress + typeToString[typeof(TEntity)] + addstr);
                HttpResponseMessage response = await client.GetAsync((typeToString[typeof(TEntity)] + addstr));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TEntity>(formatters);
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;
                }
            }
        }


        public static async Task<FileHelper> GetFileAsync(ArgType at, string qstring)
        {

            using (HttpClient client = SetupClient())
            {
                string addstr = "";
                switch (at)
                {
                    case ArgType.TypeExactString:
                        addstr = qstring;
                        break;
                    case ArgType.TypeParamsFromURI:
                        addstr = "?" + qstring;
                        break;
                    case ArgType.TypeID:
                        break;
                }
                Debug.WriteLine("GET " + client.BaseAddress + typeToString[typeof(TEntity)] + addstr);
                HttpResponseMessage response = await client.GetAsync((typeToString[typeof(TEntity)] + addstr));
                if (response.IsSuccessStatusCode)
                {
                    FileHelper fh = new FileHelper();
                    fh.ContentType = response.Content.Headers.ContentDisposition.ToString();
                    fh.FileName = response.Content.Headers.ContentDisposition.FileName;
                    fh.FileBytes = await response.Content.ReadAsByteArrayAsync();
                    fh.Status = FileHelper.FileHelperStatus.OK;
                    return fh;
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }
        }

        public static async Task<TEntity> GetAsync(object id)
        {
            using (HttpClient client = SetupClient())
            {
                HttpResponseMessage response = await client.GetAsync((typeToString[typeof(TEntity)] + "/" + id.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TEntity>(formatters);
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }
        }



        public static async Task<TEntity> GetAsync(Uri ObjectUri)
        {
            using (HttpClient client = SetupClient())
            {
                HttpResponseMessage response = await client.GetAsync(ObjectUri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<TEntity>(formatters);
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }
        }



        // put is for updates
        public static async Task<TEntity> PutAsync(TEntity entity)
        {
            using (HttpClient client = SetupClient())
            {
                HttpResponseMessage response = await client.PutAsJsonAsync<TEntity>(typeToString[typeof(TEntity)] + "/", entity);

                if (response.IsSuccessStatusCode)
                {
                    Uri item = response.Headers.Location;
                    TEntity ret = await ClientApi<TEntity>.GetAsync(item);
                    return ret;
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }

            }
        }

        // post is for inserts
        public static async Task<TEntity> PostAsync(TEntity entity)
        {
            using (HttpClient client = SetupClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync<TEntity>(typeToString[typeof(TEntity)], entity);
                if (response.IsSuccessStatusCode)
                {
                    Uri item = response.Headers.Location;
                    TEntity ret = await ClientApi<TEntity>.GetAsync(item);
                    return ret;
                }
                else // we should have received a SignalException
                {
                    Exception sexp = await HandleException(response);
                    //CustomException sexp = await HandleException(response);
                    throw sexp;

                }
            }

        }
    }
}
