namespace NetworkWatchDog
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing&&(components!=null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components=new System.ComponentModel.Container();
            timer1=new System.Windows.Forms.Timer(components);
            menuStrip1=new MenuStrip();
            中断ping请求ToolStripMenuItem=new ToolStripMenuItem();
            过滤10ms以内的消息ToolStripMenuItem=new ToolStripMenuItem();
            过滤100ms以内的消息ToolStripMenuItem=new ToolStripMenuItem();
            重新读取IP地址组ToolStripMenuItem=new ToolStripMenuItem();
            tabControl1=new TabControl();
            tabPage1=new TabPage();
            tabPage2=new TabPage();
            清空所有记录ToolStripMenuItem=new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 中断ping请求ToolStripMenuItem,过滤10ms以内的消息ToolStripMenuItem,过滤100ms以内的消息ToolStripMenuItem,重新读取IP地址组ToolStripMenuItem,清空所有记录ToolStripMenuItem });
            menuStrip1.Location=new Point(0,0);
            menuStrip1.Name="menuStrip1";
            menuStrip1.Size=new Size(652,25);
            menuStrip1.TabIndex=0;
            menuStrip1.Text="menuStrip1";
            // 
            // 中断ping请求ToolStripMenuItem
            // 
            中断ping请求ToolStripMenuItem.Name="中断ping请求ToolStripMenuItem";
            中断ping请求ToolStripMenuItem.Size=new Size(94,21);
            中断ping请求ToolStripMenuItem.Text="中断ping请求";
            中断ping请求ToolStripMenuItem.Click+=中断ping请求ToolStripMenuItem_Click;
            // 
            // 过滤10ms以内的消息ToolStripMenuItem
            // 
            过滤10ms以内的消息ToolStripMenuItem.Name="过滤10ms以内的消息ToolStripMenuItem";
            过滤10ms以内的消息ToolStripMenuItem.Size=new Size(135,21);
            过滤10ms以内的消息ToolStripMenuItem.Text="过滤10ms以内的消息";
            过滤10ms以内的消息ToolStripMenuItem.Click+=过滤10ms以内的消息ToolStripMenuItem_Click;
            // 
            // 过滤100ms以内的消息ToolStripMenuItem
            // 
            过滤100ms以内的消息ToolStripMenuItem.Name="过滤100ms以内的消息ToolStripMenuItem";
            过滤100ms以内的消息ToolStripMenuItem.Size=new Size(142,21);
            过滤100ms以内的消息ToolStripMenuItem.Text="过滤100ms以内的消息";
            过滤100ms以内的消息ToolStripMenuItem.Click+=过滤100ms以内的消息ToolStripMenuItem_Click;
            // 
            // 重新读取IP地址组ToolStripMenuItem
            // 
            重新读取IP地址组ToolStripMenuItem.Name="重新读取IP地址组ToolStripMenuItem";
            重新读取IP地址组ToolStripMenuItem.Size=new Size(115,21);
            重新读取IP地址组ToolStripMenuItem.Text="重新读取IP地址组";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock=DockStyle.Fill;
            tabControl1.Location=new Point(0,25);
            tabControl1.Name="tabControl1";
            tabControl1.SelectedIndex=0;
            tabControl1.Size=new Size(652,343);
            tabControl1.TabIndex=1;
            // 
            // tabPage1
            // 
            tabPage1.Location=new Point(4,26);
            tabPage1.Name="tabPage1";
            tabPage1.Padding=new Padding(3);
            tabPage1.Size=new Size(644,313);
            tabPage1.TabIndex=0;
            tabPage1.Text="tabPage1";
            tabPage1.UseVisualStyleBackColor=true;
            // 
            // tabPage2
            // 
            tabPage2.Location=new Point(4,26);
            tabPage2.Name="tabPage2";
            tabPage2.Padding=new Padding(3);
            tabPage2.Size=new Size(544,313);
            tabPage2.TabIndex=1;
            tabPage2.Text="tabPage2";
            tabPage2.UseVisualStyleBackColor=true;
            // 
            // 清空所有记录ToolStripMenuItem
            // 
            清空所有记录ToolStripMenuItem.Name="清空所有记录ToolStripMenuItem";
            清空所有记录ToolStripMenuItem.Size=new Size(92,21);
            清空所有记录ToolStripMenuItem.Text="清空所有记录";
            清空所有记录ToolStripMenuItem.Click+=清空所有记录ToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(652,368);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip=menuStrip1;
            Name="Form1";
            Text="网络连接监视器";
            Load+=MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private MenuStrip menuStrip1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ToolStripMenuItem 中断ping请求ToolStripMenuItem;
        private ToolStripMenuItem 过滤10ms以内的消息ToolStripMenuItem;
        private ToolStripMenuItem 过滤100ms以内的消息ToolStripMenuItem;
        private ToolStripMenuItem 重新读取IP地址组ToolStripMenuItem;
        private ToolStripMenuItem 清空所有记录ToolStripMenuItem;
    }
}