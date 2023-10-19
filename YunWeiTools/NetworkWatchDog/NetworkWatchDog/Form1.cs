using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;
namespace NetworkWatchDog
{
    //todo api报警
    public partial class Form1:Form
    {
        private const int MaxThreads = 100; // 最大线程数
        private List<string> ipTextBoxes;

        private List<string> ListiningIP;
        int RoundtripTime = 0;
        int timeout = 5000;//ms

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new List<string>();
            ListiningIP=new();
        }

        private void pingConnecting()
        {
            // 创建线程池
            ThreadPool.SetMaxThreads(MaxThreads,MaxThreads);

            // 启动ping线程
            foreach(string ipAddress in ipTextBoxes)
            {
                if(ListiningIP.Contains(ipAddress))
                {

                }
                else
                {
                    ListiningIP.Add(ipAddress);
                    Thread thread = new(() => Ping_PingCompleted(ipAddress));
                    thread.Start();
                }
            }
        }

        private void MainForm_Load(object sender,EventArgs e)
        {
            try
            {
                var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("config.json",optional: true,reloadOnChange: true);
                var config = configBuilder.Build();
                var ipGroupSection = config.GetSection("ipgroup");
                var ipGroupChildren = ipGroupSection.GetChildren();

#nullable disable //trycatch 判断是否为空
                string[] ipAddresses = ipGroupChildren.Select(x => x.Value).ToArray();
                RoundtripTime=int.Parse(config.GetSection("RoundtripTime").Value);
                timeout=int.Parse(config.GetSection("TimeOut").Value);
#nullable enable

                ipTextBoxes=ipAddresses.ToList();
                listBox1.Items.Clear();
                foreach(var ipAddress in ipTextBoxes)
                {
                    listBox1.Items.Add(ipAddress);
                }
                this.Text=$"网络连接监视器 -{RoundtripTime}ms";

                pingConnecting();
            }
            catch
            {
                MessageBox.Show(Directory.GetCurrentDirectory()+"config文件缺失");
            }
        }

        private void Ping_PingCompleted(object ipadress)
        {
            while(true)
            {
                string ip = (string)ipadress;
                Ping ping = new();

                try
                {
                    PingReply reply = ping.Send(ip,timeout);

                    if(reply.Status==IPStatus.Success)
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip}: 字节={reply.Buffer.Length} 时间={reply.RoundtripTime}ms TTL={reply.Options?.Ttl}";

                        UpdateUI(str,reply.RoundtripTime);
                    }
                    else
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip}: {reply.Status}";
                        // 主机不可达
                        UpdateUI(str,-1);
                    }
                }
                catch(Exception ex)
                {
                    var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip}: {ex.Message}";
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
                if(outtime>=RoundtripTime||outtime==-1)
                {
                    richTextBox1.AppendText(message+Environment.NewLine);
                    File.AppendAllText(Directory.GetCurrentDirectory()+"/error.log",message+"\r\n");

                    //url report
                }
                richTextBox2.AppendText(message+Environment.NewLine);
            }
        }

        private void 清空所有记录ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            richTextBox1.Text="";
            richTextBox2.Text="";
        }

        private void ReReadIpConfigToolStripMenuItem_Click(object sender,EventArgs e)
        {
            MainForm_Load(null,null);
        }
    }
}