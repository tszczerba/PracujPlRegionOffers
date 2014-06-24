using System.Collections.Generic;
using System.Linq;
using PracujPl.Commons.Database;
using PracujPl.Commons.RegionOffer;

namespace PracujPlRegionOffers.Viewer.ViewModels
{
    /// <summary>
    /// ViewModel of data returned by api
    /// </summary>
    public class JobOfferViewModel
    {

        public string Name;
        public string Id;
        public long SelfJobOffers;
        public long AggregateJobOffers;
        public int ProgressPercent;
        public List<JobOfferViewModel> Children;

        /// <summary>
        /// Mapper method from JobOffers to JobOffersViewModel
        /// Adds id that is used to identify html objects
        /// </summary>
        /// <param name="reg"></param>
        public JobOfferViewModel(RegionJobOffer reg, long allOffers)
        {
            Name = reg.RegionId.RegionName;
            Id = "roid" + reg.RegionId.RegionId;
            SelfJobOffers = reg.JobOffers;
            ProgressPercent = unchecked ((int)(reg.JobOffers*100/allOffers));
        }

        public JobOfferViewModel()
        {
        }

        public long PerformAggregations()
        {
            var agg = Children.Sum(r => r.PerformAggregations());
            AggregateJobOffers = SelfJobOffers + agg;

            foreach (var child in Children)
                child.CalculateProgressPercent(AggregateJobOffers);
            
            return AggregateJobOffers;
        }

        private void CalculateProgressPercent(long parentSum)
        {
            if (parentSum > 0)
                ProgressPercent = unchecked ((int) (AggregateJobOffers*100/parentSum));
            else
                ProgressPercent = 0;
        }
    }
}