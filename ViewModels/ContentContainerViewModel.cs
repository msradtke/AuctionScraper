using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.ViewModels
{
    public class ContentContainerViewModel : ViewModelContainer
    {
        public ContentContainerViewModel(BidViewModel bidViewModel, BidHistory bidHistory)
        {
            BidViewModel = bidViewModel;
            RecentActivity = new RecentBidActivityViewModel(bidHistory);
            AddTabItem(BidViewModel);
            AddTabItem(RecentActivity);
        }

        public void SetBidHistory(BidHistory bidHistory)
        {
            RecentActivity.SetBidHistory(bidHistory);
        }

        public BidViewModel BidViewModel { get; set; }
        public RecentBidActivityViewModel RecentActivity { get; set; }
    }
}