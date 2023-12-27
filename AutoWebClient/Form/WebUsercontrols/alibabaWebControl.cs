using AWCForm.Model;
using AWCForm.Utils;

using CefSharp;

namespace AWCForm.WebUsercontrols
{
    public partial class alibabaWebControl:UserControl
    {
        private readonly string alibabaLoginUrl = "https://account.aliyun.com/login/login.htm?oauth_callback=https%3A%2F%2Fusercenter2.aliyun.com%2Fri%2Fsummary%3Fspm%3D5176.9843921.content.12.62374882fJfxy9%26commodityCode%3D";
        private string loginid = string.Empty;
        private string password = string.Empty;
        WebBrowserByCefSharp browserByCefSharp = new WebBrowserByCefSharp();
        public alibabaWebControl(string id,string psd)
        {
            InitializeComponent();
            this.loginid=id;
            this.password=psd;
            this.Dock=DockStyle.Fill;
            browserByCefSharp.Dock=DockStyle.Fill;
            browserByCefSharp.Load(alibabaLoginUrl);
            this.Controls.Add(browserByCefSharp);


            BrowserInit();
        }
        #region chromiumWebBrowser1
        //private void WebBrowserInit()
        //{
        //    chromiumWebBrowser1.FrameLoadEnd+=(sender,args) =>
        //    {
        //        if(args.Frame.IsMain)
        //        {
        //            DoSomeThing();
        //        }
        //    };
        //}

        //private async void DoSomeThing()
        //{
        //    string script = @"
        //var usernameInput = document.getElementById('fm-login-id');
        //var passwordInput = document.getElementById('fm-login-password');
        //if (usernameInput && passwordInput) {
        //    usernameInput.value = '"+loginid+@"';
        //    passwordInput.value='"+password+@"';
        //}";


        //        {
        //            string tag = "fm-btn";
        //            string script1 = "(function(){";
        //            script1+="var doc = document.getElementById('"+tag+"');";
        //            script1+=@"if(doc){
        //               let bntRect = doc.getBoundingClientRect();
        //               let result =JSON.stringify({ x: bntRect.left, y: bntRect.top,r:bntRect.right,b:bntRect.bottom });
        //               return result;
        //            };
        //            return null;
        //        })()";
        //            Task<JavascriptResponse> result = frame.EvaluateScriptAsync(script1);
        //            result.Wait();//等待脚本执行完
        //            var response1 = result.Result;
        //            if(!response1.Success&&response1.Result!=null)
        //            {
        //                BoundingClientRect? bc = JsonSerializer.Deserialize<BoundingClientRect>(response1.Result.ToString());
        //                MouseClick(frame,(int)bc.x,(int)bc.y);
        //            }


        //        }
        //    }


        //}

        //public void MouseClick(IFrame Browser,int x,int y)
        //{
        //    Browser.Browser.GetHost().SendMouseClickEvent(x,y,MouseButtonType.Left,false,1,CefEventFlags.None);
        //    Thread.Sleep(15);
        //    Browser.Browser.GetHost().SendMouseClickEvent(x,y,MouseButtonType.Left,true,1,CefEventFlags.None);
        //}
        #endregion


        private void BrowserInit()
        {
            browserByCefSharp.FrameLoadEnd+=(sender,args) =>
            {
                if(args.Frame.IsMain)
                {
                    Dosomething();
                }
            };
            browserByCefSharp.ConsoleMessage+=(sender,e) =>
            {
                MessageBox.Show(e.Message);
            };
        }

        private async void Dosomething()
        {
            #region Login
            string writeidandpsd = @"var usernameInput = document.getElementById('fm-login-id');
var passwordInput = document.getElementById('fm-login-password');
if (usernameInput && passwordInput) {
usernameInput.value = '"+loginid+@"';
passwordInput.value='"
            +password+@"';
}";

            var FramesName = browserByCefSharp.GetBrowser().GetFrameNames();
            List<IFrame> Frames = new();
            for(int i = 0;i<FramesName.Count;i++)
            {
                Frames.Add((IFrame?)browserByCefSharp.GetBrowser().GetFrame(FramesName[i]));
            }

            foreach(var frame in Frames)
            {
                var response = await frame.EvaluateScriptAsync(writeidandpsd);
                if(response.Result!=null)
                {
                    var bc = WebbrowserUtil.GetBoundingClientRect(frame.Browser,".fm-button");
                    if(bc!=null)
                    {
                        WebbrowserUtil.ClickElement(frame.Browser,bc.x,bc.y);
                    }
                }
            }
            #endregion
        }
    }

}
