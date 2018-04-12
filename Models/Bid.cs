using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.Models
{
    public class Bid
    {
        public string Name { get; set; }
        public int BidCount { get; set; }
        public double CurrentBid { get; set; }
    }
}
