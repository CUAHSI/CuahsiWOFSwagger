using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
//using Wb.hiscentral;
using Wb.org.cuahsi.hiscentral;


namespace Wb.Models
{
    // set of classes to write out CSV files

   

    namespace formatters
    {
        public class SeriesRecordCsvFormatter : BufferedMediaTypeFormatter
        {
            public SeriesRecordCsvFormatter()
            {
                // Add the supported media type.
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
            }


            public override bool CanWriteType(System.Type type)
            {
                if (type == typeof(SeriesRecord))
                {
                    return true;
                }
                else
                {
                    Type enumerableType = typeof(IEnumerable<SeriesRecord>);
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
                    writer.WriteLine(cvs_header());
                    var products = value as IEnumerable<SeriesRecord>;
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            WriteItem(product, writer);
                        }
                    }
                    else
                    {
                        var singleProduct = value as SeriesRecord;
                        if (singleProduct == null)
                        {
                            throw new InvalidOperationException("Cannot serialize type");
                        }
                        WriteItem(singleProduct, writer);
                    }
                }
            }

            private string cvs_header()
            {
                return String.Format("#fields={0}[type='string'],{1}[type='string'],{2}[unit='degrees'],{3}[unit='degrees'],{4}[type='string'],{5}[type='date' format='MM/dd/yyyy h:mm:ss a],{6}[type='date' format='MM/dd/yyyy h:mm:ss a],{7}[type='string']",
                    "location", "VarCode", "latitude", "longitude", "VarName", "beginDate", "endDate", "ServURL");
            }

            // Helper methods for serializing Products to CSV format. 
            private void WriteItem(SeriesRecord series, StreamWriter writer)
            {
                writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", Escape(series.location), Escape(series.VarCode),
                    Escape(series.latitude), Escape(series.longitude),
                    Escape(series.VarName), Escape(series.beginDate), Escape(series.endDate), Escape(series.ServURL));
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