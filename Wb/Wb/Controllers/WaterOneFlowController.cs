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
        private const string BaseWOfURL = "http://icewater.usu.edu/littlebearriver/cuahsi_1_1.asmx";

        private WaterOneFlowClient getClientForURL(
            string url)
        {

            wof_1_1.WaterOneFlowClient client = new WaterOneFlowClient("WaterOneFlow",
               BaseWOfURL);
            return client;
        }

        /// <summary>
        /// Get Site Listing
        /// </summary>
        /// <remarks>Returns site listing for a WaterOneFlow service. Performance is reliant on the WaterOneFlow hydrolocic data service. 
        /// The CSV format is a summary of the site detailed information in the XML format</remarks>
        /// <param name="station">site identifier, {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("sites")]
        [Route("sites")]
        [SwaggerDefaultValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", "http://www.example.com/future")]
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
        /// <remarks>Returns site listing for a WaterOneFlow service. Performance is reliant on the WaterOneFlow hydrolocic data service.
        ///  The CSV format is a summary of the detailed information in the XML format</remarks>
        /// <param name="station">site identifier, {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("siteinfo")]
        [Route("siteinfo")]
        [SwaggerDefaultValue("station", "LBR:USU-LBR-Mendon")]
        [SwaggerDefaultValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", "http://www.example.com/future")]
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
        /// <remarks>Returns a listing of all varaibles for  WaterOneFlow service. Performance is reliant on the WaterOneFlow hydrolocic data service. 
        /// </remarks>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("observedProperty")]
        [Route("observedProperty")]
        [SwaggerDefaultValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", "http://www.example.com/future")]
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
        /// <remarks>Returns a single variable in a WaterOneFlow service. Performance is reliant on the WaterOneFlow hydrolocic data service.
        ///  </remarks>
        /// <param name="variable">variable  {network:identifier}</param>
        /// <param name="servUrl">WOF1.1 Service Enpoint</param>
        /// <returns></returns>
        /// <response code="400">Invaild or site not found</response>
        /// <response code="500">Service Error</response>
        [HttpGet()]
        [ActionName("info")]
    
        [Route("observedProperty/info")]
        [SwaggerDefaultValue("variable", "LBR:USU10")]
        [SwaggerDefaultValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", "http://www.example.com/future")]
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
        /// <remarks>Returns time series data values from WaterOneFlow service for a specified series. Performance is reliant on the WaterOneFlow hydrolocic data service.
        ///  The CSV format (TBD) is a summary of the detailed information in the XML format</remarks>
        
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
        [SwaggerDefaultValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", BaseWOfURL)]
        [SwaggerEnumValue("servUrl", "http://www.example.com/future")]
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
