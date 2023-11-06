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

    }
}
