using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;

using Timer = System.Windows.Forms.Timer;
namespace NetworkWatchDog
{
    //todo ip�Ͷ�Ӧҳ��İ󶨹�ϵ
    //todo �����ӵı���
    public partial class Form1:Form
    {
        private Dictionary<string,RichTextBox> ipTextBoxes;
        private Timer timer;
        int RoundtripTime = 0;

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new Dictionary<string,RichTextBox>();
            timer=new Timer();
            timer.Interval=1000; // ���ö�ʱ�����Ϊ5��
            timer.Tick+=Timer_Tick;
        }
        private void MainForm_Load(object sender,EventArgs e)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json",optional: true,reloadOnChange: true);
            var config = configBuilder.Build();
            var ipGroupSection = config.GetSection("ipgroup");
            var ipGroupChildren = ipGroupSection.GetChildren();
            string[] ipAddresses = ipGroupChildren.Select(x => x.Value).ToArray();
            RoundtripTime=int.Parse(config.GetSection("RoundtripTime").Value);

            tabControl1.TabPages.Clear();

            foreach(string ipAddress in ipAddresses)
            {
                TabPage tabPage = new(ipAddress);
                RichTextBox richTextBox = new();
                richTextBox.Dock=DockStyle.Fill;
                richTextBox.SelectionStart=richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
                tabPage.Controls.Add(richTextBox);
                tabControl1.TabPages.Add(tabPage);
                ipTextBoxes[ipAddress]=richTextBox;
            }
            timer.Start(); // ������ʱ��

        }

        private void Timer_Tick(object sender,EventArgs e)
        {
            foreach(string ipAddress in ipTextBoxes.Keys)
            {
                PingState pingState = new PingState(ipAddress,Ping_PingCompleted);
                Ping ping = new Ping();
                ping.PingCompleted+=Ping_PingCompleted;
                ping.SendAsync(ipAddress,5000,pingState);
            }
        }

        private void Ping_PingCompleted(object sender,PingCompletedEventArgs e)
        {
            PingState pingState = (PingState)e.UserState; // ��ȡ���ݵ�PingState����
            string ipAddress = pingState.IpAddress; // ��ȡIP��ַ

            if(e.Cancelled)
            {
                // ʹ��ipAddress���д���
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} ���ӱ�ȡ��");
            }
            else if(e.Error!=null)
            {
                // ʹ��ipAddress���д���
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} ����ʧ��: {e.Error.Message}\n");
            }
            else
            {
                // ʹ��ipAddress���д���
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                if(RoundtripTime<e.Reply.RoundtripTime)
                {
                    richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} �ӳ�: {e.Reply.RoundtripTime}ms\n");
                }
            }
        }

        private void ����10ms���ڵ���ϢToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=10;
        }

        private void ����100ms���ڵ���ϢToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=100;
        }

        private void �ж�ping����ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            timer1.Stop();
        }

        private void ������м�¼ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            foreach(var a in ipTextBoxes)
            {
                a.Value.Text="";
            }
        }
    }
    public class PingState
    {
        public string IpAddress
        {
            get; set;
        }
        public Action<object,PingCompletedEventArgs> Callback
        {
            get; set;
        }

        public PingState(string ipAddress,Action<object,PingCompletedEventArgs> callback)
        {
            IpAddress=ipAddress;
            Callback=callback;
        }
    }
}