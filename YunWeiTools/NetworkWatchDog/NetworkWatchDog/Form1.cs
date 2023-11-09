using System.Net.NetworkInformation;

using Newtonsoft.Json;

namespace NetworkWatchDog
{
    //���α���ʱ��
    //wpf�ع�
    //mtr����
    public partial class Form1:Form
    {
        public Configuration configura = new();

        private List<string> ListiningIP;

        public Form1()
        {
            InitializeComponent();

            ListiningIP=new();

            InitReport();
        }

        private void InitReport()
        {

        }

        private async void SendReportToDingDing(string msg)
        {
            using HttpClient client = new();
            // ����POST���������
            var postData = new FormUrlEncodedContent(new[]
            {
                //����Ⱥ������Ϊϵͳ��
                new KeyValuePair<string, string>("grounp", "xitongzu"),
                //���ñ���ʱ����at��ϵ��
                new KeyValuePair<string, string>("at", "18751936236,17361914131"),
                //������Ϣ
                new KeyValuePair<string, string>("message", msg)
            });
            try
            {
                // ����POST����
                HttpResponseMessage response = await client.PostAsync("https://www.kujiang.com/Tool/dingding",postData);

                if(response.IsSuccessStatusCode)
                {
                    // ��ȡ��Ӧ����
                    string responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                }
            }
            catch(Exception e)
            {

            }

        }

        private void pingConnecting()
        {
            foreach(var ipAddress in configura.baseSetting.NetworkGroup.Where(ip => !ListiningIP.Contains(ip.Ipconfig)))
            {
                // ����ping�߳�
                ListiningIP.Add(ipAddress.Ipconfig);
                new Thread(() => Ping_PingCompleted(ipAddress.Ipconfig)).Start();
            }
        }

        private void MainForm_Load(object sender,EventArgs e)
        {
            configura=JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
            if(configura==null)
            {
                return;
            }

            listBox1.Items.Clear();

            foreach(var ip in configura.baseSetting.NetworkGroup)
            {
                listBox1.Items.Add(ip.Ipconfig+"  "+ip.IpName);
            }

            this.Text=$"�������Ӽ����� --��/����ip�ӳ� --{configura.baseSetting.IntranettripTime}/{configura.baseSetting.ExternaltripTime}ms  --������������ {configura.errorReport.isReportError}";

            pingConnecting();

        }

        private void Ping_PingCompleted(object ip)
        {
            while(true)
            {
                string ipconfig = (string)ip;
                IpGroup ipGroup = configura.baseSetting.NetworkGroup.FindLast(x => x.Ipconfig==ipconfig);
                if(ipGroup!=null)
                {
                    Ping ping = new();

                    try
                    {
                        PingReply reply = ping.SendPingAsync(ipGroup.Ipconfig,
                            configura.baseSetting.TimeOut-configura.baseSetting.PingTimer).Result;

                        UpdateUi(reply,ipGroup);
                    }
                    catch { }
                    ping.Dispose();
                    Thread.Sleep(configura.baseSetting.PingTimer); // �ȴ�1����ٴ�ִ��ping����
                }
            }
        }

        private void UpdateUi(PingReply reply,IpGroup group)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<PingReply,IpGroup>(UpdateUi),reply,group);
            }
            else
            {
                if(richTextBox1.Lines.Count()>configura.baseSetting.BuffurMaxLine)
                {
                    var a = richTextBox1.Text.Remove(richTextBox1.GetFirstCharIndexFromLine(100));

                    richTextBox1.Text=a;
                    richTextBox1.Refresh();
                }
                if(richTextBox2.Lines.Count()>configura.baseSetting.BuffurMaxLine)
                {
                    var a = richTextBox2.Text.Remove(0,richTextBox2.GetFirstCharIndexFromLine(100));

                    richTextBox2.Text=a;
                    richTextBox2.Refresh();
                }

                int timespan = group.isintra ? configura.baseSetting.IntranettripTime : configura.baseSetting.ExternaltripTime;


                if(configura.errorReport.isReportError)
                {
                    group.GetReply(reply,timespan).TryReport(out string reportvalue,configura.errorReport.ReportMinTimes,configura.errorReport.SkipTime);

                    if(reportvalue!="")
                    {
                        SendReportToDingDing(reportvalue);

                        //MessageBox.Show(reportvalue);
                    }
                }

                string showvalue = group.GetDoneInfo().ErrorReportContent;

                if(showvalue.Length>0)
                {
                    Invoke((() =>
                    {
                        if(!group.GetDoneInfo().isSuccess)
                        {
                            richTextBox1.AppendText(showvalue+Environment.NewLine);

                            var path = Directory.GetCurrentDirectory()+$"/error/{DateTime.Now:MM-dd}";
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            File.AppendAllText(path+"/error.log",showvalue+"\r\n");
                        }
                        richTextBox2.AppendText(showvalue+Environment.NewLine);
                    }));
                }
            }
        }

        private void ������м�¼ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            richTextBox1.Text="";
            richTextBox2.Text="";
        }

        private void ReReadIpConfigToolStripMenuItem_Click(object sender,EventArgs e)
        {
#nullable disable //load�ص���
            MainForm_Load(null,null);
#nullable enable        
        }
    }

}