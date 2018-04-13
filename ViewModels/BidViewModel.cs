using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.ViewModels
{
    public class BidViewModel : WorkspaceViewModel
    {
        public BidViewModel(ObservableCollection<Bid> bids, BidData bidData,BidHistory bidHistory)
        {
            HeaderText = "Bid Items";
            Bids = bids;
            BidData = bidData;
            BidHistory = bidHistory;
            SetBidAggregateViewModel();
        }
        public void SetBids(ObservableCollection<Bid> bids)
        {
            Bids = bids;
            SetBidAggregateViewModel();
        }
        void SetBidAggregateViewModel()
        {
            if (BidData == null)
                return;
            BidAggregateViewModels = new ObservableCollection<BidAggregateViewModel>();

            foreach (var bid in Bids)
            {
                var dataItem = BidData.BidDataItems.FirstOrDefault(x => x.BidIndex == bid.Index);

                var vm = new BidAggregateViewModel();
                vm.Bid = bid;
                vm.BidDataItem = dataItem;
                vm.BidDetailViewModel = new BidDetailViewModel(BidHistory);
                vm.BidDetailViewModel.Bid = bid;
                vm.BidDetailViewModel.BidDataItem = dataItem;
                
                BidAggregateViewModels.Add(vm);
            }
        }
        public ObservableCollection<Bid> Bids { get; set; }
        public BidData BidData { get; set; }
        public ObservableCollection<BidAggregateViewModel> BidAggregateViewModels { get; set; }
        public BidHistory BidHistory { get; set; }

        public BidDetailViewModel SelectedBidDetailViewModel { get; set; }
        public BidAggregateViewModel SelectedItem { get; set; }

    }

    public class BidAggregateViewModel : WorkspaceViewModel
    {
        public Bid Bid { get; set; }
        public BidDataItem BidDataItem { get; set; }
        public BidDetailViewModel BidDetailViewModel { get; set; }
        public bool IsSelected { get; set; }
    }
}
