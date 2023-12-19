using System.Windows.Controls;
using System.Windows.Navigation;

namespace Shell.View
{
    /// <summary>
    /// WebBrowserView.xaml 的交互逻辑
    /// </summary>
    public partial class WebBrowserView:UserControl
    {
        public WebBrowserView()
        {
            InitializeComponent();
        }

        private void WebBrowser_Navigated(object sender,NavigationEventArgs e)
        {
            var a = (WebBrowser)sender;
            var c = a.Document.ToString();
        }
    }
}
