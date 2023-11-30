using System.Windows.Controls;

using NetworkWatchDog.Shell.ViewModel;

namespace NetworkWatchDog.Shell.View
{
    /// <summary>
    /// RouteTrain.xaml 的交互逻辑
    /// </summary>
    public partial class RouteTrain:UserControl
    {
        public RouteTrain()
        {
            InitializeComponent();

            this.DataContext=new TraceRouteViewModel();
        }
    }
}
