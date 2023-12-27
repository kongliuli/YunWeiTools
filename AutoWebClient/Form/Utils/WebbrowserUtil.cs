using System.Text.Json;
using System.Text.RegularExpressions;

using CefSharp;

namespace AWCForm.Utils
{
    public static class WebbrowserUtil
    {
        /// <summary>
        /// 模拟点击指定选择符DOM元素
        /// </summary>
        /// <param name="selector">jQuery选择符</param>
        public static void ClickElement(IBrowser browser,double? left,double? top,bool isFocusEvent = false)
        {
            var host = browser.GetHost();
            var x = Convert.ToInt32(left)+5;
            var y = Convert.ToInt32(top)+5;
            host.SendMouseMoveEvent(x,y,false,CefEventFlags.None);//移动鼠标
            Thread.Sleep(50);
            host.SendMouseClickEvent(x,y,MouseButtonType.Left,false,1,CefEventFlags.None);//按下鼠标左键
            Thread.Sleep(50);
            host.SendFocusEvent(isFocusEvent);
            Thread.Sleep(50);
            host.SendMouseClickEvent(x,y,MouseButtonType.Left,true,1,CefEventFlags.None);//松开鼠标左键
        }

        /// <summary>
        /// 获取元素所在位置
        /// </summary>
        /// <param name="browser">浏览器对象</param>
        /// <param name="tag">标签(NodeName)</param>
        /// <param name="document">html文档(xpath才技持)</param>
        /// <returns></returns>
        public static BoundingClientRect GetBoundingClientRect(IBrowser browser,string tag,string document = "document")
        {
            var isXPath = Regex.IsMatch(tag,"(^//.*)");
            if(isXPath)
            {
                string script = "(function(){";
                script+="var doc= document.evaluate(\""+tag.Replace("\"","'")+"\","+document+",null,XPathResult.FIRST_ORDERED_NODE_TYPE,null).singleNodeValue;";
                script+=@"if(doc != null){
                       let bntRect = doc.getBoundingClientRect();
                       let result =JSON.stringify({ x: bntRect.left, y: bntRect.top,r:bntRect.right,b:bntRect.bottom });
                       return result;
                    };
                    return null;
                })()";
                Task<JavascriptResponse> result = browser.EvaluateScriptAsync(script);
                result.Wait();//等待脚本执行完
                var response = result.Result;
                if(response.Success&&response.Result==null)
                    return null;
                if(response.Result==null)
                    return null;
                BoundingClientRect? bc = JsonSerializer.Deserialize<BoundingClientRect>(response.Result.ToString());
                return bc;
            }
            else
            {
                string script = @"
(function(){
    // 选择登录按钮元素
let loginButton = document.querySelector('.fm-button');

// 检查是否找到登录按钮元素
if (loginButton) {
    // 获取登录按钮的位置信息
    let buttonRect = loginButton.getBoundingClientRect();

    // 构造位置信息对象
    let positionInfo = {
        x: buttonRect.left,
        y: buttonRect.top,
        r: buttonRect.right,
        b: buttonRect.bottom
    };

    // 将位置信息对象转换为JSON字符串
    let result = JSON.stringify(positionInfo);

    // 返回位置信息字符串
    result;
} else {
    console.log(""Element with class 'fm-button' not found"");
    null;
    }
})();
";
                Task<JavascriptResponse> result = browser.EvaluateScriptAsync(script);
                result.Wait();//等待脚本执行完
                var response = result.Result;

                if(response.Success&&response.Result==null)
                    return null;
                if(response.Result==null)
                    return null;
                BoundingClientRect? bc = JsonSerializer.Deserialize<BoundingClientRect>(response.Result.ToString());
                return bc;
            }
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
