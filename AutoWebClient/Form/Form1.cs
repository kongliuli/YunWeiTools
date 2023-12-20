using CefSharp;
using CefSharp.WinForms;

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
            CefSettings settings = new CefSettings
            {
                CachePath="C:\\Users\\HP\\AppData\\Local\\Microsoft\\Edge Dev\\User Data\\Default\\Cache"
            };
            Cef.Initialize(settings);

            // ����WebBrowser�ؼ���ָ��ʹ��Microsoft Edge������ں�
            var webBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            //webBrowser.LoadUrl("https://ie.icoa.cn/");
            webBrowser.LoadUrl("https://account.aliyun.com/login/login.htm?oauth_callback=https%3A%2F%2Fusercenter2.aliyun.com%2Fri%2Fsummary%3Fspm%3D5176.9843921.content.12.62374882fJfxy9%26commodityCode%3D");


            webBrowser.Dock=DockStyle.Fill;
            webBrowser.LoadingStateChanged+=(sender,args) =>
            {
                if(args.IsLoading==false)
                {
                    // ҳ��������
                }
            };
            //login kujiang:YO@JQM!lsiS&ILo$


            this.Controls.Add(webBrowser);
        }
    }
}