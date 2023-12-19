namespace AWCForm
{
    public partial class Form1:Form
    {

        public string Uri = "";
        public Form1()
        {
            InitializeComponent();

            WebbrowserInit();
        }

        private void WebbrowserInit()
        {
            // 将应用程序的名称和版本号添加到注册表项
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var version = new Version(11,0,0,0);
            var regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey
                ($@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\{appName}");
            regKey.SetValue(appName,version.Major*1000+version.Minor,Microsoft.Win32.RegistryValueKind.DWord);

            // 创建WebBrowser控件并指定使用Microsoft Edge浏览器内核
            var webBrowser = new WebBrowser();
            webBrowser.Navigate("https://usercenter2.aliyun.com/ri/summary?spm=5176.9843921.content.12.62374882fJfxy9&commodityCode=&accounttraceid=51c17d5fd52b4c0eb8bcfbd6167e5ea0rygq");

            webBrowser.Dock=DockStyle.Fill;
            this.Controls.Add(webBrowser);
        }
    }
}