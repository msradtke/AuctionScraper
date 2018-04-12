using AuctionScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuctionScraper.ViewModels
{
    public class MainContentViewModel
    {
        public MainContentViewModel()
        {
            ScrapeCommand = new ActionCommand(Scrape);
            StopScrapeCommand = new ActionCommand(StopScrape);
        }
        public ICommand ScrapeCommand{ get; }
        public ICommand StopScrapeCommand { get; }


        public AuctionWebScraper Scraper { get; set; }
        void Scrape()
        {
            Scraper = new AuctionWebScraper();
            Scraper.StartAsync();
        }

        void StopScrape()
        {
            //Scraper.Stop();            
        }
    }
}
