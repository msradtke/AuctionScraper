using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionScraper.Models
{

    public class ViewModelContainer : WorkspaceViewModel
    {
        internal ObservableCollection<WorkspaceViewModel> _tabItems;
        internal WorkspaceViewModel _selectedTab;
        public ViewModelContainer()
        {
            _tabItems = new ObservableCollection<WorkspaceViewModel>();
        }

        public ObservableCollection<WorkspaceViewModel> TabItems
        {
            get { return _tabItems; }
            set
            {
                if (_tabItems != value)
                {
                    _tabItems = value;
                    RaisePropertyChanged("TabItems");
                }
            }
        }

        /* public WorkspaceViewModel SelectedTab
         {
             get { return _selectedTab; }
             set
             {
                 if (_selectedTab != value)
                 {
                     var valueType = value.GetType();
                     if (_tabItems.Any(c => c.GetType() == valueType))
                         _selectedTab = _tabItems.FirstOrDefault(x => x.GetType() == valueType);
                     else
                         _selectedTab = value;
                     RaisePropertyChanged("SelectedTab");
                 }
             }
         }*/
        public WorkspaceViewModel SelectedTab { get; set; }
        public int SelectedIndex { get; set; }
        public void AddTabItem(WorkspaceViewModel viewModel)
        {
            viewModel.Container = this;
            _tabItems.Add(viewModel);
            SelectedTab = viewModel;
        }


    }
}
