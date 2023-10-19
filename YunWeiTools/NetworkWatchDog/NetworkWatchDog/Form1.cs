using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;
namespace NetworkWatchDog
{
    //todo api报警
    public partial class Form1:Form
    {
        private const int MaxThreads = 100; // 最大线程数
        private List<Ipaddress> ipTextBoxes;

        private List<Ipaddress> ListiningIP;
        int IntranettripTime = 10, ExternaltripTime = 150;
        int PingTimer = 10, BuffurMaxLine = 10000;
        int timeout = 5000;//ms

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new List<Ipaddress>();
            ListiningIP=new();
        }

        private void pingConnecting()
        {
            // 创建线程池
            ThreadPool.SetMaxThreads(MaxThreads,MaxThreads);

            // 启动ping线程
            foreach(Ipaddress ipAddress in ipTextBoxes)
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
                var IntranetGroup = config.GetSection("IntranetGroup");
                var IntranetGroupChildren = IntranetGroup.GetChildren();

                var ExternalNetworkGroup = config.GetSection("ExternalNetworkGroup");
                var ExternalNetworkChildren = ExternalNetworkGroup.GetChildren();

#nullable disable //trycatch 判断是否为空
                string[] IntranetAddress = IntranetGroupChildren.Select(x => x.Value).ToArray();
                string[] ExternalNetworkAddresses = ExternalNetworkChildren.Select(x => x.Value).ToArray();
                IntranettripTime=int.Parse(config.GetSection("IntranettripTime").Value);
                ExternaltripTime=int.Parse(config.GetSection("ExternaltripTime").Value);
                timeout=int.Parse(config.GetSection("TimeOut").Value);


                PingTimer=int.Parse(config.GetSection("PingTimer").Value);
                BuffurMaxLine=int.Parse(config.GetSection("BuffurMaxLine").Value);
#nullable enable

                ipTextBoxes.Clear();
                ipTextBoxes=(from ip in IntranetAddress select new Ipaddress() { ipinfo=ip.ToString(),isintra=true }).ToList();
                ipTextBoxes.AddRange((from ip in ExternalNetworkAddresses select new Ipaddress() { ipinfo=ip.ToLower(),isintra=false }).ToList());


                listBox1.Items.Clear();
                foreach(var ip in ipTextBoxes)
                {
                    listBox1.Items.Add(ip.ipinfo);
                }


                this.Text=$"网络连接监视器 --内/外网ip延迟 --{IntranettripTime}/{ExternaltripTime}ms";

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
                Ipaddress ip = (Ipaddress)ipadress;
                Ping ping = new();

                try
                {
                    PingReply reply = ping.Send(ip.ipinfo,timeout);
                    if(reply.Status==IPStatus.Success)
                    {
                        UpdateUI($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip.ipinfo}: 字节={reply.Buffer.Length} 时间={reply.RoundtripTime}ms TTL={reply.Options?.Ttl}",reply.RoundtripTime,ip.isintra);
                    }
                    else
                    {
                        //不成功汇总
                        UpdateUI($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip.ipinfo}: {reply.Status}",-1);
                    }
                }
                catch(Exception ex)
                {
                    //异常报告
                    UpdateUI($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --{ip.ipinfo}: {ex.Message}",-1);
                }
                Thread.Sleep(PingTimer); // 等待1秒后再次执行ping命令
            }
        }

        private void UpdateUI(string message,long outtime,bool istra = true)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<string,long,bool>(UpdateUI),message,outtime,istra);
            }
            else
            {
                int triptime = istra ? IntranettripTime : ExternaltripTime;
                if(richTextBox1.Lines.Count()>BuffurMaxLine)
                {
                    var a = richTextBox1.Text.Remove(richTextBox1.GetFirstCharIndexFromLine(100));

                    richTextBox1.Text=a;
                    richTextBox1.Refresh();
                }
                if(richTextBox2.Lines.Count()>BuffurMaxLine)
                {
                    var a = richTextBox2.Text.Remove(0,richTextBox2.GetFirstCharIndexFromLine(100));

                    richTextBox2.Text=a;
                    richTextBox2.Refresh();
                }
                Task.Run(() =>
                {
                    // 执行长时间运行的操作
                    if(outtime>=triptime||outtime==-1)
                    {
                        Invoke((Action)(() =>
                        {
                            richTextBox1.AppendText(message+Environment.NewLine);
                            File.AppendAllText(Directory.GetCurrentDirectory()+"/error.log",message+"\r\n");
                        }));
                    }

                    Invoke((Action)(() =>
                    {
                        richTextBox2.AppendText(message+Environment.NewLine);
                    }));
                });
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

    public class Ipaddress
    {
        public string ipinfo
        {
            get; set;
        }

        public bool isintra
        {
            get; set;
        }
    }
}