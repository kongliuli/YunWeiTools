using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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

            //chromiumWebBrowser1.LoadUrl(alibabaLoginUrl);
            //WebBrowserInit();

            GrabUrlByKeyWord();
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

        //    var FramesName = chromiumWebBrowser1.GetBrowser().GetFrameNames();
        //    List<IFrame> Frames = new();
        //    for(int i = 0;i<FramesName.Count;i++)
        //    {
        //        Frames.Add((IFrame?)chromiumWebBrowser1.GetBrowser().GetFrame(FramesName[i]));
        //    }

        //    foreach(var frame in Frames)
        //    {
        //        var response = await frame.EvaluateScriptAsync(script);
        //        if(response.Result!=null)
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

        public void GrabUrlByKeyWord()
        {
            ChromeOptions options = new ChromeOptions();
            //创建chrome驱动程序
            IWebDriver webDriver = new ChromeDriver(options);
            //跳至百度
            webDriver.Navigate().GoToUrl(alibabaLoginUrl);
            //找到页面上的搜索框 输入关键字
            webDriver.FindElement(By.Id("Account")).SendKeys(loginid);
            webDriver.FindElement(By.Id("Password")).SendKeys(password);
            //点击搜索按钮
            webDriver.FindElement(By.Id("btnWebUser")).Click();

            webDriver.Url=alibabaLoginUrl;

            while(true)
            {
                var isHaveNext = DoNext(webDriver);

                if(isHaveNext==false)
                {
                    break;
                }
            }
            webDriver.Close();
        }

    }
    [Serializable]
    public class BoundingClientRect
    {

        public double? x
        {
            get; set;
        }
        public double? y
        {
            get; set;
        }
        public double? r
        {
            get; set;
        }
        public double? b
        {
            get; set;
        }
    }
}
