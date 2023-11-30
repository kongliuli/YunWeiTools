using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using NetworkWatchDog.Shell.Model;

using TabItem = HandyControl.Controls.TabItem;

namespace NetworkWatchDog.Shell.ViewModel
{

    public class TraceRouteViewModel:ObservableObject
    {
        private ObservableCollection<TabItem>? _tabItems;
        public ObservableCollection<TabItem>? TabItems
        {
            get; set;
        }

        public ObservableCollection<TradeRouteModel>? TR
        {
            get; set;
        }

        private ObservableCollection<TradeRouteModel>? _tr;

        public void AddTraceRoute(string ip)
        {
            TabItems?.Add(new TabItem { Header=ip });
        }

    }
}
