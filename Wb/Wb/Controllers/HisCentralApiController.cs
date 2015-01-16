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

[HttpGet()]
        [Route("Series")]
        [ActionName("GetSeriesCatalogForBox2")]
        [SwaggerDefaultValue("east", "-119")]
        [SwaggerDefaultValue("west", "-114")]
        [SwaggerDefaultValue("north", "42")]
        [SwaggerDefaultValue("south", "40")]
        [SwaggerDefaultValue("conceptKeyword", "Temperature")]
        [SwaggerDefaultValue("networkIds", "52")]
        [SwaggerDefaultValue("beginDate", "2012-01-01")]
        [SwaggerDefaultValue("endDate", "2014-01-01")]

        public IEnumerable<SeriesRecord> GetWaterOneFlowServiceInfo(
    float north, float south, float west, float east, 
 	string conceptKeyword, string networkIds,
    DateTime beginDate, DateTime endDate
)
        {
            return CallGetSeriesCatalogForBox2(north, south, west, east,conceptKeyword,  networkIds,
 beginDate,  endDate);
        }

private IEnumerable<SeriesRecord> CallGetSeriesCatalogForBox2(
    float north, float south, float west, float east,
string conceptKeyword, string networkIds,
DateTime beginDate, DateTime endDate)
        {
            hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");

            var respnse = client.GetSeriesCatalogForBox2(west,south,east,north,conceptKeyword,
                networkIds, beginDate.ToString(), endDate.ToString());
            return respnse;
        }
    }
}
