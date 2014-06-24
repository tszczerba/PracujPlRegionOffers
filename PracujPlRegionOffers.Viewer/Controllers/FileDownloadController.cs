using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PracujPlRegionOffers.Viewer.Controllers
{
    public class FileDownloadController : ApiController
    {
        static TraceSource Log = new TraceSource("PracujPlJobOffers");
        // GET: api/FileDownload/weblog
        public HttpResponseMessage GetFile(string name)
        {
            var allowedProps = new List<string> {"source", "weblog", "consolelog", "dbfile"};

            if (!allowedProps.Contains(name.ToLower()))
            {
                Log.TraceInformation("Invalid request: {0}", name);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                var response = new HttpResponseMessage();
                var path = ConfigurationManager.AppSettings[name.ToUpper()];

                Log.TraceInformation("Requested file: {0}", name);
                if (!File.Exists(path))
                {
                    Log.TraceInformation("File doesn't exist: {0}", name);
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                var ms = new MemoryStream();

                using(var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    file.Close();
                }
                response.Content = new ByteArrayContent(ms.ToArray());
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"){FileName = Path.GetFileName(path)};
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.StatusCode = HttpStatusCode.OK;
                Log.TraceInformation("Served file: {0}", name);
                return response;


            }
            catch (Exception e)
            {
                Log.TraceEvent(TraceEventType.Error, 0, "Exception in api/FileDownload Get");
                Log.TraceData(TraceEventType.Error, 0, e);
                return null;
                
            }
        }

    }
}
