using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;
namespace NetworkWatchDog
{
    //todo api����
    public partial class Form1:Form
    {
        private const int MaxThreads = 100; // ����߳���
        private List<string> ipTextBoxes;

        private List<string> ListiningIP;
        int RoundtripTime = 0;

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new List<string>();
        }

        private void pingConnecting()
        {
            // �����̳߳�
            ThreadPool.SetMaxThreads(MaxThreads,MaxThreads);

            // ����ping�߳�
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
                string[] ipAddresses = ipGroupChildren.Select(x => x.Value).ToArray();
                RoundtripTime=int.Parse(config.GetSection("RoundtripTime").Value);

                ipTextBoxes=ipAddresses.ToList();
                listBox1.Items.Clear();
                foreach(var ipAddress in ipTextBoxes)
                {
                    listBox1.Items.Add(ipAddress);
                }
                this.Text=$"�������Ӽ����� -{RoundtripTime}ms";

                pingConnecting();
            }
            catch
            {
                MessageBox.Show("config�ļ�ȱʧ");
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
                    PingReply reply = ping.Send(ip,5000);

                    if(reply.Status==IPStatus.Success)
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --���� {ip} �Ļظ�: �ֽ�={reply.Buffer.Length} ʱ��={reply.RoundtripTime}ms TTL={reply.Options.Ttl}";

                        UpdateUI(str,reply.RoundtripTime);
                    }
                    else
                    {
                        var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --���� {ip} �Ļظ�: {reply.Status}";
                        // �������ɴ�
                        UpdateUI(str,-1);
                    }
                }
                catch(Exception ex)
                {
                    var str = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} --���� {ip} �Ļظ�: {ex.Message}";
                    // �������ɴ�
                    UpdateUI(str,-1);
                }
                Thread.Sleep(1000); // �ȴ�1����ٴ�ִ��ping����
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

        private void ������м�¼ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            richTextBox1.Text="";
            richTextBox2.Text="";
        }

        private void ReReadIpConfigToolStripMenuItem_Click(object sender,EventArgs e)
        {
            MainForm_Load(null,null);
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