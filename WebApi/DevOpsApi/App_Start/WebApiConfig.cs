using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DevOpsApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();
            //EnableCorsAttribute cors = new EnableCorsAttribute("http://localhost:80/DemoApp/WebForm1.aspx", "*", "GET,PUT,POST");
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET,PUT,POST");

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
