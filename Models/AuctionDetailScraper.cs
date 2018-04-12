using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
namespace AuctionScraper.Models
{
    public class AuctionDetailScraper : WebScraper
    {
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
            identity.HttpRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.9; rv:50.0) Gecko/20100101 Firefox/50.0");
            identity.HttpRequestHeaders.Add("Host", "developer.mozilla.org");
            identity.HttpRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            identity.UseCookies = true;
            this.Identities.Add(identity);
        }
        public List<Bid> Bids { get; set; }
        public void GetDetails(List<Bid> bids)
        {
            
            Bids = bids;
            foreach (var bid in bids)
                Request(bid.DetailUrl, ParseDetail, new MetaData { { "index", bid.Index } });

        }
        public void ParseDetail(Response response)

        {
            var index = response.MetaData.Get<int>("index");
            var imgNode = response.Css("[id=previewimg]").FirstOrDefault();
            var link = imgNode.Attributes["src"];
            Bids[index].PictureUrl = link;
        }
        public override void Parse(Response response)
        {
            throw new NotImplementedException();
        }
    }
}
