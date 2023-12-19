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
            // ��Ӧ�ó�������ƺͰ汾����ӵ�ע�����
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var version = new Version(11,0,0,0);
            var regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey
                ($@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\{appName}");
            regKey.SetValue(appName,version.Major*1000+version.Minor,Microsoft.Win32.RegistryValueKind.DWord);

            // ����WebBrowser�ؼ���ָ��ʹ��Microsoft Edge������ں�
            var webBrowser = new WebBrowser();
            webBrowser.Navigate("https://usercenter2.aliyun.com/ri/summary?spm=5176.9843921.content.12.62374882fJfxy9&commodityCode=&accounttraceid=51c17d5fd52b4c0eb8bcfbd6167e5ea0rygq");

            webBrowser.Dock=DockStyle.Fill;
            this.Controls.Add(webBrowser);
        }
    }
}