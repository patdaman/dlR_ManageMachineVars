using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
//using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLib
{
    public class AzureAdAuth
    {
        //internal static readonly string AppInsightsUri = System.Configuration.ConfigurationManager.AppSettings["AppInsightsApiUri"];
        // private static string classicVmUrl = @"https://management.azure.com/subscriptions/{subscription-id}/resourceGroups/{resource-group-name}/providers/microsoft.classiccompute/virtualmachines/{vm-name}/";
        //internal static readonly string apiKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];
        //private static string clientId = ConfigurationManager.AppSettings["ClientId"];
        public static readonly string AppInsightsUri = System.Configuration.ConfigurationManager.AppSettings["AppInsightsApiUri"];
        public static string classicVmUrl = @"https://management.azure.com/subscriptions/{subscription-id}/resourceGroups/{resource-group-name}/providers/microsoft.classiccompute/virtualmachines/{vm-name}/";
        public static readonly string apiKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];
        public static string clientId = ConfigurationManager.AppSettings["ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["Tenant"];
        private static string devOpsResourceId = ConfigurationManager.AppSettings["DevOpsResourceId"];
        private static string devOpsBaseAddress = ConfigurationManager.AppSettings["DevOpsBaseAddress"];
        private static string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];

        static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        private static AuthenticationContext authContext = new AuthenticationContext(authority);
        private static ClientCredential clientCredential = new ClientCredential(clientId, apiKey);

        private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

        //    app.UseCookieAuthentication(new CookieAuthenticationOptions());

        //    app.UseOpenIdConnectAuthentication(
        //        new OpenIdConnectAuthenticationOptions
        //        {
        //            ClientId = clientId,
        //            Authority = authority,
        //            RedirectUri = redirectUri,
        //            PostLogoutRedirectUri = redirectUri,
        //            Notifications = new OpenIdConnectAuthenticationNotifications
        //            {
        //                AuthenticationFailed = context =>
        //                {
        //                    context.HandleResponse();
        //                    context.Response.Redirect("/Home/Error");
        //                    return Task.FromResult(0);
        //                }
        //            }
        //        });
        public static string AuthorizeAzureAd()
        { 
            AuthenticationResult result = null;
            int retryCount = 0;
            bool retry = false;
            do
            {
                retry = false;
                try
                {
                    // ADAL includes an in memory cache, so this call will only send a message to the server if the cached token is expired.
                    //result = await authContext.AcquireTokenAsync(devOpsResourceId, clientCredential);
                    result = authContext.AcquireTokenAsync(devOpsResourceId, clientCredential).Result;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("temporarily_unavailable"))
                    {
                        retry = true;
                        retryCount++;
                        Thread.Sleep(3000);
                    }
                }
            } while ((retry == true) && (retryCount < 3));

            // Retrieve the user's Object Identifier claim, which is used as the key to the To Do list.
            string ownerId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            return ownerId;
        }

        public static string GetAccessToken()
        {
            var authenticationContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", tenant));
            var result = authenticationContext.AcquireTokenAsync(resource: "https://management.core.windows.net/", clientCredential: clientCredential).Result;
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }
            string token = result.AccessToken;
            return token;
        }
    }
}
