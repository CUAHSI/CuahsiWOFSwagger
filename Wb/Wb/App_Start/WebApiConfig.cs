using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Wb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //http://stackoverflow.com/questions/12617471/adjust-mvc-4-webapi-xmlserializer-to-lose-the-namespace
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            // Web API configuration and services

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
