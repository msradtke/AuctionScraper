﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AuctionScraper.Models
{
    public class BidHistory
    {
        public BidHistory()
        {
            BidHistoryItems = new List<BidHistoryItem>();
        }
        public List<BidHistoryItem> BidHistoryItems { get; set; }
    }
}
