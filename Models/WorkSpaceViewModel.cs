﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using AuctionScraper.Model;
namespace AuctionScraper.Models
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        private ViewModelBase _container;
        public string HeaderText { get; set; }
        public override string ToString()
        {
            return HeaderText;
        }

        public ViewModelBase Container
        {
            get { return _container; }
            set
            {
                if (_container != value)
                {
                    _container = value;
                    RaisePropertyChanged("Container");
                }
            }
        }
        protected void CloseDialog()
        {
        }
    }
}
