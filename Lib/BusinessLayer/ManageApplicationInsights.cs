using System;
using System.Collections.Generic;
using ViewModel;

namespace BusinessLayer
{
    public class ManageApplicationInsights
    {
        public ManageApplicationInsights()
        {

        }

        public ApplicationInsights GetMachineData(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicationInsights UpdateInsight(ApplicationInsights value)
        {
            throw new NotImplementedException();
        }

        public ApplicationInsights CreateInsight(ApplicationInsights value)
        {
            throw new NotImplementedException();
        }

        public ApplicationInsights DeleteInsight(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationInsights> GetAllInsights()
        {
            throw new NotImplementedException();
        }

        public List<ApplicationInsights> GetAllInsights(DateTime startDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        //private static string GetTelemetry(string queryType, string queryPath, string parameterString)
        //{
        //    string url = @"https://management.azure.com/subscriptions/{subscription-id}/resourceGroups/{resource-group-name}/providers/{resource-provider-namespace}/{resource-type}/{resource-name}/metrics?api-version=2014-04-01&$filter={filter}";
        //    string subscriptionId = string.Empty;
        //    string resourceGroupName = string.Empty;
        //    string resourceProviderNamespace = string.Empty;
        //    string resourceType = string.Empty;
        //    string resourceName = string.Empty;
        //    string filter = string.Empty;
        //    string requestUrl = string.Format(url, subscriptionId, resourceGroupName, resourceProviderNamespace, resourceType, resourceName, filter);

        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Add("x-api-key", ApiLib.AzureAdAuth.apiKey);
        //    var req = string.Format(ApiLib.AzureAdAuth.AppInsightsUri, ApiLib.AzureAdAuth.clientId, queryType, queryPath, parameterString);
        //    HttpResponseMessage response = client.GetAsync(req).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //    else
        //    {
        //        return response.ReasonPhrase;
        //    }
        //}
    }
}