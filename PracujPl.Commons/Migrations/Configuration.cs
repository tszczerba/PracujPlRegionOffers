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
                new Region {RegionName = "�wiat", ParentRegion = null}
                );

            context.SaveChanges();

            context.Regions.AddOrUpdate(
                p =>  p.RegionName,
                new Region { RegionName = "Ca�a Polska", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "�wiat") },
                new Region { RegionName = "Zagranica", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "�wiat") }
                );

            context.SaveChanges();

            context.Regions.AddOrUpdate(
                p => p.RegionName,
                new Region { RegionName = "Mazowieckie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Dolno�l�skie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Ma�opolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Wielkopolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "�l�skie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Pomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "��dzkie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Kujawsko-pomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Zachodniopomorskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Lubelskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Podkarpackie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Warmi�sko-mazurskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Opolskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Lubuskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "�wi�tokrzyskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") },
                new Region { RegionName = "Podlaskie", ParentRegion = context.Regions.FirstOrDefault(p => p.RegionName == "Ca�a Polska") }
                );
        }
    }
}
