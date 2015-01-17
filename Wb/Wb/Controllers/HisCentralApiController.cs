using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Http;
//using System.Web.Mvc;
//using System.Web.Mvc.Html;

using Wb.hiscentral;

namespace Wb.Controllers
{
    [RoutePrefix("hiscentral")]
    public class HisCentralApiController : ApiController
    {
        // GET: api/HisCentral
        /// <summary>
        /// Get Listing of Searchable Concepts
        /// </summary>
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
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
           
            var respnse = client.GetSearchableConcepts();
            return respnse;
        }

    


        // GET: api/HisCentral/5
        /// <summary>
        /// Get the Ontology Tree for a concept
        /// </summary>
        /// <param name="conceptKeyword">concept Keyword. Try Streamflow</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        /// 
        [HttpGet()]
        [Route("Ontology/{conceptKeyword}")]
        [ActionName("GetSearchableConcepts")]
        [SwaggerDefaultValue("conceptKeyword", "Streamflow")]
        public OntologyNode GetOntologyTree(string conceptKeyword)
        {
            return CallGetOntologyTree(conceptKeyword);
        }

        private OntologyNode CallGetOntologyTree(string conceptKeyword)
        {
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");

            var respnse = client.getOntologyTree(conceptKeyword);
            return respnse;
        }

        // GET: api/HisCentral/5
        /// <summary>
        /// Get Listing of Water One Flow Services in catalog
        /// </summary>
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
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");

            var respnse = client.GetWaterOneFlowServiceInfo();
            return respnse;
        }

        /// <summary>
        /// Get list of services in a bounding box
        /// </summary>
        /// <param name="north">north, try 42</param>
        /// <param name="south">south, try 40</param>
        /// <param name="west">west, try -114</param>
        /// <param name="east">east, try -110</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [Route("Services/box")]
        [ActionName("GetServicesInBox2")]
        [SwaggerDefaultValue("east", "-119")]
        [SwaggerDefaultValue("west", "-114")]
        [SwaggerDefaultValue("north", "42")]
        [SwaggerDefaultValue("south", "40")]
        public IEnumerable<ServiceInfo> GetServicesInBox2(float north, float south, float west, float east)
        {
            return CallGetServicesInBox2(north, south, west, east);
        }

        private IEnumerable<ServiceInfo> CallGetServicesInBox2( float north, float south, float west, float east)
        {
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");

            var respnse = client.GetServicesInBox2(west,south,east,north );
            return respnse;
        }

        /// <summary>
        /// Series Catalog in Box
        /// </summary>
        /// <param name="north">north, try 42</param>
        /// <param name="south">south, try 40</param>
        /// <param name="west">west, try -114</param>
        /// <param name="east">east, try -110</param>
        /// <param name="conceptKeyword">conceptKeyword, try streamflow</param>
        /// <param name="networkIds">networkIDs,(try '52' or '1,2,4') comma separated network identifiers from GetWaterOneFlowServiceInfo or GetServicesInBox</param>
        /// <param name="startTime">starttime  as ISO, try 2012-01-01</param>
        /// <param name="endTime">endtime as ISO, try 2014-01-01</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response> 
        [HttpGet()]
        [Route("Series")]
        [ActionName("GetSeriesCatalogForBox2")]
        [SwaggerDefaultValue("east", "-119")]
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
    DateTime startTime, DateTime endTime
)
        {
            return CallGetSeriesCatalogForBox2(north, south, west, east,conceptKeyword,  networkIds,
 startTime,  endTime);
        }

private IEnumerable<SeriesRecord> CallGetSeriesCatalogForBox2(
    float north, float south, float west, float east,
string conceptKeyword, string networkIds,
DateTime beginDate, DateTime endDate)
        {
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");

            var respnse = client.GetSeriesCatalogForBox2(west,south,east,north,conceptKeyword,
                networkIds, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            return respnse;
        }
    }
}
