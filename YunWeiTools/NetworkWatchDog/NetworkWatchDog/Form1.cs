using System.Net.NetworkInformation;

namespace NetworkWatchDog
{
    //todo ip和对应页面的绑定关系
    //todo 长连接的保持
    public partial class Form1:Form
    {
        private List<IpAddress> ips = new List<IpAddress>();
        int timetrip = 100;
        public Form1()
        {
            InitializeComponent();

            ips.Add(new IpAddress("192.168.10.214"));
            ips.Add(new IpAddress("www.baidu.com"));

            this.Text=$"已开始对ip组的实时监控,当前规则是大于{timetrip}ms";

            this.tabControl1.Controls.Clear();

            //TabPageInit();
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
                p.PingCompleted+=PingCompletedCallback; // 设置回调函数

                tb.Controls.Add(rtb);
                tabControl1.Controls.Add(tb);

                // while(true)
                // {
                p.SendAsync(a.Ip,1000); // 发送ping请求
                // }
            }
        }

        private void timer1_Tick(object sender,EventArgs e)
        {

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
                a.rtb.AppendText(a.GetCheck(100));
            }
        }

        private void 开启pingToolStripMenuItem_Click(object sender,EventArgs e)
        {
            TabPageInit();
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
                    return "";// $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")}ping成功,延迟为{reply.RoundtripTime}ms\r\n";
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")+$"Ping成功,延迟过高,超过{timetrip}ms  为{reply.RoundtripTime}ms "+"\r\n";
                }
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ")+"Ping失败。"+"\r\n";
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