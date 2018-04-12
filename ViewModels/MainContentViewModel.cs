﻿using AuctionScraper.Models;
using AuctionScraper.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            LoadPictureUrl();
            BidViewModel = new BidViewModel(Bids, BidData);

            ScrapeCommand = new ActionCommand(Scrape);
            SaveBidDataCommand = new ActionCommand(SaveBidData);
        }
        public ICommand ScrapeCommand{ get; }
        public ICommand SaveBidDataCommand { get; }


        public AuctionWebScraper Scraper { get; set; }
        public ObservableCollection<Bid> Bids { get; set; }
        public BidData BidData { get; set; }

        public BidViewModel BidViewModel { get; set; }
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
            if(changes)
            {
                Bids = _dataService.GetCurrentBidsFromHistory();
                BidViewModel.SetBids(Bids);
            }

            
        }

        void SaveBidData()
        {
            _dataService.SaveBidData(BidData);
        }

        async void LoadPictureUrl()
        {
            bool changes = false;
            foreach (var bid in Bids)
            {
                var bidData = BidData.BidDataItems.FirstOrDefault(x => x.BidIndex == bid.Index);
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
