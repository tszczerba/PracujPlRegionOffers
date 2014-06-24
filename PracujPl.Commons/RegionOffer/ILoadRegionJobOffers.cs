using System.Collections.Generic;
using PracujPl.Commons.Database;

namespace PracujPl.Commons.RegionOffer
{
    /// <summary>
    /// Interface that defines how to get RegionOffers
    /// </summary>
    public interface ILoadRegionJobOffers
    {
        IEnumerable<RegionJobOffer> Get();
        IEnumerable<RegionJobOffer> GetLastDays(string region, int count);
    }
}