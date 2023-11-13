using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using HandyControl.Controls;

namespace NetworkWatchDog.Shell.ViewModel
{

    public class TraceRouteViewModel:ObservableObject
    {
        private ObservableCollection<TabItem> _tabItems;
        public ObservableCollection<TabItem> TabItems
        {
            get; set;
        }

        public void AddTraceRoute(string ip)
        {
            TabItems.Add(new TabItem { Header=ip });
        }

    }
}
