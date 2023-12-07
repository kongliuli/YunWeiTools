﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

using DingTalkLib;

using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class IpSnifferViewModel:ObservableObject
    {
        private IpSnifferConfig _ipsnifferconfig;
        private IConfigurationRoot builder;
        private ReportConfig _reportConfig;

        private ObservableCollection<IpGroup> _listiningIp = new();
        public ObservableCollection<IpGroup> ListiningIp
        {
            get => _listiningIp;
            set
            {
                SetProperty(ref _listiningIp,value);
            }
        }

        private StringStreamInfomation _info = new();
        public StringStreamInfomation Info
        {
            get => _info;
            set
            {
                SetProperty(ref _info,value);
            }
        }
        private StringStreamInfomation _errorinfo = new();
        public StringStreamInfomation ErrorInfo
        {
            get => _errorinfo;
            set
            {
                SetProperty(ref _errorinfo,value);
            }
        }

        public IpSnifferViewModel()
        {
            builder=new ConfigurationBuilder()
                       .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true)
                       .AddJsonFile("Configuartions/ReportConfig.json",optional: true,reloadOnChange: true)
                       .Build();

            _ipsnifferconfig=builder.GetSection("IpSnifferConfig").Get<IpSnifferConfig>();
            _reportConfig=builder.GetSection("ReportConfig").Get<ReportConfig>();
            GetIpStart();
        }


        #region 核心方法
        public void GetIpStart()
        {
            pingConnecting();
        }
        private void pingConnecting()
        {
            foreach(var ipAddress in _ipsnifferconfig.BaseSetting.NetworkGroup.Where(ip => !ListiningIp.Contains(ip)))
            {
                ListiningIp.Add(ipAddress);
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
                group.GetReply(reply,timespan).TryReportMarkdown(out string reportvalue,_reportConfig.errorReport.ReportMinTimes,_reportConfig.errorReport.SkipTime);

                if(reportvalue!="")
                {
                    SendReportToDingDing(reportvalue,"测试markdown");
                }
            }

            string showvalue = group.GetDoneInfo().ErrorReportContent;

            if(showvalue.Length>0)
            {
                Info.ADDInfo(showvalue);
                if(!group.GetDoneInfo().isSuccess)
                {
                    ErrorInfo.ADDInfo(showvalue);
                    LogManager.WriteLog(showvalue,"IpSniffer");
                }
            }
            Thread.Sleep(1);
        }

        private void SendReportToDingDing(string reportvalue,string msgtitle)
        {
            MessageContent markdown = new()
            {
                title=msgtitle,
                text=reportvalue
            };

            MarkDownDingTalkClient md = new(
                markdown,
                "https://oapi.dingtalk.com/robot/send",
                "cf48aae08bb3cf2bf88a79b349c38ade78348d8571d70b21b56113df2fb74897",
                "SECac5f99d07087195fecc94ac270bbf5d76b8da57a5e4499c047474b851e71fcc2",
                null);

            var repo = md.IGetPostUri()
                                  .SendMessageAsync()
                                  .Response
                                  .Result
                                  .Content;
        }

        #endregion
    }
}
