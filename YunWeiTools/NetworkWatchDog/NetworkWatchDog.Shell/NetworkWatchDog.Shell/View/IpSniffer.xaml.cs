using System.Windows.Controls;

using NetworkWatchDog.Shell.ViewModel;

namespace NetworkWatchDog.Shell.View
{
    /// <summary>
    /// IpSniffer.xaml 的交互逻辑
    /// </summary>
    public partial class IpSniffer:UserControl
    {
        public IpSniffer()
        {
            InitializeComponent();
            this.DataContext=new IpSnifferViewModel();
        }

        private void TabControl_Loaded(object sender,System.Windows.RoutedEventArgs e)
        {
            ((this.DataContext) as IpSnifferViewModel).GetIpStart();
        }
    }
}
