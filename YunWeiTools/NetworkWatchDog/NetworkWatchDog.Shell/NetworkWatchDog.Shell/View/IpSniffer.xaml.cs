using System.Windows.Controls;

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
            this.DataContext=new IpSniffer();
        }
    }
}
