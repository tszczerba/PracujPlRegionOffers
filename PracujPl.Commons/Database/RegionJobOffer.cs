using System;
using System.ComponentModel.DataAnnotations;

namespace PracujPl.Commons.Database
{
    public class RegionJobOffer
    {
        /// <summary>
        /// Class that represents number of job offers in given region
        /// </summary>
        [Key]
        public int RegionJobOfferId { get; set; }
        public virtual Region RegionId { get; set; }
        public DateTime LoadDateTime { get; set; }
        public long JobOffers { get; set; }
    }
}
