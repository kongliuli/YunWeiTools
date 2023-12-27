using CefSharp;
using CefSharp.WinForms;

namespace AWCForm.Model
{
    public class WebBrowserByCefSharp:ChromiumWebBrowser
    {
        /// <summary>
        /// 遍历全部的frame执行js,完成命令返回第一个frame
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public IFrame FindFrameQueueByScript(string script)
        {
            IFrame? fr = null;
            foreach(var a in this.GetBrowser().GetFrameNames())
            {
                var frame = this.GetBrowser().GetFrame(a);
                if(frame!=null)
                {
                    var response = frame.EvaluateScriptAsPromiseAsync(script).Result;
                    if(response!=null&&response.Success==true)
                    {
                        if(response.Result!=null)
                        {
                            fr=frame;
                            break;
                        }
                    }
                }
            }
            if(fr is not null)
            {
                return fr;
            }
            return null;
        }

    }
}
