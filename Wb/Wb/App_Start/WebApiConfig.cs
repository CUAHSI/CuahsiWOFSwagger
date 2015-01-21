using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Wb.Models.formatters;

namespace Wb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //http://stackoverflow.com/questions/12617471/adjust-mvc-4-webapi-xmlserializer-to-lose-the-namespace
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            // http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.UseDataContractJsonSerializer = true;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            json.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          //  json.SerializerSettings.Culture = new CultureInfo("it-IT");

            config.Formatters.Add(new SeriesRecordCsvFormatter());
            config.Formatters.Add(new Wof11_SiteInfoCsvFormatter());
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
