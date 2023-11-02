using System.Net.NetworkInformation;

using Newtonsoft.Json;

namespace NetworkWatchDog
{
    public partial class Form1:Form
    {
        public Configuration configura = new();

        private List<string> ListiningIP;
        int IntranettripTime = 10, ExternaltripTime = 150;
        int PingTimer = 1000, BuffurMaxLine = 10000;
        int timeout = 5000;

        public Form1()
        {
            InitializeComponent();
            //SendReportToDingDingTEST();
            ListiningIP=new();
        }

        private async void SendReportToDingDingTEST()
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
                new KeyValuePair<string, string>("message", "{ipname} ap���ӹ��ϱ�����\r\n\r\nip��{ip}\r\n�ͺţ�{ipinfo}\r\nλ�ã�{iplocation}\r\n��Ϣ����apһ�����ڳ�����36�����ӹ��ϡ����¼���տ��鿴����\r\n")
            });

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

            this.Text=$"�������Ӽ����� --��/����ip�ӳ� --{configura.baseSetting.IntranettripTime}/{configura.baseSetting.ExternaltripTime}ms";

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
                        PingReply reply = ping.Send(ipGroup.Ipconfig,configura.baseSetting.TimeOut);



                        if(reply.Status==IPStatus.Success)
                        {
                            UpdateUI(ipGroup.Ipconfig,$": �ֽ�={reply.Buffer.Length} ʱ��={reply.RoundtripTime}ms TTL={reply.Options?.Ttl}",reply.RoundtripTime,ipGroup.isintra);
                        }
                        else
                        {
                            //���ɹ�����
                            UpdateUI(ipGroup.Ipconfig,$": {reply.Status}",-1);
                        }
                    }
                    catch(Exception ex)
                    {
                        //�쳣����
                        UpdateUI(ipGroup.Ipconfig,$": {ex.Message}",-1);
                    }
                    ping.Dispose();
                    Thread.Sleep(PingTimer); // �ȴ�1����ٴ�ִ��ping����
                }
                else
                {
                    UpdateUI("","�������,�Ҳ���ip��Ϣ",-1);
                }
            }
        }

        private void UpdateUI(string ip,string info,long outtime,bool istra = true)
        {
            string message = $"{DateTime.Now:yyyy-MM-mm HH:mm:ss} --{ip}";

            if(InvokeRequired)
            {
                Invoke(new Action<string,string,long,bool>(UpdateUI),ip,info,outtime,istra);
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
                    // ִ�г�ʱ�����еĲ���
                    if(outtime>=triptime||outtime==-1)
                    {
                        Invoke((() =>
                        {
                            richTextBox1.AppendText(message+info+Environment.NewLine);
                            File.AppendAllText(Directory.GetCurrentDirectory()+"/error.log",message+info+"\r\n");
                        }));
                    }

                    Invoke((() =>
                    {
                        richTextBox2.AppendText(message+info+Environment.NewLine);
                    }));
                });
            }
        }

        private void UpdateUiNew(PingReply reply,IpGroup group)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<PingReply,IpGroup>(UpdateUiNew),reply,group);
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

                bool issuccess = false;
                string message = "";
                if(reply!=null)
                {
                    if(reply.Status==IPStatus.Success)
                    {
                        issuccess=true;
                    }
                    else
                    {

                    }
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