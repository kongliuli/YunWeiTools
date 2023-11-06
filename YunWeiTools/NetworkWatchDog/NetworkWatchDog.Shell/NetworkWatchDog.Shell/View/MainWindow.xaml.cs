using System.Windows;
using System.Windows.Controls;

using NetworkWatchDog.Shell.ViewModel;

namespace NetworkWatchDog.Shell.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView:Window
    {
        public MainView()
        {
            InitializeComponent();

            this.DataContext=new MainViewModel();

            this.MaxHeight=SystemParameters.PrimaryScreenHeight;

            this.WindowState=WindowState.Maximized;
        }

        public void TabControl_Loaded(object sender,RoutedEventArgs e)
        {
            // 获取TabControl
            TabControl tabControl = sender as TabControl;

            // 将用户控件添加到相应的容器中
            (tabControl.Items[0] as TabItem).Content=new IpSniffer();

        }
    }
}
