using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.Models
{
    public class BidDataItem
    {
        public int BidIndex { get; set; }
        public Bid Bid { get; set; }
        public string Notes { get; set; }
        public bool IsStarred { get; set; }
        public string PictureUrl { get; set; }

    }
}
