using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using System.Threading.Tasks;
using System.Threading;

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
            var client = new hiscentralSoapClient("hiscentralSoap");
            //var client = new hisCentral.hiscentral();
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
        [ActionName("GetSearchableConcepts")]
        [SwaggerDefaultValue("conceptKeyword", "Streamflow")]
        [ResponseType(typeof(OntologyNode))]
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
            var client = new hiscentralSoapClient("hiscentralSoap");
            //var client = new hisCentral.hiscentral();

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
            var client = new hiscentralSoapClient("hiscentralSoap");
            // var client = new hisCentral.hiscentral();
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
            var client = new hiscentralSoapClient("hiscentralSoap");
            // var client = new hisCentral.hiscentral();

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
        /// <param name="startTime">starttime  as ISO, try 2012-01-01</param>
        /// <param name="endTime">endtime as ISO, try 2014-01-01</param>
        /// <param name="networkIds">networkIDs,(Empty=all, try '52' or '2,4356') comma separated network identifiers from GetWaterOneFlowServiceInfo or GetServicesInBox</param>
        /// <param name="conceptKeyword">conceptKeyword, try streamflow. Empty = All</param>
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
        //original
        //public IEnumerable<SeriesRecord> GetWaterOneFlowServiceInfo(float north, float south, float west, float east, DateTime startTime, DateTime endTime, string networkIds="", string conceptKeyword = "", string format = null)
        //{
        //    return CallGetSeriesCatalogForBox2(north, south, west, east, networkIds,
        //        startTime, endTime, conceptKeyword);
        //}
        public IEnumerable<SeriesRecord> GetWaterOneFlowServiceInfo(float north, float south, float west, float east, DateTime? startTime, DateTime? endTime, string networkIds = "", string conceptKeyword = "", string format = null)
        {
            ConcurrentBag<SeriesRecord> seriesResponse = new ConcurrentBag<SeriesRecord>();

            CallGetSeriesCatalogForBox3(north, south, west, east, networkIds,
                 startTime, endTime, conceptKeyword, seriesResponse);
             return seriesResponse;
        }

        private async void CallGetSeriesCatalogForBox2(float north, float south, float west, float east, string networkIds, DateTime? beginDate, DateTime? endDate, string conceptKeyword, ConcurrentBag<SeriesRecord> result)
        {
            ConcurrentBag<SeriesRecord> seriesResponse = new ConcurrentBag<SeriesRecord>();
            var client = new hiscentralSoapClient("hiscentralSoap");
            //var client = new hisCentral.hiscentral();
            var begin = beginDate.HasValue ? beginDate.Value.ToString("yyyy-MM-dd") : "";
            var end = endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : "";

            var t = await client.GetSeriesCatalogForBox2Async(north, south, west, east, conceptKeyword,
                networkIds, begin, end);
            ProcessSeries(t,
                 seriesResponse);


        }

        private async void CallGetSeriesCatalogForBox3(float north, float south, float west, float east, string networkIds, DateTime? beginDate, DateTime? endDate, string conceptKeyword, ConcurrentBag<SeriesRecord> result)
         {
            
            //var client = new hiscentralSoapClient("hiscentralSoap");
            //var client = new hisCentral.hiscentral();
            var begin = beginDate.HasValue ? beginDate.Value.ToString("yyyy-MM-dd") : "";
            var end = endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : "";

            var requests = inputToRequests(north, south, west, east,
 networkIds, beginDate, endDate, conceptKeyword);
            await GetSeries3(requests, result);
            //var t = await processSeriesRecords(north, south,  west,  east,
            //    networkIds, begin, end, conceptKeyword);
            //ProcessSeries3(t,
            //     result);


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
        //private IEnumerable<SeriesRecord> CallGetSeriesCatalogForBox2(float north, float south, float west, float east, string networkIds, DateTime beginDate, DateTime endDate, string conceptKeyword)
        //{
        //    //hiscentral.hiscentralSoapClient client = new hiscentralSoapClient("hiscentralSoap");
        //    // var client = new hisCentral.hiscentral();
        //    //GET /webservices/hiscentral.asmx/GetSitesInBox2?xmin=string&xmax=string&ymin=string&ymax=string&conceptKeyword=string&networkIDs=string HTTP/1.1

        //    UriBuilder u =
        //        new UriBuilder("http://hiscentral.cuahsi.org/webservices/hiscentral.asmx/GetSeriesCatalogForBox2?");

        //    string appendstring = "{0}={1}&";

        //    u.Query = string.Format(appendstring, "xmin", west);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "xmax", east);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymin", south);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymax", north);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "conceptKeyword", conceptKeyword);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "networkIds", networkIds);
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "beginDate", beginDate.ToString("yyyy-MM-dd"));
        //    u.Query = u.Query.Substring(1) + string.Format(appendstring, "endDate", endDate.ToString("yyyy-MM-dd"));

        //    WebRequest request = WebRequest.Create(u.Uri);
        //    request.Method = "GET";

        //    var xmlSerializer = new XmlSerializer(typeof(ArrayOfSeriesRecord));
        //    var res = request.GetResponse();

        //    var stream = res.GetResponseStream();

        //    var respnse = (ArrayOfSeriesRecord)xmlSerializer.Deserialize(stream);
        //    //var respnse = client.GetSeriesCatalogForBox2(west, south, east, north, conceptKeyword,
        //    //    networkIds, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));



        //    return respnse.seriesResponse;
        //}

        [XmlRoot("ArrayOfSeriesRecord", Namespace = "http://hiscentral.cuahsi.org/20100205/")]
        // [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://hiscentral.cuahsi.org/20100205/")]
        public class ArrayOfSeriesRecord
        {
            [XmlElement("SeriesRecord")]
            public SeriesRecord[] seriesResponse { get; set; }
        }
        private List<HisCentralRequest> inputToRequests(float north, float south, float west, float east,
            string networkIds, DateTime? beginDate, DateTime? endDate, string conceptKeyword)
        {
            List<HisCentralRequest> requests = new List<HisCentralRequest>();
            List<int> networks = new List<int>() ;
            List<string> concepts = new List<string>();
            
            DateTimeOffset begin;
            DateTimeOffset end;
            if (!string.IsNullOrEmpty(networkIds))
            {
                var networkStrings = networkIds.Split(',');
                foreach (var n in networkStrings)
                {
                    int nn;
                    if (int.TryParse(n, out nn)) { networks.Add(nn); }
                }
            }
            //var beginValid = DateTimeOffset.TryParse(beginDate, out begin);
            //var endValid = DateTimeOffset.TryParse(endDate, out end);
            if (!string.IsNullOrEmpty(networkIds))
            {
                concepts = conceptKeyword.Split(',').ToList<string>();
            }
            var boundingBoxes = createBoxes(new Bbox { North = north, South = south, West = west, East = east });
            foreach (var box in boundingBoxes)
            {
                var localList = new List<HisCentralRequest>();
                var hisRequest = new HisCentralRequest { Box = box };
                localList.Add(hisRequest);
                localList = addConcepts(localList, concepts);
                localList = addNetworks(localList, networks);
                localList = addDateTime(localList, beginDate, endDate);

                requests.AddRange(localList);
    
            }
            return requests;
        }
        private List<HisCentralRequest> addDateTime(List<HisCentralRequest> hisRequests, DateTime? beginDate, DateTime? endDate)
        {
            hisRequests.ForEach(h => h.Begin = beginDate);
            hisRequests.ForEach(h => h.End = endDate);
            return hisRequests;
        }
        private List<HisCentralRequest> addConcepts(List<HisCentralRequest> hisRequests, List<string> concepts)
        {
            if (hisRequests == null) return null;

            if (concepts == null || concepts.Count == 0)
            {
                return hisRequests;
            }
            
            var newRequests = new List<HisCentralRequest>();

            foreach (var hr in hisRequests)
            {

                foreach (var c in concepts)
                {
                    var x = hr.ShallowCopy();
                    x.Concept = c;
                    newRequests.Add(x);
                }
            }
            return newRequests;

        }

        private List<HisCentralRequest> addNetworks(List<HisCentralRequest> hisRequests, List<int> networks)
        {
            if (hisRequests == null) return null;

            if (networks == null || networks.Count == 0)
            {
                return hisRequests;
            }

            var newRequests = new List<HisCentralRequest>();

            foreach (var hr in hisRequests)
            {

                foreach (var c in networks)
                {
                    var x = hr.ShallowCopy();
                    x.Network = c;
                    newRequests.Add(x);
                }
            }
            return newRequests;

        }
        

        private List<Bbox> createBoxes(Bbox fullExtent)
        {
            var tileWidth = 1.0;
            var tileHeight = 1.0; 
            var tiles = new List<Bbox>();
            double fullWidth = Math.Abs(fullExtent.North - fullExtent.South);
            double fullHeight = Math.Abs(fullExtent.East - fullExtent.West);

            if (fullWidth < tileWidth || fullHeight < tileHeight)
            {
                tiles.Add(fullExtent);
                return tiles;
            }

            double yll = fullExtent.South; //y-coordinate of the tile's lower left corner
            var numColumns = (int)(Math.Ceiling(fullWidth / tileWidth));
            var numRows = (int)(Math.Ceiling(fullHeight / tileHeight));
            var lastTileWidth = fullWidth - ((numColumns - 1) * tileWidth);
            var lastTileHeight = fullHeight - ((numRows - 1) * tileHeight);
            int r;

            for (r = 0; r < numRows; r++)
            {
                double xll = fullExtent.West; //x-coordinate of the tile's lower left corner

                if (r == numRows - 1)
                {
                    tileHeight = lastTileHeight;
                }

                int c;
                for (c = 0; c < numColumns; c++)
                {
                    var newTile = c == (numColumns - 1) ? new Bbox{West=xll,South=yll,East=xll + lastTileWidth, North=yll + tileHeight} :
                                                          new Bbox{West=xll,South=yll,East= xll + tileWidth,North=yll + tileHeight };
                    tiles.Add(newTile);
                    xll = xll + tileWidth;
                }
                yll = yll + tileHeight;
            }
            return tiles;

        }


        private class Bbox
        {
            public double East;
            public double West;
            public double North;
            public double South;

            public Bbox()
            { }

            public Bbox (Bbox box)
            {
                this.East = box.East;
                this.West = box.West;
                this.North = box.North;
                this.South = box.South;
                
            }

        }
        private class HisCentralRequest
        {
            public Bbox Box;
            public string Concept;
            public Int32? Network;
            public DateTimeOffset? Begin;
            public  DateTimeOffset? End;

            public HisCentralRequest ShallowCopy()
            {
                return (HisCentralRequest)this.MemberwiseClone();
            }

            public HisCentralRequest DeepCopy()
            {
                HisCentralRequest other = (HisCentralRequest)this.MemberwiseClone();
                other.Box = new Bbox(Box);

                return other;
            }
        }





        private async Task<IEnumerable<SeriesRecord>> processSeriesRecords(double north, double south, double west, double east,
            string networkIds, string beginDate, string endDate, string conceptKeyword)
        {

            UriBuilder u =
                new UriBuilder("http://hiscentral.cuahsi.org/webservices/hiscentral.asmx/GetSeriesCatalogForBox2?");

            string appendstring = "{0}={1}&";

            u.Query = string.Format(appendstring, "xmin", west);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "xmax", east);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymin", south);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "ymax", north);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "conceptKeyword", conceptKeyword);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "networkIds", networkIds);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "beginDate", beginDate);
            u.Query = u.Query.Substring(1) + string.Format(appendstring, "endDate", endDate);

            WebRequest request = WebRequest.Create(u.Uri);
            request.Method = "GET";

            var xmlSerializer = new XmlSerializer(typeof(ArrayOfSeriesRecord));
            var res = request.GetResponse();

            var stream = res.GetResponseStream();

            var respnse = (ArrayOfSeriesRecord)xmlSerializer.Deserialize(stream);
            return respnse.seriesResponse;

        }

        private async Task GetSeries3(List<HisCentralRequest> urlList, ConcurrentBag<SeriesRecord> series)
        {
            //ConcurrentBag<SeriesRecord> series = new ConcurrentBag<SeriesRecord>();

            List<Task<IEnumerable<SeriesRecord>>> tasks = new List<Task<IEnumerable<SeriesRecord>>> ();
           foreach ( var  hir in urlList) {
               //tasks.Add( processSeriesRecords(u) );
               tasks.Add(processSeriesRecords(hir.Box.North, hir.Box.South, hir.Box.West, hir.Box.East,
             hir.Network.ToString(),
             hir.Begin.HasValue ? hir.Begin.Value.ToString("yyyy-MM-dd") : "",
             hir.End.HasValue ? hir.End.Value.ToString("yyyy-MM-dd") : "", 
             hir.Concept));
           }
           
            //foreach (var t in tasks){
               
            //   try { ProcessSeries3(await t, series); }
            //   catch (OperationCanceledException) { }
            //   //catch (Exception exc) { Handle(exc); }
            //  };

           foreach (var bucket in AsyncTask.Interleaved(tasks))
           {
               var t = await bucket;
               try { ProcessSeries3(await t, series); }
               catch (OperationCanceledException) { }
               //catch (Exception exc) { Handle(exc); }
           }
        }

        private void ProcessSeries(GetSeriesCatalogForBox2Response t, ConcurrentBag<SeriesRecord> series)
        {

            var s = t.GetSeriesCatalogForBox2Result;
            series.Union(s);

        }
        private void ProcessSeries3(IEnumerable<SeriesRecord> t, ConcurrentBag<SeriesRecord> series)
        {

            if (t != null) { 
               // does not work for some reason
                //http://stackoverflow.com/questions/10177768/concurrentbag-add-multiple-items
              //series.Concat<SeriesRecord>(t.AsEnumerable<SeriesRecord>()); 
                t.AsParallel().ForAll(i => series.Add(i));
                
                //foreach (var item in t)
                //{
                //    series.Add(item);
                //}
            }
            

        }
        //private void Process(Task<ArrayOfSeriesRecord> t, IEnumerable<SeriesRecord> series){
        //    ArrayOfSeriesRecord aa = (ArrayOfSeriesRecord) t.Result;
        //    series.Union(t.Result.seriesResponse);

        //}





        public static class AsyncTask
        {
            /* Async tasks. We need to get 1 degree tiles for best performance
             * from http://blogs.msdn.com/b/pfxteam/archive/2012/08/02/processing-tasks-as-they-complete.aspx
             * */


            public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
            {
                var inputTasks = tasks.ToList();

                var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
                var results = new Task<Task<T>>[buckets.Length];
                for (int i = 0; i < buckets.Length; i++)
                {
                    buckets[i] = new TaskCompletionSource<Task<T>>();
                    results[i] = buckets[i].Task;
                }

                int nextTaskIndex = -1;
                Action<Task<T>> continuation = completed =>
                {
                    var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                    bucket.TrySetResult(completed);
                };

                foreach (var inputTask in inputTasks)
                    inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

                return results;
            }
        }

    }
}
