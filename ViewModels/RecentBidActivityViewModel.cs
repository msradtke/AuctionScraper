using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuctionScraper.ViewModels
{
    public class RecentBidActivityViewModel : WorkspaceViewModel
    {
        public RecentBidActivityViewModel(BidHistory bidHistory)
        {
            HeaderText = "Recent Activity";
            Hours = 1;

            BidActivityHistoryViewModel = new BidActivityHistoryViewModel();
            BidHistoryItems = new ObservableCollection<BidHistoryItem>();
            BidHistory = bidHistory;
            SearchActivityCommand = new ActionCommand(SearchActivity);

            SearchActivity();

        }
        public ICommand SearchActivityCommand { get; set; }
        public ObservableCollection<BidHistoryItem> BidHistoryItems { get; set; }
        public BidActivityHistoryViewModel BidActivityHistoryViewModel { get; set; }
        public BidHistory BidHistory { get; set; }
        public int Hours { get; set; }

        public void SetBidHistory(BidHistory bidHistory)
        {
            BidHistory = bidHistory;
            SearchActivity();
        }
        void SearchActivity()
        {
            BidActivityHistoryViewModel.BidHistoryItems.Clear();
            if (BidHistory != null)
                foreach(var item in BidHistory.BidHistoryItems)
                {
                    var timeNow = DateTime.Now;
                    if (timeNow - item.DateTime <= TimeSpan.FromHours(Hours))
                    {
                        BidHistoryItems.Add(item);

                        var vm = new BidActivityAggregateViewModel();
                        vm.Name = item.Bid.Name;
                        vm.DateTime = item.DateTime;
                        vm.CurrentBid = item.Bid.CurrentBid;
                        vm.BidCount = item.Bid.BidCount;
                        vm.LotNumber = item.Bid.LotNumber;
                        var previousBid = BidHistory.BidHistoryItems.Where(x => x.Bid.LotNumber == item.Bid.LotNumber && x.DateTime < item.DateTime).OrderByDescending(x => x.DateTime).FirstOrDefault();
                        if (previousBid != null)
                        {
                            vm.PreviousBidAmount = previousBid.Bid.CurrentBid;
                            vm.PreviousBidCount = previousBid.Bid.BidCount;
                            vm.PreviousDateTime = previousBid.DateTime;
                        }

                        BidActivityHistoryViewModel.BidHistoryItems.Add(vm);
                    }
                }
        }
    }

    public class BidActivityHistoryViewModel : WorkspaceViewModel
    {
        public BidActivityHistoryViewModel()
        {
            BidHistoryItems = new ObservableCollection<BidActivityAggregateViewModel>();
        }
        public ObservableCollection<BidActivityAggregateViewModel> BidHistoryItems { get; set; }
    }
    public class BidActivityAggregateViewModel : WorkspaceViewModel
    {
        public int LotNumber{ get; set; }
        public string Name { get; set; }
        public int BidCount { get; set; }
        public double CurrentBid { get; set; }
        public DateTime DateTime { get; set; }
        public int PreviousBidCount { get; set; }
        public double PreviousBidAmount { get; set; }
        public DateTime PreviousDateTime { get; set; }
    }
}
