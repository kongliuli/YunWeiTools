namespace GetImageGroupByAnyData
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
            tabControl1=new TabControl();
            tabPage1=new TabPage();
            tabPage2=new TabPage();
            richTextBox1=new RichTextBox();
            listBox1=new ListBox();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock=DockStyle.Right;
            tabControl1.Location=new Point(179,0);
            tabControl1.Name="tabControl1";
            tabControl1.SelectedIndex=0;
            tabControl1.Size=new Size(822,512);
            tabControl1.TabIndex=0;
            // 
            // tabPage1
            // 
            tabPage1.Location=new Point(4,26);
            tabPage1.Name="tabPage1";
            tabPage1.Padding=new Padding(3);
            tabPage1.Size=new Size(814,482);
            tabPage1.TabIndex=0;
            tabPage1.Text="绑定界面";
            tabPage1.UseVisualStyleBackColor=true;
            tabPage1.DragDrop+=Form1_DragDrop;
            tabPage1.DragEnter+=Form1_DragEnter;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Location=new Point(4,26);
            tabPage2.Name="tabPage2";
            tabPage2.Padding=new Padding(3);
            tabPage2.Size=new Size(814,482);
            tabPage2.TabIndex=1;
            tabPage2.Text="信息展示";
            tabPage2.UseVisualStyleBackColor=true;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock=DockStyle.Fill;
            richTextBox1.Location=new Point(3,3);
            richTextBox1.Name="richTextBox1";
            richTextBox1.Size=new Size(808,476);
            richTextBox1.TabIndex=0;
            richTextBox1.Text="";
            // 
            // listBox1
            // 
            listBox1.Dock=DockStyle.Fill;
            listBox1.FormattingEnabled=true;
            listBox1.ItemHeight=17;
            listBox1.Location=new Point(0,0);
            listBox1.Name="listBox1";
            listBox1.Size=new Size(179,512);
            listBox1.TabIndex=1;
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(1001,512);
            Controls.Add(listBox1);
            Controls.Add(tabControl1);
            Name="Form1";
            Text="Form1";
            DragDrop+=Form1_DragDrop;
            DragEnter+=Form1_DragEnter;
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListBox listBox1;
        private RichTextBox richTextBox1;
    }
}