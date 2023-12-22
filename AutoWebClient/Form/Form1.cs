using AWCForm.WebUsercontrols;

using CefSharp;
using CefSharp.WinForms;

namespace AWCForm
{
    public partial class Form1:Form
    {

        public string Uri = "";
        public Form1()
        {
            InitializeComponent();

            CefSettings settings = new()
            {
                CachePath="C:\\Users\\HP\\AppData\\Local\\Microsoft\\Edge Dev\\User Data\\Default\\Cache"
            };
            Cef.Initialize(settings);


            TabInit();
        }

        private void TabInit()
        {
            //酷匠地平线加载5个阿里云aws界面 kujiang 花苼书城 花苼书城2 红豆书城1 红豆书城2
            tabControl1.TabPages.Clear();
            TabPage T1 = new()
            {
                Text="kujiang"
            };
            T1.Controls.Add(new alibabaWebControl("kujiang","YO@JQM!lsiS&ILo$") { Dock=DockStyle.Fill });
            tabControl1.TabPages.Add(T1);
            //TabPage T2 = new();
            //T2.Text="花苼书城";
            //T2.Controls.Add(new alibabaWebControl() { Dock=DockStyle.Fill });
            //tabControl1.TabPages.Add(T2);
            //TabPage T3 = new();
            //T3.Text="花苼书城2";
            //T3.Controls.Add(new alibabaWebControl() { Dock=DockStyle.Fill });
            //tabControl1.TabPages.Add(T3);
            //TabPage T4 = new();
            //T4.Text="红豆书城1";
            //T4.Controls.Add(new alibabaWebControl() { Dock=DockStyle.Fill });
            //tabControl1.TabPages.Add(T4);
            //TabPage T5 = new();
            //T5.Text="红豆书城2";
            //T5.Controls.Add(new alibabaWebControl() { Dock=DockStyle.Fill });
            //tabControl1.TabPages.Add(T5);
        }



    }
}