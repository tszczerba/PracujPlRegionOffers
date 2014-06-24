using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PracujPl.Commons.Database;

namespace PracujPlRegionOffers.Viewer.ViewModels
{
    public static class JobOfferModelViewFactory
    {
        /// <summary>
        /// Factory to place JobOffer records on Region hierarchy
        /// </summary>
        /// <returns></returns>
        public static JobOfferViewModel GetHierarchyFromDb(IEnumerable<RegionJobOffer> jobOffers )
        {
            List<Region> assoc = null;
            Region topNodeRegion;

            using (var ctx = new RegionJobOffersContext())
            {
                topNodeRegion = ctx.Regions.SingleOrDefault(r => r.ParentRegion == null);
                assoc = ctx.Regions.ToList();
            }

            if (topNodeRegion != null)
            {
                var ret = new JobOfferViewModel
                {
                    Id = "roid" + topNodeRegion.RegionId,
                    Name = topNodeRegion.RegionName,
                    SelfJobOffers = 0,
                    ProgressPercent = 100,
                    Children =
                        GetChildren(assoc.Where(r => r.ParentRegion != null), jobOffers, topNodeRegion.RegionId)
                };

                ret.PerformAggregations();
                return ret;
            }
            else
            {
                throw new Exception("Region dimension structure corrupted");
            }
        }


        /// <summary>
        /// Retrives childern of given parent node
        /// </summary>
        /// <param name="assoc">Paren-child Associacion table</param>
        /// <param name="elements">Elements to be placed on tree</param>
        /// <param name="parent">Parent node</param>
        /// <returns></returns>
        private static List<JobOfferViewModel> GetChildren(IEnumerable<Region> assoc, IEnumerable<RegionJobOffer> elements, int parent)
        {
            return assoc.Where(r => r.ParentRegion.RegionId == parent).Select(r =>
            {
                var rjo = elements.SingleOrDefault(rj => rj.RegionId.RegionId == r.RegionId);
                var jobOffers = (rjo == null) ? 0 : rjo.JobOffers;
                return new JobOfferViewModel
                {
                    Name = r.RegionName,
                    Id = "roid" + r.RegionId,
                    SelfJobOffers = jobOffers,
                    Children = GetChildren(assoc, elements, r.RegionId)
                };
            }).ToList();

        }
    }
}