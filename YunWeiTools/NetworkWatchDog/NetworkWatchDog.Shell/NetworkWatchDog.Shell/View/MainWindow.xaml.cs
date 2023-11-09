using System.Diagnostics;
using System.Windows;

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

        }

        private void Window_Closing(object sender,System.ComponentModel.CancelEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("NetworkWatchDog.Shell");
            foreach(Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
