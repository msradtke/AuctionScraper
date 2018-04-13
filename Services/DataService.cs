using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AuctionScraper.Services
{
    public class DataService
    {
        string _bidHistoryPath;
        string _bidDataPath;

        BidHistory _bidhistory;
        BidData _bidData;

        public DataService()
        {
            _bidHistoryPath = AppDomain.CurrentDomain.BaseDirectory + @"BidHistory.xml";
            _bidDataPath = AppDomain.CurrentDomain.BaseDirectory + @"BidData.xml";
        }
        public BidHistory GetBidHistory()
        {
            BidHistory bidHistory;
            XmlSerializer serializer = new XmlSerializer(typeof(BidHistory));
            using (var reader = XmlReader.Create(_bidHistoryPath))
            {
                bidHistory = (BidHistory)serializer.Deserialize(reader);
            }
            return bidHistory;
        }
        public ObservableCollection<Bid> GetCurrentBidsFromHistory()
        {
            ObservableCollection<Bid> currentBids = new ObservableCollection<Bid>();


            //load bidhistory and data
            XmlSerializer serializer = new XmlSerializer(typeof(BidHistory));
            using (var reader = XmlReader.Create(_bidHistoryPath))
            {
                _bidhistory = (BidHistory)serializer.Deserialize(reader);
            }

            serializer = new XmlSerializer(typeof(BidData));
            using (var reader = XmlReader.Create(_bidDataPath))
            {
                _bidData = (BidData)serializer.Deserialize(reader);
            }

            foreach (var bidDataItem in _bidData.BidDataItems)
            {
                BidHistoryItem latest = null;
                foreach (var bidHistoryItems in _bidhistory.BidHistoryItems)
                {
                    if (bidHistoryItems.Bid.Index == bidDataItem.Bid.Index)
                        if (latest == null)
                            latest = bidHistoryItems;
                        else if (bidHistoryItems.DateTime > latest.DateTime)
                            latest = bidHistoryItems;
                }

                currentBids.Add(latest.Bid);
            }

            return currentBids;
        }

        public bool CheckForBidChanges(List<Bid> currentBids, List<Bid> newBids)
        {
            BidHistory newBidHistory = new BidHistory();

            foreach (var nBid in newBids)
            {
                bool changed = false;
                var cBid = currentBids.FirstOrDefault(x => x.Index == nBid.Index);
                if (cBid == null)
                    changed = true;
                else if (nBid != null)
                {
                    if (cBid.BidCount < nBid.BidCount) //bidCountChange
                    {
                        changed = true;
                    }
                    else if (cBid.CurrentBid != nBid.CurrentBid)
                    {
                        changed = true;
                    }
                }
                if (changed)
                {
                    BidHistoryItem bidHistory = new BidHistoryItem();
                    bidHistory.DateTime = DateTime.Now;
                    bidHistory.Bid = nBid;
                    newBidHistory.BidHistoryItems.Add(bidHistory);
                }
            }

            if (currentBids.Count < newBids.Count)
            {
                var onlyNew = newBids.Except(currentBids).ToList();

                AddBidDataToFile(onlyNew);

            }

            if (newBidHistory.BidHistoryItems.Count > 0)
            {
                AddBidHistoryToFile(newBidHistory);
                return true;
            }


            return false;

        }
        public BidData GetBidData()
        {
            BidData data;
            XmlSerializer serializer = new XmlSerializer(typeof(BidData));
            using (var reader = XmlReader.Create(_bidDataPath))
            {
                data = (BidData)serializer.Deserialize(reader);
            }

            return data;
        }
        public void AddBidHistoryToFile(BidHistory bidHistory)
        {
            XDocument bidHistoryDocument = XDocument.Load(_bidHistoryPath);
            XElement root = bidHistoryDocument.Root;
            var items = root.Element("BidHistoryItems");


            foreach (var history in bidHistory.BidHistoryItems)
            {
                XElement element = history.ToXElement<BidHistoryItem>();
                items.Add(element);

            }
            bidHistoryDocument.Save(_bidHistoryPath);
        }

        public void AddBidDataToFile(List<Bid> newBids)
        {
            XDocument bidDataDoc = XDocument.Load(_bidDataPath);
            XElement root = bidDataDoc.Root;
            var items = root.Element("BidDataItems");


            foreach (var bid in newBids)
            {
                var dataItem = new BidDataItem();
                dataItem.Bid = bid;
                dataItem.BidIndex = bid.Index;

                XElement element = dataItem.ToXElement<BidDataItem>();
                items.Add(element);
            }
            bidDataDoc.Save(_bidDataPath);
        }
        public void SaveBidData(BidData bidData)
        {
            var serializer = new XmlSerializer(typeof(BidData));
            using (var writer = XmlWriter.Create(_bidDataPath))
            {
                serializer.Serialize(writer, bidData);
            }
        }
        public void Initialize()
        {
            if (!File.Exists(_bidDataPath))
            {
                var bidData = new BidData();
                var serializer = new XmlSerializer(typeof(BidData));
                using (var writer = XmlWriter.Create(_bidDataPath))
                {
                    serializer.Serialize(writer, bidData);
                }
            }
            if (!File.Exists(_bidHistoryPath))
            {
                var bidHistory = new BidHistory();
                var serializer = new XmlSerializer(typeof(BidHistory));
                using (var writer = XmlWriter.Create(_bidHistoryPath))
                {
                    serializer.Serialize(writer, bidHistory);
                }
            }
        }

        public async Task<string> GetPictureUrl(string detailUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                
                using (HttpResponseMessage response = await client.GetAsync(detailUrl))
                {
                    using (HttpContent content = response.Content)
                    {
                        var read = await content.ReadAsStringAsync();
                        string search = @"<img id=""previewimg"" class=""img-responsive full"" src=""";
                        var index = read.IndexOf(search, StringComparison.Ordinal);
                        var start = index + search.Length;
                        var endQuote = read.IndexOf('"', start);
                        var link = read.Substring(start, endQuote - start);
                        return link;
                    }
                }
            }
        }


    }
}
