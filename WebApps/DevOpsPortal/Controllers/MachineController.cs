using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevOpsPortal.Controllers
{
    public class MachineController : BaseController
    {
#if RELEASE
        [Authorize(Roles = "Engineers")]
#endif
        private const string URL = @"https://api.applicationinsights.io/beta/apps/{0}/{1}/{2}?{3}";
        private static string classicVmUrl = @"https://management.azure.com/subscriptions/{subscription-id}/resourceGroups/{resource-group-name}/providers/microsoft.classiccompute/virtualmachines/{vm-name}/";

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> GetAppMetrics()
        {
            IEnumerable<ViewModel.ApplicationInsights> appMetrics = null;
            try
            {
                appMetrics = await ApiLib.ClientApi<ViewModel.ApplicationInsights>.GetAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(appMetrics.ToList<ViewModel.ApplicationInsights>());
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<JsonResult> GetMachineTelemetry(int id)
        {
            ViewModel.AppVar apiAppVar = null;
            try
            {
                apiAppVar = await ApiLib.ClientApi<ViewModel.AppVar>.GetAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(apiAppVar);
        }

        private static HttpClient SetupClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            HttpClient client = new HttpClient(handler, true);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
            client.DefaultRequestHeaders.Add("x-api-key", @"b3v3jkhwky2xu0a28pjmm7nplqg6lsmpowqib2iy");
            client.BaseAddress = new Uri(@"https://api.applicationinsights.io");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiLib.AzureAdAuth.GetAccessToken()); //.AccessToken);
            return client;
        }

        private static string GetTelemetry(string queryType, string queryPath, string parameterString)
        {
            string url = @"https://management.azure.com/subscriptions/{subscription-id}/resourceGroups/{resource-group-name}/providers/{resource-provider-namespace}/{resource-type}/{resource-name}/metrics?api-version=2014-04-01&$filter={filter}";
            string subscriptionId = string.Empty;
            string resourceGroupName = string.Empty;
            string resourceProviderNamespace = string.Empty;
            string resourceType = string.Empty;
            string resourceName = string.Empty;
            string filter = string.Empty;
            string requestUrl = string.Format(url, subscriptionId, resourceGroupName, resourceProviderNamespace, resourceType, resourceName, filter);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", ApiLib.AzureAdAuth.apiKey);
            var req = string.Format(ApiLib.AzureAdAuth.AppInsightsUri, ApiLib.AzureAdAuth.clientId, queryType, queryPath, parameterString);
            HttpResponseMessage response = client.GetAsync(req).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }
    }
}