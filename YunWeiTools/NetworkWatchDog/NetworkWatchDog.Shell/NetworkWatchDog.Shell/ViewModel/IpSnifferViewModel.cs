using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Configuration;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.Shell.ViewModel
{
    public class IpSnifferViewModel:ObservableObject
    {
        private IpSnifferConfig _ipsnifferconfig;
        private IConfigurationRoot builder;
        private ReportConfig _reportConfig;

        #region 界面绑定
        //tabcontrol填充
        private TabControlItemsGroup? _tabItem;

        public TabControlItemsGroup? TabItem
        {
            get => _tabItem;
            set
            {
                SetProperty(ref _tabItem,value);
            }
        }

        private void InitUiFromConfig()
        {
#nullable disable
            TabItem=(builder.GetSection("IpSnifferConfig:MainPage:TabControlItemsGroup")
                            .Get<TabControlItemsGroup>());

#nullable enable
        }
        private void InitListView()
        {
            foreach(var item in TabItem.Items)
            {
            }
        }

        #endregion

        private List<string> ListiningIP = new List<string>();




        #region 核心方法
        public void GetIpStart()
        {
            pingConnecting();
        }
        private void pingConnecting()
        {
            foreach(var ipAddress in _ipsnifferconfig.BaseSetting.NetworkGroup.Where(ip => !ListiningIP.Contains(ip.Ipconfig)))
            {
                // 启动ping线程
                ListiningIP.Add(ipAddress.Ipconfig);
                new Thread(() => Ping_PingCompleted(ipAddress.Ipconfig)).Start();
            }
        }
        private void Ping_PingCompleted(object ip)
        {
            while(true)
            {
                string ipconfig = (string)ip;
                IpGroup ipGroup = _ipsnifferconfig.BaseSetting.NetworkGroup.FindLast(x => x.Ipconfig==ipconfig);
                if(ipGroup!=null)
                {
                    Ping ping = new();

                    try
                    {
                        PingReply reply = ping.SendPingAsync(ipGroup.Ipconfig,
                             _ipsnifferconfig.BaseSetting.TimeOut-_ipsnifferconfig.BaseSetting.PingTimer).Result;

                        UpdateUi(reply,ipGroup);
                    }
                    catch
                    {

                    }
                    ping.Dispose();
                    Thread.Sleep(_ipsnifferconfig.BaseSetting.PingTimer); // 等待1秒后再次执行ping命令
                }
                else
                {

                }
            }
        }


        private void UpdateUi(PingReply reply,IpGroup group)
        {
            int timespan = group.isintra ? _ipsnifferconfig.BaseSetting.IntranettripTime : _ipsnifferconfig.BaseSetting.ExternaltripTime;

            if(_reportConfig.errorReport.isReportError)
            {
                group.GetReply(reply,timespan).TryReport(out string reportvalue,_reportConfig.errorReport.ReportMinTimes,_reportConfig.errorReport.SkipTime);

                if(reportvalue!="")
                {
                    //SendReportToDingDing(reportvalue);
                }
            }

            string showvalue = group.GetDoneInfo().ErrorReportContent;

            if(showvalue.Length>0)
            {
                foreach(var item in _tabItem.Items)
                {
                    Thread.Sleep(1);
                    bool b = false;
                    switch(item.Header)
                    {

                        case "实时信息":
                        b=true;
                        item.Info.ADDInfo(showvalue);
                        break;
                        case "错误报告":
                        if(!group.GetDoneInfo().isSuccess)
                        {
                            item.Info.ADDInfo(showvalue);
                            LogManager.WriteLog(showvalue,"IpSniffer");
                        }
                        break;
                    }
                    item.Info.ADDInfo(showvalue);
                }

            }
        }

        #endregion


        public IpSnifferViewModel()
        {
            builder=new ConfigurationBuilder()
                       .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true)
                       .AddJsonFile("Configuartions/ReportConfig.json",optional: true,reloadOnChange: true)
                       .Build();

            _ipsnifferconfig=builder.GetSection("IpSnifferConfig").Get<IpSnifferConfig>();
            _reportConfig=builder.GetSection("ReportConfig").Get<ReportConfig>();


            InitUiFromConfig();
            InitListView();



        }
    }
}