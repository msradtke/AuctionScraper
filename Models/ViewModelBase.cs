using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using System.ComponentModel;
using PropertyChanged;
namespace AuctionScraper.Model
{
    public class ViewModelBase : IViewModelBase, INotifyPropertyChanged
    {
        List<object> _handlers; //keep reference to handlers alive
        public ViewModelBase()
        {
            ListenRegionIds = new List<Guid>();
            PublishRegionIds = new List<Guid>();
            _handlers = new List<object>();
            ChildViewModels = new List<IViewModelBase>();
        }
        /// <summary>
        /// Alerts a listener to change view based on event of other viewmodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="FactoryParameter">Parameter to pass to the viewmodel factory</param>
        public delegate void ChangeViewEventHandler(object sender, object FactoryParameter = null);

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public List<Guid> ListenRegionIds { get; set; }
        public List<Guid> PublishRegionIds { get; set; }

        public string Description { get; private set; }
        private bool _descriptionIsSet { get; set; }
        public bool SetDescription(string name)
        {
            if (!_descriptionIsSet)
            {
                Description = name;
                _descriptionIsSet = true;
            }
            return _descriptionIsSet;
        }
        public IEventAggregator EventAggregator { get; set; }

        protected List<IViewModelBase> ChildViewModels { get; set; }
        public void AddListenRegionId(Guid id)
        {
            ListenRegionIds.Add(id);
            foreach (var child in ChildViewModels)
                child.AddListenRegionId(id);
        }
        public void AddPublishRegionId(Guid id)
        {
            PublishRegionIds.Add(id);
            foreach (var child in ChildViewModels)
                child.AddPublishRegionId(id);
        }
        public void AddListenRegionIds(List<Guid> ids)
        {
            ListenRegionIds.AddRange(ids);
            foreach (var child in ChildViewModels)
                child.AddListenRegionIds(ids);
        }
        public void AddPublishRegionIds(List<Guid> ids)
        {
            PublishRegionIds.AddRange(ids);
            foreach (var child in ChildViewModels)
                child.AddPublishRegionIds(ids);
        }

        public virtual void Initialize() { }
    }
    public interface IViewModelBase
    {
        void AddListenRegionId(Guid id);
        void AddPublishRegionId(Guid id);
        void AddListenRegionIds(List<Guid> ids);
        void AddPublishRegionIds(List<Guid> ids);
        bool SetDescription(string description);
    }


}

