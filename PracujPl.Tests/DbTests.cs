using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracujPl.Commons.Database;
using PracujPl.Commons.RegionOffer;

namespace PracujPl.Tests
{
    /// <summary>
    /// To nie są prawdziwe testy tylko mój "brudnopis" :)
    /// </summary>
    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public void DbConnectionTest()
        {
            using (var ctx = new RegionJobOffersContext())
            {
                const string name = "Tomkowo";

                var before = (from r in ctx.Regions
                             select r).Count();

                var dummy = new Region
                {
                    RegionId = 10000,
                    RegionName = name
                };

                ctx.Regions.Add(dummy);
                ctx.SaveChanges();

                var result = from r in ctx.Regions
                             where r.RegionName == name
                             select r;

                foreach (var r in result)
                    ctx.Regions.Remove(r);
                ctx.SaveChanges();

                var after = (from r in ctx.Regions
                              select r).Count();

                Assert.AreEqual(before,after);
            }
        }

        [TestMethod]
        public void GroupTest()
        {
            var x = new LoadRegionJobOffersFromDb();

            var y = x.Get();


        }

        [TestMethod]
        public void SeedAddOrUpdate()
        {
            using (var context = new RegionJobOffersContext())
            {
                var regions = (from r in context.Regions
                    select r).ToDictionary(r => r.RegionName, r => r);

            }
        }

        [TestMethod]
        public void GetFromSite()
        {
            ILoadRegionJobOffers l = new LoadRegionJobOffersFromSite();

            var r = l.Get();

        }

        [TestMethod]
        public void GroupByDate()
        {
            var region = "maZowIEckiE";
            var count = 3;

            using (var ctx = new RegionJobOffersContext())
            {
                var currentDateTime = DateTime.UtcNow.Date;

                var x = (from RegionJobOffer j in ctx.RegionJobOffers
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
                             RegionId = z.RegionId,
                             LoadDateTime = z.LoadDateTime,
                             JobOffers = z.JobOffers
                         })
                        ;

                foreach (var an in x)
                {
                    Console.WriteLine(an.ToString());
                }
                var x20 = 20;
            }
        }

        [TestMethod]
        public void GenerateDummyData()
        {
            var count = 10;
            var rand = new Random();
            using (var ctx = new RegionJobOffersContext())
            {
                var regiony = from r in ctx.Regions
                    select r;

                for(int i=0; i<count;i++)
                foreach (var r in regiony)
                {
                    ctx.RegionJobOffers.Add(new RegionJobOffer
                    {
                        JobOffers = rand.Next(100000),
                        RegionId = r,
                        LoadDateTime = DateTime.UtcNow.AddDays(-1*i)
                    });
                }
                ctx.SaveChanges();
            }
        }
    }
}
