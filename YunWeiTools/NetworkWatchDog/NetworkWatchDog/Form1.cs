using System.Net.NetworkInformation;

using Microsoft.Extensions.Configuration;
namespace NetworkWatchDog
{
    public partial class Form1:Form
    {
        private const int MaxThreads = 100; // ����߳���
        private Dictionary<string,RichTextBox> ipTextBoxes;

        int RoundtripTime = 0;

        public Form1()
        {
            InitializeComponent();
            ipTextBoxes=new Dictionary<string,RichTextBox>();
        }

        private void pingConnecting()
        {
            // �����̳߳�
            ThreadPool.SetMaxThreads(MaxThreads,MaxThreads);

            // ����ping�߳�
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

            this.Text=$"�������Ӽ����� -{RoundtripTime}ms";

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


        private void ����10ms���ڵ���ϢToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=10;
            this.Text=$"�������Ӽ����� -��ʱ������ -{RoundtripTime}ms";
        }

        private void ����100ms���ڵ���ϢToolStripMenuItem_Click(object sender,EventArgs e)
        {
            RoundtripTime=100;
            this.Text=$"�������Ӽ����� -��ʱ������ -{RoundtripTime}ms";
        }

        private void �ж�ping����ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            timer1.Stop();
            this.Text=$"�������Ӽ����� -��ʱ���ر� -{RoundtripTime}ms";
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