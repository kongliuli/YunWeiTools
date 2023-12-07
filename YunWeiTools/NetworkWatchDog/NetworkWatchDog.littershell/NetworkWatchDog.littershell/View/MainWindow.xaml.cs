using System.Diagnostics;
using System.Windows;

namespace NetworkWatchDog.littershell.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView:Window
    {
        public MainView()
        {
            InitializeComponent();




            this.MaxHeight=SystemParameters.PrimaryScreenHeight;

            this.WindowState=WindowState.Maximized;
        }

        private void Window_Closing(object sender,System.ComponentModel.CancelEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("NetworkWatchDog.littershell");
            foreach(Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
