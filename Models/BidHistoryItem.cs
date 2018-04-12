using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AuctionScraper.Models
{
    [Serializable]
    public class BidHistoryItem
    {
        [XmlElement]
        public Bid Bid { get; set; }
        [XmlElement]
        public DateTime DateTime { get; set; }
    }
}
