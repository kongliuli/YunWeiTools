namespace AWCForm.WebUsercontrols
{
    partial class alibabaWebControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing&&(components!=null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            chromiumWebBrowser1=new CefSharp.WinForms.ChromiumWebBrowser();
            SuspendLayout();
            // 
            // chromiumWebBrowser1
            // 
            chromiumWebBrowser1.ActivateBrowserOnCreation=false;
            chromiumWebBrowser1.Dock=DockStyle.Fill;
            chromiumWebBrowser1.Location=new Point(0,0);
            chromiumWebBrowser1.Name="chromiumWebBrowser1";
            chromiumWebBrowser1.Size=new Size(1070,633);
            chromiumWebBrowser1.TabIndex=0;
            // 
            // alibabaWebControl
            // 
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            Controls.Add(chromiumWebBrowser1);
            Name="alibabaWebControl";
            Size=new Size(1070,633);
            ResumeLayout(false);
        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chromiumWebBrowser1;
    }
}
