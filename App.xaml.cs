using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AuctionScraper.Models;
using AuctionScraper.ViewModels;
using Ninject;
using Ninject.Extensions.Factory;
using Prism.Events;

namespace AuctionScraper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();

        }

        void ConfigureContainer()
        {
            container = new StandardKernel();

            container.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            container.Bind<IMainContentViewModel>().To<MainContentViewModel>().InTransientScope();

            container.Bind<IAuctionWebScraperFactory>().ToFactory();

            var window = container.Get<MainWindow>();

            Current.MainWindow = window;

            Current.MainWindow.Show();

        }
    }
}
