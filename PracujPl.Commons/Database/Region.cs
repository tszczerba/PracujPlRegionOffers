using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracujPl.Commons.Database
{
    /// <summary>
    /// Class that represents Regions in database
    /// </summary>
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        [MaxLength(128)]
        public string RegionName { get; set; }
        public Region ParentRegion { get; set; }
        public List<Region> ChildRegions { get; set; } 
        public virtual List<RegionJobOffer> RegionJobOffers { get; set; }
         
    }
}
