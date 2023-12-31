using System.Net.NetworkInformation;

using Newtonsoft.Json;

namespace NetworkWatchDog
{
    //屏蔽报警时段
    //wpf重构
    //mtr功能
    public partial class Form1:Form
    {
        DingTalkLib.MarkDownDingTalkClient _client;
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
            // 构造POST请求的内容
            var postData = new FormUrlEncodedContent(new[]
            {
                //报警群组设置为系统组
                new KeyValuePair<string, string>("grounp", "xitongzu"),
                //设置报警时触发at联系人
                new KeyValuePair<string, string>("at", "18751936236,17361914131"),
                //报警信息
                new KeyValuePair<string, string>("message", msg)
            });
            try
            {
                // 发送POST请求
                HttpResponseMessage response = await client.PostAsync("https://www.kujiang.com/Tool/dingding",postData);

                if(response.IsSuccessStatusCode)
                {
                    // 获取响应内容
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
                // 启动ping线程
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

            this.Text=$"网络连接监视器 --内/外网ip延迟 --{configura.baseSetting.IntranettripTime}/{configura.baseSetting.ExternaltripTime}ms  --报警规则启用 {configura.errorReport.isReportError}";

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
                    Thread.Sleep(configura.baseSetting.PingTimer); // 等待1秒后再次执行ping命令
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
                    group.GetReply(reply,timespan).TryReportMarkdownTable(out string reportvalue,configura.errorReport.ReportMinTimes,configura.errorReport.SkipTime);

                    if(reportvalue!="")
                    {
                        //SendReportToDingDing(reportvalue);

                        var at = new DingTalkLib.DingTalkAtSetting()
                        {
                            AtMoblies=new string[] { "18751936236" },
                            AtUsers=new string[] { "18751936236" },
                            IsAtAll=false
                        };

                        var markdown = new DingTalkLib.MessageContent()
                        {
                            title="网络监控报警",
                            text=$"#### 网络监控报警\n\n> **{group.IpName}** ap连接故障报警\n> \n> ip：{group.Ipconfig}\n> 型号：{group.MachineLocation}\n> 位置：\n> 信息：此ap一分钟内出现了{configura.errorReport.ReportMinTimes}次连接故障。请登录向日葵查看详情\n\n| 时间 | 错误信息 |\n| --- | --- | --- |\n{reportvalue}"
                        };


                        _client=new(markdown,
                                                   "https://oapi.dingtalk.com/robot/send",
                                                   "cf48aae08bb3cf2bf88a79b349c38ade78348d8571d70b21b56113df2fb74897",
                                                   "SECac5f99d07087195fecc94ac270bbf5d76b8da57a5e4499c047474b851e71fcc2",
                                                   at);
                        _client.IGetPostUri().SendMessageAsync();
                        var respone = _client.Response.Result;
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

        private void 清空所有记录ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            richTextBox1.Text="";
            richTextBox2.Text="";
        }

        private void ReReadIpConfigToolStripMenuItem_Click(object sender,EventArgs e)
        {
#nullable disable //load重调用
            MainForm_Load(null,null);
#nullable enable        
        }
    }

}