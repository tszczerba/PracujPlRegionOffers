using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracujPl.Commons.Database;

namespace PracujPl.Commons.RegionOffer
{
    /// <summary>
    /// Loads RegionJobOffer objects from database
    /// </summary>
    public class LoadRegionJobOffersFromDb : ILoadRegionJobOffers
    {
        public IEnumerable<RegionJobOffer> Get()
        {
            using (var ctx = new RegionJobOffersContext())
            {
                return (from RegionJobOffer j in ctx.RegionJobOffers
                        group j by j.RegionId into g
                        select g.OrderByDescending(t => t.LoadDateTime)
                        .FirstOrDefault())
                        .Include("RegionId")
                        .ToList();
            }
        }

        public IEnumerable<RegionJobOffer> Get(DateTime dateTime)
        {
            using (var ctx = new RegionJobOffersContext())
            {
                return (from j in ctx.RegionJobOffers
                        where j.LoadDateTime == dateTime
                        group j by j.RegionId into g
                        select g.OrderByDescending(t => t.LoadDateTime).FirstOrDefault()).ToList();
            }
        }

        public IEnumerable<RegionJobOffer> GetLastDays(string region, int count)
        {
            using (var ctx = new RegionJobOffersContext())
            {
                var currentDateTime = DateTime.UtcNow.Date;

                // Sql compact doesn't allow creation of object in select clause
                var ret = (from RegionJobOffer j in ctx.RegionJobOffers
                         where j.RegionId.RegionName.ToLower() == region.ToLower()
                         && j.LoadDateTime >= DbFunctions.AddDays(currentDateTime, -1 * count)
                         group j by new { Date = DbFunctions.TruncateTime(j.LoadDateTime), Region = j.RegionId } into g
                         select new
                         {
                             RegionId = g.Key.Region,
                             LoadDateTime = g.Key.Date.Value,
                             JobOffers = (long)Math.Round(g.Average(r => r.JobOffers))
                         }).ToList().Select(z => new RegionJobOffer
                         {
                             RegionId = new Region
                             {
                                 RegionId = z.RegionId.RegionId,
                                 RegionName = z.RegionId.RegionName
                             },
                             LoadDateTime = z.LoadDateTime,
                             JobOffers = z.JobOffers
                         }).ToList();

                return ret;
            }
        }
    }
}
