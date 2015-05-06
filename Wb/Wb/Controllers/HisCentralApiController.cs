using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
//using System.Web.Http;
using System.Web.Http;
//using System.Web.Mvc;
//using System.Web.Mvc.Html;
using System.Web.Http.Description;
using System.Xml.Serialization;
using Microsoft.Owin;
//using Wb.hiscentral;
using Wb.org.cuahsi.hiscentral;
using hisCentral = Wb.org.cuahsi.hiscentral ;

namespace Wb.Controllers
{
    [RoutePrefix("hiscentral")]

    public class HisCentralApiController : ApiController
    {
        // GET: api/HisCentral
        /// <summary>
        /// Get Listing of Searchable Concepts
        /// </summary>
        /// <remarks>Returns concept keywords that can be subitted in HIS Central serivce calls.. Performance is reliant on the hiscentral.cuahsi.org. </remarks>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [ActionName("GetSearchableConcepts")]
        [Route("Ontology")]
        public IEnumerable<string> GetSearchableConcepts()
        {
            return CallGetSearchableConcepts();
        }

        private IEnumerable<string> CallGetSearchableConcepts()
        {
            // WCF
            //hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
            var client = new hisCentral.hiscentral();
            var respnse = client.GetSearchableConcepts();


            return respnse;
        }




