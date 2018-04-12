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
        public BidDetailViewModel()
        {

        }
        public Bid Bid { get; set; }
        public BidDataItem BidDataItem { get; set; }
    }
}