using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.Models
{
    public class BidData
    {
        public BidData()
        {
            BidDataItems = new List<BidDataItem>();
        }
        public List<BidDataItem> BidDataItems { get; set; }

    }
}
