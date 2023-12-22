using CefSharp;

namespace AWCForm.WebUsercontrols
{
    public partial class alibabaWebControl:UserControl
    {
        private readonly string alibabaLoginUrl = "https://account.aliyun.com/login/login.htm?oauth_callback=https%3A%2F%2Fusercenter2.aliyun.com%2Fri%2Fsummary%3Fspm%3D5176.9843921.content.12.62374882fJfxy9%26commodityCode%3D";
        private string loginid = string.Empty;
        private string password = string.Empty;
        public alibabaWebControl(string id,string psd)
        {
            InitializeComponent();
            this.loginid=id;
            this.password=psd;
            this.Dock=DockStyle.Fill;

            chromiumWebBrowser1.LoadUrl(alibabaLoginUrl);

            WebBrowserInit();

        }

        private void WebBrowserInit()
        {
            chromiumWebBrowser1.FrameLoadEnd+=async (sender,args) =>
            {
                if(args.Frame.IsMain)
                {
                    DoSomeThing();
                }
            };
        }

        private async void DoSomeThing()
        {

            string script = @"var iframe = document.getElementById('alibaba-login-iframe');
                                    if (iframe) {
                                        var usernameInput = iframe.contentDocument.getElementById('fm-login-id');
                                        var passwordInput = iframe.contentDocument.getElementById('fm-login-password');
                                        if (usernameInput && passwordInput) {
                                            usernameInput.value = '"+loginid+"';"+
                                    "passwordInput.value = '"+password+"';"+
                                    "}"+
                                    "} else {console.error('The iframe element was not found.');}";
            var a = await chromiumWebBrowser1.GetMainFrame().EvaluateScriptAsync(script);
        }
    }
}
