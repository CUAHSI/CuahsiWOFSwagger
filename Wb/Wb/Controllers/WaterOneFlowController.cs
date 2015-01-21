using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
//using System.Web.Mvc;
using System.Web.Http.Description;
using Microsoft.Owin.Security.Provider;
using Swashbuckle.Swagger;
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

        /// <summary>
        /// Get Site Listing
        /// </summary>
        /// <param name="station">site identifier, {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("GetSites")]
        [Route("sites")]
        [SwaggerDefaultValue("servUrl", "http://water.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx")]
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


        
        /// <summary>
        /// Site Detailed Site Information
        /// </summary>
        /// <param name="station">site identifier, {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("FeatureOfInterest")]
        [Route("siteinfo")]
        [SwaggerDefaultValue("station", "LBR:USU-LBR-Mendon")]
        [SwaggerDefaultValue("servUrl", "http://water.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx")]
        [ResponseType(typeof(IEnumerable<SiteInfoResponseTypeSite>))]
        public IHttpActionResult GetSiteInfo([FromUri] string[] station, [FromUri] String servUrl = null)
        {
            if (station.Length == 0)
            {
                return BadRequest("Must submit a site code");

            }
            foreach  (var s in station)
            {
                if (String.IsNullOrWhiteSpace(s))
                
                {
                    station = station.Where(val => val != s).ToArray();
                }
            }
            if (station.Length == 0)
            {
                return BadRequest("No valid station codes. All empty.");

            }
            try
            {
                var res = CallGetSiteInfo(station);
                if (res.Any())
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest("No Sites returned");
                }
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Object reference not set to an instance of an object."))
                {
                    return BadRequest("No Sites Found");  
                }
                return InternalServerError();

            }
            
        }
        private IEnumerable<SiteInfoResponseTypeSite> CallGetSiteInfo(string[] sites)
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var respnse = client.GetSiteInfoMultpleObject(sites, null);
            return respnse.site;
        }

        /// <summary>
        /// Get Variables
        /// </summary>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("GetVariables")]
        [Route("observedProperty")]
        [SwaggerDefaultValue("servUrl", "http://water.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx")]
        public IEnumerable<VariableInfoType> GetAllVariables([FromUri] String servUrl = null)
        {
            return CallGetVariables();
        }
        private IEnumerable<VariableInfoType> CallGetVariables()
        {
            wof_1_1.WaterOneFlowClient client = getClientForURL(null);

            var respnse = client.GetVariablesObject( null);
            return respnse.variables;
        }

        /// <summary>
        /// Describe a variable
        /// </summary>
        /// <param name="variable">variable  {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("GetVariables")]
        [Route("observedProperty/info")]
        [SwaggerDefaultValue("variable", "LBR:USU10")]
        [SwaggerDefaultValue("servUrl", "http://water.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx")]
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

/// <summary>
/// Get Timeseries data 
/// </summary>
        /// <param name="station">site  {network:identifier}</param>
        /// <param name="variable">variable  {network:identifier}</param>
        /// <param name="startTime">start date time</param>
        /// <param name="endTime">end date time</param>
/// <param name="servUrl"></param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("GetValues")]
        [Route("values")]
        [SwaggerDefaultValue("station", "LBR:USU-LBR-Mendon")]
        [SwaggerDefaultValue("variable", "LBR:USU10")]
        [SwaggerDefaultValue("startTime", "2010-01-01")]
        [SwaggerDefaultValue("endTime", "2010-02-01")]
        [SwaggerDefaultValue("servUrl", "http://water.sdsc.edu/lbrsdsc/cuahsi_1_1.asmx")]
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
