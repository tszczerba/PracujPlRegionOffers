using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using PracujPl.Commons.Database;
using PracujPl.Commons.RegionOffer;
using PracujPlRegionOffers.Viewer.ViewModels;

namespace PracujPlRegionOffers.Controllers
{
    /// <summary>
    /// Web api controller that responds with hierarchy of Job Offers
    /// </summary>
    [RoutePrefix("api")]
    public class RegionOffersController : ApiController
    {
        static TraceSource Log = new TraceSource("PracujPlJobOffers");

        // GET: api/JobOffers
        [HttpGet, Route("JobOffers")]
        public IHttpActionResult Get()
        {
            try
            {
                Log.TraceEvent(TraceEventType.Verbose, 0, "In api/JobOffers Get");
                ILoadRegionJobOffers loadRegionJobOffers = new LoadRegionJobOffersFromDb();

                var jobOffers = loadRegionJobOffers.Get();
                if (jobOffers == null)
                {
                    var e = new Exception("No Records in database");
                    Log.TraceData(TraceEventType.Error, 0, e);
                    return InternalServerError(e);
                }
                var ret = JobOfferModelViewFactory.GetHierarchyFromDb(jobOffers);
                Log.TraceEvent(TraceEventType.Verbose, 0, "Out of api/JobOffers Get");
                return Ok(ret);

            }
            catch(Exception e)
            {
                Log.TraceEvent(TraceEventType.Error, 0, "Exception in api/JobOffers Get");
                Log.TraceData(TraceEventType.Error, 0, e);
                throw new Exception("Exception in api/JobOffers Get", e);
            }
        }

        // GET: api/JobOffers/Region/mazowieckie/3
        [HttpGet, Route("JobOffers/{name}/{count}")]
        public IHttpActionResult Get(string name, int? count)
        {
            try
            {
                if (name.IsEmpty() || count == null)
                    return NotFound();

                var daysBack = count.Value;

                Log.TraceEvent(TraceEventType.Verbose, 0, String.Format("In api/JobOffers/{0}/{1} Get", name, count));
                ILoadRegionJobOffers loadRegionJobOffers = new LoadRegionJobOffersFromDb();

                var ret = loadRegionJobOffers.GetLastDays(name, daysBack);
                if (ret == null)
                {
                    var e = new Exception("No data found");
                    Log.TraceData(TraceEventType.Error, 0, e);
                    return InternalServerError(e);
                }
                Log.TraceEvent(TraceEventType.Verbose, 0, String.Format("Out of api/JobOffers/{0}/{1} Get", name, count));
                return Ok(ret);

            }
            catch (Exception e)
            {
                var er = String.Format("Exception in api/JobOffers/{0}/{1} Get", name, count);
                Log.TraceEvent(TraceEventType.Error, 0, er);
                Log.TraceData(TraceEventType.Error, 0, e);
                throw new Exception(er, e);
            }
        }



    }
}
