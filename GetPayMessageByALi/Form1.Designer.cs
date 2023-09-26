namespace GetPayMessageByALi
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
            panel1=new Panel();
            weeks=new Label();
            label1=new Label();
            comboBox1=new ComboBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(weeks);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(comboBox1);
            panel1.Dock=DockStyle.Top;
            panel1.Location=new Point(0,0);
            panel1.Name="panel1";
            panel1.Size=new Size(653,128);
            panel1.TabIndex=0;
            // 
            // weeks
            // 
            weeks.AutoSize=true;
            weeks.Location=new Point(192,15);
            weeks.Name="weeks";
            weeks.Size=new Size(43,17);
            weeks.TabIndex=2;
            weeks.Text="label2";
            // 
            // label1
            // 
            label1.AutoSize=true;
            label1.Location=new Point(12,15);
            label1.Name="label1";
            label1.Size=new Size(80,17);
            label1.TabIndex=1;
            label1.Text="选择计费周期";
            // 
            // comboBox1
            // 
            comboBox1.AllowDrop=true;
            comboBox1.FormattingEnabled=true;
            comboBox1.Location=new Point(98,12);
            comboBox1.Name="comboBox1";
            comboBox1.Size=new Size(79,25);
            comboBox1.TabIndex=0;
            comboBox1.SelectedIndexChanged+=comboBox1_SelectedIndexChanged;
            // 
            // Form1
            // 
            AllowDrop=true;
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(653,355);
            Controls.Add(panel1);
            Name="Form1";
            Text="Form1";
            DragDrop+=Form1_DragDrop;
            DragEnter+=Form1_DragEnter;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label weeks;
        private Label label1;
        private ComboBox comboBox1;
    }
}