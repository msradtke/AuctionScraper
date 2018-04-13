using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionScraper.Models;
namespace AuctionScraper.ViewModels
{
    public class BidDetailViewModel : WorkspaceViewModel
    {
        private Bid _bid;

        public BidDetailViewModel(BidHistory bidHistory)
        {
            BidHistoryViewModel = new BidHistoryViewModel(bidHistory);
        }
        public Bid Bid
        {
            get => _bid;
            set
            {               
                _bid = value;
                BidHistoryViewModel.SetBidHistories(_bid);
            }
        }
        public BidDataItem BidDataItem { get; set; }
        public BidHistory BidHistory { get; set; }
        public BidHistoryViewModel BidHistoryViewModel { get; set; }
    }
}