using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

namespace PracujPl.Commons.Database
{
    /// <summary>
    /// Database context of regions and regionJobOffers
    /// </summary>
    public class RegionJobOffersContext : DbContext
    {
        /// <summary>
        /// Run base constructor with given connection string to prevent LocalDb/SqlExpress publish
        /// </summary>
        public RegionJobOffersContext() :
            base(ConfigurationManager.ConnectionStrings["RegionJobOffersConnection"].ConnectionString)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionJobOffer> RegionJobOffers { get; set; } 

    }
}
