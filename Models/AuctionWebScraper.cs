using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
namespace AuctionScraper.Models
{
    public class AuctionWebScraper : WebScraper
    {

        public override void Init()
        {
            Bids = new List<Bid>();
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
            foreach (var title_link in response.Css("h1.title a"))
            {
                string strTitle = title_link.TextContentClean;
                var newBid = new Bid();
                newBid.Name = strTitle;
                Scrape(newBid);
                Bids.Add(newBid);
            }   

            if (response.CssExists("ul.pagination > li:last-child > a[href]"))
            {
                
                var next_page = response.Css("ul.pagination > li:last-child > a[href]")[0].Attributes["href"];
                this.Request(next_page, Parse);
            }
            
        }

        public List<Bid> Bids { get; set; }
    }

    public class Bid
    {
        public string Name { get; set; }
    }
}
