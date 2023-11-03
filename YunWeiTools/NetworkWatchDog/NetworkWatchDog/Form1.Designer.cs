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
            重新读取IP地址组ToolStripMenuItem=new ToolStripMenuItem();
            清空所有记录ToolStripMenuItem=new ToolStripMenuItem();
            splitContainer1=new SplitContainer();
            listBox1=new ListBox();
            tabcontrol1=new TabControl();
            tabPage1=new TabPage();
            richTextBox2=new RichTextBox();
            tabPage2=new TabPage();
            richTextBox1=new RichTextBox();
            tabPage3=new TabPage();
            richTextBox3=new RichTextBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabcontrol1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 重新读取IP地址组ToolStripMenuItem,清空所有记录ToolStripMenuItem });
            menuStrip1.Location=new Point(0,0);
            menuStrip1.Name="menuStrip1";
            menuStrip1.Size=new Size(664,25);
            menuStrip1.TabIndex=0;
            menuStrip1.Text="menuStrip1";
            // 
            // 重新读取IP地址组ToolStripMenuItem
            // 
            重新读取IP地址组ToolStripMenuItem.Name="重新读取IP地址组ToolStripMenuItem";
            重新读取IP地址组ToolStripMenuItem.Size=new Size(116,21);
            重新读取IP地址组ToolStripMenuItem.Text="重新读取配置文件";
            重新读取IP地址组ToolStripMenuItem.Click+=ReReadIpConfigToolStripMenuItem_Click;
            // 
            // 清空所有记录ToolStripMenuItem
            // 
            清空所有记录ToolStripMenuItem.Name="清空所有记录ToolStripMenuItem";
            清空所有记录ToolStripMenuItem.Size=new Size(92,21);
            清空所有记录ToolStripMenuItem.Text="清空所有记录";
            清空所有记录ToolStripMenuItem.Click+=清空所有记录ToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock=DockStyle.Fill;
            splitContainer1.Location=new Point(0,25);
            splitContainer1.Name="splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabcontrol1);
            splitContainer1.Size=new Size(664,437);
            splitContainer1.SplitterDistance=101;
            splitContainer1.TabIndex=2;
            // 
            // listBox1
            // 
            listBox1.Dock=DockStyle.Fill;
            listBox1.FormattingEnabled=true;
            listBox1.ItemHeight=17;
            listBox1.Location=new Point(0,0);
            listBox1.Name="listBox1";
            listBox1.Size=new Size(101,437);
            listBox1.TabIndex=0;
            // 
            // tabcontrol1
            // 
            tabcontrol1.Controls.Add(tabPage1);
            tabcontrol1.Controls.Add(tabPage2);
            tabcontrol1.Controls.Add(tabPage3);
            tabcontrol1.Dock=DockStyle.Fill;
            tabcontrol1.Location=new Point(0,0);
            tabcontrol1.Name="tabcontrol1";
            tabcontrol1.SelectedIndex=0;
            tabcontrol1.Size=new Size(559,437);
            tabcontrol1.TabIndex=1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(richTextBox2);
            tabPage1.Location=new Point(4,26);
            tabPage1.Name="tabPage1";
            tabPage1.Padding=new Padding(3);
            tabPage1.Size=new Size(551,407);
            tabPage1.TabIndex=0;
            tabPage1.Text="实时连接信息";
            tabPage1.UseVisualStyleBackColor=true;
            // 
            // richTextBox2
            // 
            richTextBox2.Dock=DockStyle.Fill;
            richTextBox2.Location=new Point(3,3);
            richTextBox2.Name="richTextBox2";
            richTextBox2.Size=new Size(545,401);
            richTextBox2.TabIndex=0;
            richTextBox2.Text="";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Location=new Point(4,26);
            tabPage2.Name="tabPage2";
            tabPage2.Padding=new Padding(3);
            tabPage2.Size=new Size(551,407);
            tabPage2.TabIndex=1;
            tabPage2.Text="异常信息";
            tabPage2.UseVisualStyleBackColor=true;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock=DockStyle.Fill;
            richTextBox1.Location=new Point(3,3);
            richTextBox1.Name="richTextBox1";
            richTextBox1.Size=new Size(545,401);
            richTextBox1.TabIndex=0;
            richTextBox1.Text="";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(richTextBox3);
            tabPage3.Location=new Point(4,26);
            tabPage3.Name="tabPage3";
            tabPage3.Padding=new Padding(3);
            tabPage3.Size=new Size(551,407);
            tabPage3.TabIndex=2;
            tabPage3.Text="信息展示";
            tabPage3.UseVisualStyleBackColor=true;
            // 
            // richTextBox3
            // 
            richTextBox3.Dock=DockStyle.Fill;
            richTextBox3.Location=new Point(3,3);
            richTextBox3.Name="richTextBox3";
            richTextBox3.Size=new Size(545,401);
            richTextBox3.TabIndex=0;
            richTextBox3.Text="";
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(664,462);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip=menuStrip1;
            Name="Form1";
            Text="网络连接监视器";
            Load+=MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabcontrol1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 重新读取IP地址组ToolStripMenuItem;
        private ToolStripMenuItem 清空所有记录ToolStripMenuItem;
        private SplitContainer splitContainer1;
        private ListBox listBox1;
        private RichTextBox richTextBox1;
        private TabControl tabcontrol1;
        private TabPage tabPage1;
        private RichTextBox richTextBox2;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private RichTextBox richTextBox3;
    }
}