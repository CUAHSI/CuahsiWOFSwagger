using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Profile;
using Wb.hiscentral;
using Wb.wof_1_1;
using WebGrease;

namespace Wb.Models
{
    // set of classes to write out CSV files

   

    namespace formatters
    {
        public class Wof11_ValuesCsvFormatter : BufferedMediaTypeFormatter
        {
            public Wof11_ValuesCsvFormatter()
            {
                // Add the supported media type.
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
            }


            public override bool CanWriteType(System.Type type)
            {
                if (type == typeof(TimeSeriesType))
                {
                    return true;
                }
                else
                {
                    Type enumerableType = typeof(IEnumerable<TimeSeriesType>);
                    return enumerableType.IsAssignableFrom(type);
                }
            }
            public override bool CanReadType(Type type)
            {
                return false;
            }

            public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
            {

                using (var writer = new StreamWriter(writeStream))
                {
                    var products = value as IEnumerable<TimeSeriesType>;
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            WriteItem(product, writer);
                        }
                    }
                    else
                    {
                        var singleProduct = value as TimeSeriesType;
                        if (singleProduct == null)
                        {
                            throw new InvalidOperationException("Cannot serialize type");
                        }
                        WriteItem(singleProduct, writer);
                    }
                }
            }
            // Helper methods for serializing Products to CSV format. 
            private void WriteItem(TimeSeriesType timeSeriesType, StreamWriter writer)
            {
                var ts = timeSeriesType;
                SiteInfoType siteInfo;


                string siteCode;
                string sitenetwork;
                string siteName;
                string siteType;
                double lat;
                double lon;

                if (typeof (SiteInfoType) == ts.sourceInfo.GetType())
                {
                    siteInfo = ts.sourceInfo as SiteInfoType;

                    siteCode = siteInfo.siteCode.First().Value;
                    sitenetwork = siteInfo.siteCode.First().network;
                    siteName = siteInfo.siteName;
                   // siteType = siteInfo.siteType.ToString();
                    try
                    {
                        var loc = siteInfo.geoLocation.geogLocation;
                        if (loc.GetType().IsAssignableFrom(typeof (LatLonPointType)))
                        {
                            var pt = loc as LatLonPointType;
                            lat = pt.latitude;
                            lon = pt.longitude;
                        }
                        else
                        {
                            lat = -9999;
                            lon = -9999;
                        }
                    }
                    catch
                    {
                        // bad site
                        writer.WriteLine(
                            FormattedSeriesRow(network: sitenetwork, siteCode: siteCode, siteName: siteName)
                            );
                        return;
                    }

                }
                else
                {
                    return;
                }



                if (timeSeriesType != null)
                {
                    if (timeSeriesType != null)
                    {

                        var timeSeries = timeSeriesType;
                            var variableCode = timeSeries.variable.variableCode.First().Value;
                            var variableVocab = timeSeries.variable.variableCode.First().vocabulary;

                            foreach (var tsValues in timeSeries.values)
                            {
                               

                                if (tsValues.value != null)
                                {
                                    // some defaults on tsValues.value
                                    foreach (var aValue in tsValues.value)
                                    {
                                        
                                    
                                    var dataValue = aValue.Value;

                                    var obsDateTime = aValue.dateTime;
                                        DateTime obsdateTimeUTC;
                                        if (aValue.dateTimeUTCSpecified)
                                             obsdateTimeUTC = aValue.dateTimeUTC;

                                        var methodCode = aValue.methodCode;
                                        var methodID = aValue.methodID;
                                    

                                    writer.WriteLine(
                                        FormattedSeriesRow(network: sitenetwork, siteCode: siteCode,
                                            siteName: siteName,
                                            lat: lat.ToString(), lon: lon.ToString(),
                                            variableCode: variableCode, variableVocab: variableVocab,
                                            dataValue: dataValue,
                                            obsDateTime: obsDateTime.ToString("u")
                                            )
                                        );

                                    //writer.WriteLine("{0},{1},{2},{3},{4},{5}", Escape(siteCode), Escape(siteName),
                                    //    Escape(variableCode), Escape(variableVocab), Escape(startTime), Escape(endtime));
                                }
                            }
                                else
                                {
                                    writer.WriteLine("# no series for site {0} {1}", Escape(siteCode), Escape(siteName));
                                }
                            }
                        }
                    }
                    else
                    {
                        // only a site
                        writer.WriteLine(
                            FormattedSeriesRow(network: sitenetwork, siteCode: siteCode, siteName: siteName,
                            lat:lat.ToString(), lon:lon.ToString())
                            );
                    }
                
            }

            private string FormattedSeriesRow(string siteName = "",
                string siteCode = "",
                String variableCode = "", string network = "",
                string obsDateTime = "", decimal dataValue =-9999 , string variableName = "",
                string variableVocab="", string lat="", string lon="" )
            {

                return String.Format("{0}:{1},{2}:{3},{4},{5}", Escape(network), Escape(siteCode), variableVocab, Escape(variableCode)
                    , Escape(obsDateTime), dataValue);
            }
            static char[] _specialChars = new char[] { ',', '\n', '\r', '"' };

            private string Escape(object o)
            {
                if (o == null)
                {
                    return "";
                }
                string field = o.ToString();
                if (field.IndexOfAny(_specialChars) != -1)
                {
                    // Delimit the entire field with quotes and replace embedded quotes with "".
                    return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
                }
                else return field;
            }



        }  //namespace foramtter
    }
}