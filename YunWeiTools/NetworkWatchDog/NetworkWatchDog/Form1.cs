using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;

using Timer = System.Windows.Forms.Timer;
namespace NetworkWatchDog
{
    //todo ip和对应页面的绑定关系
    //todo 长连接的保持
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
            timer.Interval=1000; // 设置定时器间隔为5秒
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
            timer.Start(); // 启动定时器

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
            PingState pingState = (PingState)e.UserState; // 获取传递的PingState对象
            string ipAddress = pingState.IpAddress; // 获取IP地址

            if(e.Cancelled)
            {
                // 使用ipAddress进行处理
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} 连接被取消");
            }
            else if(e.Error!=null)
            {
                // 使用ipAddress进行处理
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} 连接失败: {e.Error.Message}\n");
            }
            else
            {
                // 使用ipAddress进行处理
                RichTextBox richTextBox = ipTextBoxes[ipAddress];
                if(RoundtripTime<e.Reply.RoundtripTime)
                {
                    richTextBox.AppendText($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  {ipAddress} 延迟: {e.Reply.RoundtripTime}ms\n");
                }
            }
        }

        private void 过滤10ms以内的消息ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=10;
        }

        private void 过滤100ms以内的消息ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=100;
        }

        private void 中断ping请求ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            timer1.Stop();
        }

        private void 清空所有记录ToolStripMenuItem_Click(object sender,EventArgs e)
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