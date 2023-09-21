using System.Net.NetworkInformation;

namespace NetworkWatchDog
{
    public partial class Form1:Form
    {
        private List<IpAddress> ips = new List<IpAddress>();
        int timetrip = 100;
        public Form1()
        {
            InitializeComponent();

            ips.Add(new IpAddress("192.168.0.1"));
            ips.Add(new IpAddress("192.168.10.214"));
            ips.Add(new IpAddress("www.baidu.com"));

            this.Text=$"�ѿ�ʼ��ip���ʵʱ���,��ǰ�����Ǵ���{timetrip}ms";

            this.tabControl1.Controls.Clear();

            TabPageInit();
        }

        private void TabPageInit()
        {
            foreach(var a in ips)
            {
                TabPage tb = new();
                tb.Text=a.Ip;
                var rtb = new RichTextBox() { Name=a.Ip,Dock=DockStyle.Fill };
                a.rtb=rtb;
                Ping p = new Ping();
                p.PingCompleted+=PingCompletedCallback; // ���ûص�����

                tb.Controls.Add(rtb);
                tabControl1.Controls.Add(tb);

                while(true)
                {
                    p.SendAsync(a.Ip,1000); // ����ping����
                    // �ȴ�1��
                    System.Threading.Thread.Sleep(1000);
                }

            }
        }

        private void timer1_Tick(object sender,EventArgs e)
        {
            foreach(IpAddress ip in ips)
            {
                try
                {
                    //���ڲ�ͬip�����̵߳�����  threadpool���й���
                    ip.rtb.AppendText(ip.GetCheck(timetrip));
                }
                catch
                {
                    //δ���ö�Ӧip
                }
            }
        }

        public void PingCompletedCallback(object sender,PingCompletedEventArgs e)
        {
            if(e.Error!=null)
            {
                Console.WriteLine("Ping failed: "+e.Error.Message);
            }
            else if(e.Cancelled)
            {
                Console.WriteLine("Ping cancelled.");
            }
            else
            {
                PingReply reply = e.Reply;
                var a = ips.FindLast(x => x.Ip==reply.Address.ToString());
                a.rtb.AppendText(a.GetCheck(1));
            }
        }

    }

    public class IpAddress
    {
        public IpAddress(string ip,int? port)
        {
            Ip=ip;
            Port=port;
        }
        public IpAddress(string ip)
        {
            Ip=ip;
        }
        public RichTextBox rtb
        {
            get; set;
        }

        public string GetCheck(int timetrip)
        {
            string host = string.Empty;
            host=Ip;
            if(Port!=null)
            {
                host+=":"+Port;
            }

            Ping pingSender = new();
            PingReply reply = pingSender.Send(host);
            pingSender.Dispose();

            if(reply.Status==IPStatus.Success)
            {
                if(reply.RoundtripTime<=timetrip)
                {
                    return "";// $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")}ping�ɹ�,�ӳ�Ϊ{reply.RoundtripTime}ms\r\n";
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")+$"Ping�ɹ�,�ӳٹ���,����{timetrip}ms  Ϊ{reply.RoundtripTime}ms "+"\r\n";
                }
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")+"Pingʧ�ܡ�"+"\r\n";
            }
        }


        public string Ip
        {
            get; set;
        }

        public int? Port
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        } = false;
    }
}