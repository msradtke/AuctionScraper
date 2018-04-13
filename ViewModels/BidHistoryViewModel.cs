using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.ViewModels
{
    public class BidHistoryViewModel : WorkspaceViewModel
    {
        public BidHistoryViewModel(BidHistory bidHistory)
        {
            BidHistory = bidHistory;
        }

        public void SetBidHistories(Bid bid)
        {
            var allHistoryItems = BidHistory.BidHistoryItems.Where(x => x.Bid.Index == bid.Index);
            BidHistoryAggregateViewModel = new BidHistoryAggregateViewModel();
            foreach (var item in allHistoryItems)
            {
                BidHistoryAggregateViewModel.BidHistoryItems.Add(item);
            }
            BidHistoryItems = BidHistoryAggregateViewModel.BidHistoryItems.OrderBy(x => x.DateTime).ToList();
        }

        public BidHistory BidHistory { get; set; }
        public BidHistoryAggregateViewModel BidHistoryAggregateViewModel { get; set; }
        public List<BidHistoryItem> BidHistoryItems { get; set; }
    }
    public class BidHistoryAggregateViewModel : WorkspaceViewModel
    {
        public BidHistoryAggregateViewModel()
        {
            BidHistoryItems = new ObservableCollection<BidHistoryItem>();
        }
        public ObservableCollection<BidHistoryItem> BidHistoryItems { get; set; }

    }
}
