using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using PracujPl.Commons.Database;

namespace PracujPl.Commons.RegionOffer
{
    /// <summary>
    /// Implementation of ILoadRegionRegionJobOffer to load RegionJobOffer from pracuj.pl site
    /// </summary>
    public class LoadRegionJobOffersFromSite : ILoadRegionJobOffers
    {
        /// <summary>
        /// Retrives pracuj.pl page and from object with id = "locationBox_items" retrives list of regions along with job offers
        /// </summary>
        /// <param name="url">Url of pracuj.pl search job site</param>
        /// <returns>Collection of RegionJobOffer objects constructed from retrived data</returns>
        public IEnumerable<RegionJobOffer> Get()
        {
            try
            {
                var currentDateTime = DateTime.UtcNow;
                List<Region> regionLkp = null;

                var url = ConfigurationManager.AppSettings["PracujPlUrl"];
                var pracujpl = new HtmlWeb();
                var page = pracujpl.Load(url);

                var locationBoxItems = page.GetElementbyId("locationBox_items");
                var spans = locationBoxItems.Descendants("span");

                var spanInnerText =
                    // Remove all whitespaces
                    spans.Select(w => Regex.Replace(HttpUtility.HtmlDecode(w.InnerText), @"\s+", String.Empty));

                return (spanInnerText
                    // Filter out top node - Cała Polska
                        .Where(w => !(w.ToLower().Contains("polska")))
                        .Select(w => GetRegionJobOfferFromString(w, currentDateTime))).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error getting data from site", e);
            }

        }

        /// <summary>
        /// Returns RegionOffers object constructed from input text having defined structure
        /// </summary>
        /// <param name="text">Input text. Must follow pattern "{Region name}({number of job offers})"</param>
        /// <returns>RegionOffers object with proper name and value of job offers.</returns>
        private static RegionJobOffer GetRegionJobOfferFromString(string text, DateTime currentDateTime)
        {
            long _JobOffers;
            Region regionId = null;
            // Random value added for testing
            //var r = new Random();

            using (var ctx = new RegionJobOffersContext())
            try
            {
                _JobOffers = int.Parse(Regex.Match(text, @"\(([^)]*)\)").Groups[1].Value);//+ r.Next(10000);
                var _name = Regex.Match(text, @"([^(]*)").Groups[1].Value.Trim().FirstCharToUpper();

                regionId = new Region{ RegionName = _name };
            }
            catch (Exception e)
            {
                throw new Exception("Error parsing data retrived from site", e);
            }

            return new RegionJobOffer
            {
                RegionId = regionId,
                JobOffers = _JobOffers,
                LoadDateTime = currentDateTime
            };
        }



        public IEnumerable<RegionJobOffer> GetLastDays(string region, int count)
        {
            var ret = Get().Where(r => r.RegionId.RegionName.ToLower() == region.ToLower());
            return ret;
        }
    }
}
