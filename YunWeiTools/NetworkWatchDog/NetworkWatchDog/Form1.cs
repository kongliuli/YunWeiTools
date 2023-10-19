using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;
namespace NetworkWatchDog
{
    public partial class Form1:Form
    {
        private const int MaxThreads = 100; // 最大线程数
        private Dictionary<string,RichTextBox> ipTextBoxes;

        int RoundtripTime = 0;

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new Dictionary<string,RichTextBox>();
        }

        private void pingConnecting()
        {
            // 创建线程池
            ThreadPool.SetMaxThreads(MaxThreads,MaxThreads);

            // 启动ping线程
            foreach(var ipAddress in ipTextBoxes)
            {
                Thread thread = new(() => Ping_PingCompleted(ipAddress.Key));
                thread.Start();
                //ThreadPool.QueueUserWorkItem(Ping_PingCompleted,ipAddress.Key);
            }
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


            foreach(string ipAddress in ipAddresses)
            {
                ipTextBoxes[ipAddress]=null;
            }

            this.Text=$"网络连接监视器 -{RoundtripTime}ms";

            pingConnecting();
        }

        private void Ping_PingCompleted(object ipadress)
        {
            while(true)
            {
                string ip = (string)ipadress;
                Ping ping = new();

                try
                {
                    PingReply reply = ping.Send(ip,5000);

                    if(reply.Status==IPStatus.Success)
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --来自 {ip} 的回复: 字节={reply.Buffer.Length} 时间={reply.RoundtripTime}ms TTL={reply.Options.Ttl}";

                        UpdateUI(str,reply.RoundtripTime);
                    }
                    else
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --来自 {ip} 的回复: {reply.Status}";
                        // 主机不可达
                        UpdateUI(str,-1);
                    }
                }
                catch(Exception ex)
                {
                    var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --来自 {ip} 的回复: {ex.Message}";
                    // 主机不可达
                    UpdateUI(str,-1);
                }
                Thread.Sleep(1000); // 等待1秒后再次执行ping命令
            }
        }

        private void UpdateUI(string message,long outtime)
        {

            if(InvokeRequired)
            {
                Invoke(new Action<string,long>(UpdateUI),message,outtime);
            }
            else
            {
                if(outtime>=RoundtripTime)
                {
                    richTextBox1.AppendText("Connect   "+message+Environment.NewLine);
                }
                if(outtime==-1)
                {
                    richTextBox1.AppendText("ERROR     "+message+Environment.NewLine);
                }
            }
        }


        private void 过滤10ms以内的消息ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=10;
            this.Text=$"网络连接监视器 -计时器开启 -{RoundtripTime}ms";
        }

        private void 过滤100ms以内的消息ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=100;
            this.Text=$"网络连接监视器 -计时器开启 -{RoundtripTime}ms";
        }

        private void 中断ping请求ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            timer1.Stop();
            this.Text=$"网络连接监视器 -计时器关闭 -{RoundtripTime}ms";
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