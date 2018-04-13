using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IronWebScraper;
using Prism.Events;

namespace AuctionScraper.Models
{
    public class AuctionWebScraper : WebScraper
    {
        IEventAggregator _eventAggregator;
        public AuctionWebScraper(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Bids = new List<Bid>();
        }
        public override void Init()
        {
            
            this.LoggingLevel = WebScraper.LogLevel.All;
            this.HttpRetryAttempts = 1;
            this.HttpTimeOut = TimeSpan.FromSeconds(3);
            var identity = new HttpIdentity();
            identity.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.9; rv:50.0) Gecko/20100101 Firefox/50.0";
            identity.HttpRequestHeaders.Add("Accept", "*/*");
            identity.HttpRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            identity.HttpRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            identity.HttpRequestHeaders.Add("Accept-Charset", "ISO - 8859 - 1, utf - 8; q = 0.7,*; q = 0.3");
            identity.HttpRequestHeaders.Add("User-Agent","Mozilla/5.0 (Macintosh; Intel Mac OS X 10.9; rv:50.0) Gecko/20100101 Firefox/50.0");
            identity.HttpRequestHeaders.Add("Host", "developer.mozilla.org");
            identity.HttpRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            identity.UseCookies = true;
            this.Identities.Add(identity);
            this.Request("http://machinerymax.com/Event/Details/10145158/COMING-SOON-Cornerstone-Architectural-Concepts-COMPLETE-SHOP-CLOSURE", Parse, identity);

            
            
        }


        public override void Parse(Response response)
        {
            var beginBidLength = Bids.Count;
            var i = beginBidLength;
            foreach (var title_link in response.Css("h1.title a"))
            {
                string strTitle = title_link.TextContentClean;
                string detailUrl = title_link.Attributes["href"];
                
                
                var newBid = new Bid();
                newBid.Name = strTitle;
                newBid.DetailUrl = detailUrl;
                

                Scrape(newBid);
                Bids.Add(newBid);
                ++i;

            }
            i = beginBidLength;
            foreach (var title_link in response.Css("h1.title em"))
            {
                string em = title_link.InnerTextClean;
                var resultString = Regex.Match(em, @"\d+").Value;
                Bids[i].LotNumber= Int32.Parse(resultString);

                ++i;
            }
            
                i = beginBidLength;
            foreach (var priceString in response.Css("span.NumberPart"))
            {
                string s = priceString.TextContentClean;
                try
                {
                    var converted=Convert.ToDouble(s);
                    Bids[i].CurrentBid = converted;
                }
                catch(FormatException fe)
                {

                }
                ++i;
            }

            i = beginBidLength;
            foreach (var bidCount in response.Css("span.awe-rt-AcceptedListingActionCount"))
            {
                string s = bidCount.TextContentClean;
                int count = 0;
                try
                {
                    var converted = Convert.ToInt32(s);
                    count = converted;
                }
                catch (FormatException fe)
                {

                }
                Bids[i].BidCount = count;

                ++i;
            }


            if (response.CssExists("ul.pagination > li:last-child > a[href]"))
            {
                
                var next_page = response.Css("ul.pagination > li:last-child > a[href]")[0].Attributes["href"];
                this.Request(next_page, Parse);
            }
            else
            {
                _eventAggregator.GetEvent<AuctionNewResponeEvent>().Publish(Bids.ToList());
            }

        }
        public List<Bid> Bids { get; set; }
    }
    
    public interface IAuctionWebScraperFactory
    {
        AuctionWebScraper CreateAuctionWebScraper();
    }
}