        // GET: api/HisCentral/5
        /// <summary>
        /// Get the Ontology Tree for a concept
        /// </summary>
        /// <param name="conceptKeyword">concept Keyword. Try Streamflow</param>
        /// <returns></returns>
        /// <response code="200">OK</response> 
        /// <response code="404">keywod not found</response> 
        ///  <response code="500">Service Error</response> 
        /// 
        [HttpGet()]
        [Route("Ontology/{conceptKeyword}")]
        [ActionName("GetOntologyTree")]
        [SwaggerDefaultValue("conceptKeyword", "Streamflow")]
        //[ResponseType(typeof (OntologyNode))]
        public IHttpActionResult GetOntologyTree(string conceptKeyword)
        {
            if (String.IsNullOrWhiteSpace(conceptKeyword))
            {
                return BadRequest("conceptKeyword cannot be null or empty");
            }
            else
            {

                try
                {
                    var response = CallGetOntologyTree(conceptKeyword);
                    if (String.IsNullOrWhiteSpace(response.keyword))
                    {
                        return NotFound();
                    }
                    return Ok(response);
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }

        }

        private OntologyNode CallGetOntologyTree(string conceptKeyword)
        {
            // wfc
            //hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
            var client = new hisCentral.hiscentral();

            var respnse = client.getOntologyTree(conceptKeyword);

            return respnse;
        }

        // GET: api/HisCentral/5
        /// <summary>
        /// Get Listing of Water One Flow Services in catalog
        /// </summary>
        /// <remarks>Lists all  WaterOneFlow services. Performance is reliant on the hiscentral.cuahsi.org. </remarks>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [Route("Services")]
        [ActionName("GetWaterOneFlowServiceInfo")]
        public IEnumerable<ServiceInfo> GetWaterOneFlowServiceInfo()
        {
            return CallGetWaterOneFlowServiceInfo();
        }

        private IEnumerable<ServiceInfo> CallGetWaterOneFlowServiceInfo()
        {
            // hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
            var client = new hisCentral.hiscentral();
            var respnse = client.GetWaterOneFlowServiceInfo();
            return respnse;
        }

        /// <summary>
        /// Get list of services in a bounding box
        /// </summary>
        ///  <remarks>Returns WaterOneFlow services for a region. This allows users to harvest information from appropriate data servvices. Performance is reliant on the hiscentral.cuahsi.org. </remarks>
        /// <param name="north">north, try 42</param>
        /// <param name="south">south, try 40</param>
        /// <param name="west">west, try -114</param>
        /// <param name="east">east, try -110</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [Route("Services/box")]
        [ActionName("GetServicesInBox2")]
        [SwaggerDefaultValue("east", "-110")]
        [SwaggerDefaultValue("west", "-114")]
        [SwaggerDefaultValue("north", "42")]
        [SwaggerDefaultValue("south", "40")]
        public IEnumerable<ServiceInfo> GetServicesInBox2(float north, float south, float west, float east)
        {
            return CallGetServicesInBox2(north, south, west, east);
        }

        private IEnumerable<ServiceInfo> CallGetServicesInBox2(float north, float south, float west, float east)
        {
            // hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
            var client = new hisCentral.hiscentral();

            var respnse = client.GetServicesInBox2(west, south, east, north);
            return respnse;
        }

        /// <summary>
        /// Series Catalog in Box
        /// </summary>
        /// <remarks>This call is slow. It performance is reliant on the hiscentral.cuahsi.org services it consumes.</remarks>
        /// <param name="north">north, try 42</param>
        /// <param name="south">south, try 40</param>
        /// <param name="west">west, try -114</param>
        /// <param name="east">east, try -110</param>
        /// <param name="conceptKeyword">conceptKeyword, try streamflow</param>
        /// <param name="networkIds">networkIDs,(try '52' or '1,2,4') comma separated network identifiers from GetWaterOneFlowServiceInfo or GetServicesInBox</param>
        /// <param name="startTime">starttime  as ISO, try 2012-01-01</param>
        /// <param name="endTime">endtime as ISO, try 2014-01-01</param>
        /// <param name="format">format, CSV format: 'csv', JSON format: 'json', XML format: 'xml' </param>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [Route("Series")]
        [ActionName("Series")]
        [SwaggerDefaultValue("east", "-110")]
        [SwaggerDefaultValue("west", "-114")]
        [SwaggerDefaultValue("north", "42")]
        [SwaggerDefaultValue("south", "40")]
        [SwaggerDefaultValue("conceptKeyword", "Temperature")]
        [SwaggerDefaultValue("networkIds", "52")]
        [SwaggerDefaultValue("startTime", "2012-01-01")]
        [SwaggerDefaultValue("endTime", "2012-06-01")]

        public IEnumerable<SeriesRecord> GetWaterOneFlowServiceInfo(
            float north, float south, float west, float east,
            string conceptKeyword, string networkIds,
            DateTime startTime, DateTime endTime, string format = null
            )
        {
            return CallGetSeriesCatalogForBox2(north, south, west, east, conceptKeyword, networkIds,
                startTime, endTime);
        }

        //WCF ASMX call
//private IEnumerable<SeriesRecord> CallGetSeriesCatalogForBox2(
//    float north, float south, float west, float east,
//string conceptKeyword, string networkIds,
//DateTime beginDate, DateTime endDate)
//        {
//            //hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
//            var client = new hisCentral.hiscentral();

//            var respnse = client.GetSeriesCatalogForBox2(west,south,east,north,conceptKeyword,
//                networkIds, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
//            return respnse;
//        }

        // rewite to use rest
        private IEnumerable<SeriesRecord> CallGetSeriesCatalogForBox2(
            float north, float south, float west, float east,
            string conceptKeyword, string networkIds,
            DateTime beginDate, DateTime endDate)
        {
            //hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
            // var client = new hisCentral.hiscentral();
            //GET /webservices/hiscentral.asmx/GetSitesInBox2?xmin=string&xmax=string&ymin=string&ymax=string&conceptKeyword=string&networkIDs=string HTTP/1.1

            UriBuilder u =
                new UriBuilder("http://hiscentral.cuahsi.org/webservices/hiscentral.asmx/GetSeriesCatalogForBox2?");

            string appendstring = "{0}={1}&";

            u.Query = string.Format(appendstring, "xmin", west);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "xmax", east);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymin", south);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymax", north);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "conceptKeyword", conceptKeyword);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "networkIds", networkIds);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "beginDate", beginDate.ToString("yyyy-MM-dd"));
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "endDate", endDate.ToString("yyyy-MM-dd"));

            WebRequest request = WebRequest.Create(u.Uri);
            request.Method = "GET";

            var xmlSerializer = new XmlSerializer(typeof(ArrayOfSeriesRecord));
            var res = request.GetResponse();

            var stream = res.GetResponseStream();

            var respnse = (ArrayOfSeriesRecord)xmlSerializer.Deserialize(stream);
            //var respnse = client.GetSeriesCatalogForBox2(west, south, east, north, conceptKeyword,
            //    networkIds, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

            return respnse.seriesResponse;
        }

        [XmlRoot("ArrayOfSeriesRecord", Namespace = "http://hiscentral.cuahsi.org/20100205/")]
       // [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://hiscentral.cuahsi.org/20100205/")]
        public class ArrayOfSeriesRecord
        {
            [XmlElement("SeriesRecord")]
            public SeriesRecord[] seriesResponse { get; set; }
        }
       
      
        
    }
}
