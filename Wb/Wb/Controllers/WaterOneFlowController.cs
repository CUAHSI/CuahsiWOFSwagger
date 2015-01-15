using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
//using System.Web.Mvc;
using Microsoft.Owin.Security.Provider;
using Wb.wof_1_1;

namespace Wb.Controllers
{
    [System.Web.Http.RoutePrefix("wateroneflow")]
    public class WaterOneFlowController : ApiController
    {
        private WaterOneFlowClient getClientForURL(
            string url)
        {

            wof_1_1.WaterOneFlowClient client = new WaterOneFlowClient("WaterOneFlow",
                "http://hydro10.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx");
            return client;
        }

        [HttpGet()]
        [ActionName("GetSites")]
        [Route("sites")]

        public IEnumerable<SiteInfoResponseTypeSite> GetSites([FromUri] string[] station =null, [FromUri] String servUrl = null)
        {
            return CallGetSites(station);
        }
        private IEnumerable<SiteInfoResponseTypeSite> CallGetSites(string[] sitees)
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);
          

            var respnse = client.GetSitesObject(new string[] {}, null);
            return respnse.site;
        }

        //[HttpGet()]
        //[ActionName("FeatureOfInterest")]
        //[Route("siteinfo")]

        //public IEnumerable<SiteInfoResponseTypeSite> GetSiteInfo([FromUri]string[] sites)
        //{
        //    return CallGetSiteInfo(sites);
        //}
        //private IEnumerable<SiteInfoResponseTypeSite> CallGetSiteInfo(string[] sitees)
        //{
        //    wof_1_1.WaterOneFlowClient client = new WaterOneFlowClient("WaterOneFlow");

        //    var respnse = client.GetSiteInfoMultpleObject(sitees,null);
        //    return respnse.site;
        //}
        [HttpGet()]
        [ActionName("FeatureOfInterest")]
        [Route("siteinfo")]

        public IEnumerable<SiteInfoResponseTypeSite> GetSiteInfo([FromUri] string[] station, [FromUri] String servUrl = null)
        {
            return CallGetSiteInfo(station);
        }
        private IEnumerable<SiteInfoResponseTypeSite> CallGetSiteInfo(string[] sites)
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var respnse = client.GetSiteInfoMultpleObject(sites, null);
            return respnse.site;
        }
        [HttpGet()]
        [ActionName("GetVariables")]
        [Route("observedProperty")]

        public IEnumerable<VariableInfoType> GetVariables([FromUri] String servUrl = null)
        {
            return CallGetVariables();
        }
        private IEnumerable<VariableInfoType> CallGetVariables()
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var respnse = client.GetVariablesObject( null);
            return respnse.variables;
        }

        [HttpGet()]
        [ActionName("GetVariables")]
        [Route("observedProperty/")]

        public VariableInfoType GetVariables([FromUri]string variable, [FromUri] String servUrl = null)
        {
            return CallGetVariableInfo(variable);
        }
        private VariableInfoType CallGetVariableInfo(string variable)
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var respnse = client.GetVariableInfoObject(variable,null);
            return respnse.variables.First();
        }


        [HttpGet()]
        [ActionName("GetValues")]
        [Route("values")]

        public TimeSeriesType GetValues([FromUri]string station, [FromUri]string variable,
            DateTime? startTime = null, DateTime? endTime = null, 
            [FromUri] String servUrl = null)
        {
            return CallGetValues(station,variable, startTime,endTime,null);
        }
        private TimeSeriesType CallGetValues([FromUri]string station ,[FromUri]string variable,
            DateTime? startTime = null, DateTime? endTime = null, 
            [FromUri] String servUrl = null)
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var begin = startTime.HasValue ? startTime.Value.ToString("yyyy-MM-dd") : null;
            var end = endTime.HasValue ? endTime.Value.ToString("yyyy-MM-dd") : null;
            var respnse = client.GetValuesObject(station, variable, begin, end, null);
            return respnse.timeSeries.First();
        }
    }

}
