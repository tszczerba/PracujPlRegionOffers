using PracujPl.Commons.Database;

namespace PracujPl.Commons.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RegionJobOffersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RegionJobOffersContext context)
        {
            // Initial regions
            context.Regions.AddOrUpdate(
                p => p.RegionName,
                new Region {RegionName = "Œwiat", ParentRegion = null}
                );

            context.SaveChanges();

            context.Regions.AddOrUpdate(
                p =>  p.RegionName,
                new Region { RegionName = "Ca³a Polska", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Œwiat") },
                new Region { RegionName = "Zagranica", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Œwiat") }
                );

            context.SaveChanges();

            context.Regions.AddOrUpdate(
                p => p.RegionName,
                new Region { RegionName = "Mazowieckie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Dolnoœl¹skie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Ma³opolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Wielkopolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Œl¹skie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Pomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "£ódzkie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Kujawsko-pomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Zachodniopomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Lubelskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Podkarpackie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Warmiñsko-mazurskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Opolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Lubuskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Œwiêtokrzyskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") },
                new Region { RegionName = "Podlaskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca³a Polska") }
                );
        }
    }
}
