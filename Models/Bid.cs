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
        public int LotNumber { get; set; }
        public string PictureUrl { get; set; }
        public string DetailUrl { get; set; }
        public string Bidder { get; set; }

        public BidData BidData { get; set; }
    }
}
