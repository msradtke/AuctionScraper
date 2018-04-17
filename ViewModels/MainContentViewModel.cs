using AuctionScraper.Models;
using AuctionScraper.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace AuctionScraper.ViewModels
{
    public class MainContentViewModel : WorkspaceViewModel, IMainContentViewModel
    {
        DataService _dataService;
        IAuctionWebScraperFactory _auctionWebScraperFactory;
        public MainContentViewModel(IEventAggregator eg, IAuctionWebScraperFactory auctionWebScraperFactory)
        {
            EventAggregator = eg;
            _auctionWebScraperFactory = auctionWebScraperFactory;

            EventAggregator.GetEvent<AuctionNewResponeEvent>().Subscribe(NewBidResponse);

            Bids = new ObservableCollection<Bid>();



            _dataService = new DataService();
            _dataService.Initialize();
            Bids = _dataService.GetCurrentBidsFromHistory();
            BidData = _dataService.GetBidData();
            BidHistory = _dataService.GetBidHistory();
            GetBidder();
            LoadPictureUrl();

            BidViewModel = new BidViewModel(Bids, BidData, BidHistory);
            ContentContainerViewModel = new ContentContainerViewModel(BidViewModel, BidHistory);


            ScrapeCommand = new ActionCommand(Scrape);
            SaveBidDataCommand = new ActionCommand(SaveBidData);
            SetTimer();
        }
        public ICommand ScrapeCommand { get; }
        public ICommand SaveBidDataCommand { get; }


        public AuctionWebScraper Scraper { get; set; }
        public ObservableCollection<Bid> Bids { get; set; }
        public BidData BidData { get; set; }
        public BidHistory BidHistory { get; set; }
        public BidViewModel BidViewModel { get; set; }
        public ContentContainerViewModel ContentContainerViewModel { get; set; }

        void SetTimer()
        {
            Timer timer = new Timer(10*60*1000);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Scrape();
        }

        void Scrape()
        {
            Scraper = _auctionWebScraperFactory.CreateAuctionWebScraper();
            Scraper.StartAsync();
        }

        void StopScrape()
        {
            //Scraper.Stop();            
        }

        void NewBidResponse(List<Bid> bids)
        {

            var changes = _dataService.CheckForBidChanges(Bids.ToList(), bids);
            if (changes)
            {
                Bids = _dataService.GetCurrentBidsFromHistory();
                
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    BidViewModel.SetBids(Bids);
                    BidHistory = _dataService.GetBidHistory();
                    ContentContainerViewModel.SetBidHistory(BidHistory);
                });
            }
            GetBidder();

        }

        void SaveBidData()
        {
            _dataService.SaveBidData(BidData);
        }

        async void GetBidder()
        {
            bool changes = false;
            List<BidHistoryItem> bidHistoryItems = new List<BidHistoryItem>();
            foreach (var bidHistory in BidHistory.BidHistoryItems)
            {
                
                if (String.IsNullOrWhiteSpace(bidHistory.Bid.Bidder))
                {
                    var bidder = await _dataService.GetBidder(bidHistory.Bid.DetailUrl, bidHistory.Bid.BidCount);
                    if (!String.IsNullOrWhiteSpace(bidder))
                    {

                        changes = true;
                        bidHistory.Bid.Bidder = bidder;
                        bidHistoryItems.Add(new BidHistoryItem { Bid = bidHistory.Bid, DateTime = bidHistory.DateTime });
                    }
                }
            }

            if (changes)
                _dataService.UpdateBidHistory(bidHistoryItems);

            Bids = _dataService.GetCurrentBidsFromHistory();
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                BidViewModel.SetBids(Bids);
                BidHistory = _dataService.GetBidHistory();
                ContentContainerViewModel.SetBidHistory(BidHistory);
            });
        }
        async void LoadPictureUrl()
        {
            bool changes = false;
            foreach (var bid in Bids)
            {
                var bidData = BidData.BidDataItems.FirstOrDefault(x => x.LotNumber == bid.LotNumber);
                if (bidData == null)
                    continue;
                if (String.IsNullOrWhiteSpace(bidData.PictureUrl))
                {
                    changes = true;
                    var pic = await _dataService.GetPictureUrl(bid.DetailUrl);
                    bidData.PictureUrl = "http://machinerymax.com/" + pic;
                }
            }
            if (changes)
                SaveBidData();
        }
    }

    public interface IMainContentViewModel { }
}
