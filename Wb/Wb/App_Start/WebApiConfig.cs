using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
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
            config.Formatters.XmlFormatter.AddQueryStringMapping("format","xml","text/xml");
            config.Formatters.JsonFormatter.AddQueryStringMapping("format", "json", "text/json");
           // config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml","text/xml");
           // config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json","text/json");
            // http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
           // json.UseDataContractJsonSerializer = true;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
           json.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
             
            //json.SerializerSettings.Culture = new CultureInfo("it-IT");
            var seriesf = new SeriesRecordCsvFormatter();
            seriesf.MediaTypeMappings.Add(new QueryStringMapping("format", "csv", "text/csv"));
            // seriesf.MediaTypeMappings.Add(new UriPathExtensionMapping("csv", "text/csv"));
            config.Formatters.Add(seriesf);
            var wof11Site = new Wof11_SiteInfoCsvFormatter();
            wof11Site.MediaTypeMappings.Add(new QueryStringMapping("format", "csv", "text/csv"));
            //wof11Site.MediaTypeMappings.Add(new UriPathExtensionMapping("csv", "text/csv"));
            config.Formatters.Add(wof11Site);

            var wof11Values = new Wof11_ValuesCsvFormatter();
            wof11Values.MediaTypeMappings.Add(new QueryStringMapping("format", "csv", "text/csv"));
            //wof11Site.MediaTypeMappings.Add(new UriPathExtensionMapping("csv", "text/csv"));
            config.Formatters.Add(wof11Values);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
name: "Url extension",
routeTemplate: "api/{controller}/{action}.{ext}/{id}",
defaults: new { id = RouteParameter.Optional }
); 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
