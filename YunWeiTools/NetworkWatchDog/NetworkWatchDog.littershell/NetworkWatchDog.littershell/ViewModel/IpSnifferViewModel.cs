using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Input;

using DingTalkLib;

using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class IpSnifferViewModel:ObservableObject
    {
        private IpSnifferConfig? _ipsnifferconfig;
        private IConfigurationRoot builder;
        private ReportConfig? _reportConfig;



        #region 初始化属性
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

        private bool _IsAutoRefalsh = true;
        public bool IsAutoRefalsh
        {
            get => _IsAutoRefalsh;
            set
            {
                SetProperty(ref _IsAutoRefalsh,value);
            }
        }
        #endregion

        public IpSnifferViewModel()
        {
            ConfigInit();


            GetIpStart();
        }

        #region 方法绑定
        public ICommand ClearCommand
        {
            get
            {
                return new RelayCommand(ClearClick);
            }
        }

        public ICommand ReloadCommand
        {
            get
            {
                return new RelayCommand(ConfigInit);
            }
        }

        public ICommand ReportCommand
        {
            get
            {
                return new RelayCommand(TryReport);
            }
        }
        #endregion
        private void TryReport()
        {
            SendReportToDingDing($"### 网络连通性报警\r\n>- 报警设备:无线ap-35-6\r\n>- IP地址:192.168.0.19\r\n>- 设备信息:913\r\n>- 设备位置:\r\n\r\n>- 报警内容:内网测试连通性发生多次异常\r\n>- 报警阈值:在一分钟内访问基准地址超过失败10次,失败标准为延迟超过100ms\r\n\r\n失败信息\r\n2023-12-08 14:36:29 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:33 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:37 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:41 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:45 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:49 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:53 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:57 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:37:01 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:37:05 192.168.0.19 DestinationHostUnreachable\r\n\r\n","测试报警");
        }


        private void ClearClick()
        {
            Info.Infos=new();
            ErrorInfo.Infos=new();
        }
        private void ConfigInit()
        {
            _ipsnifferconfig=ViewModelLocator._ipsnifferconfig;
            _reportConfig=ViewModelLocator._reportConfig;
        }
        private void ClearCommand_CanExecuteChanged(object? sender,System.EventArgs e)
        {
            Info.Infos=new ObservableCollection<string>();
            ErrorInfo.Infos=new ObservableCollection<string>();
        }

        #region 核心方法
        /// <summary>
        /// 启动方法
        /// </summary>
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
        /// <summary>
        /// ui更新
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="group"></param>
        private void UpdateUi(PingReply reply,IpGroup group)
        {
            int timespan = group.isintra ? _ipsnifferconfig.BaseSetting.IntranettripTime : _ipsnifferconfig.BaseSetting.ExternaltripTime;

            if(_reportConfig.errorReport.isReportError)
            {
                group.GetReply(reply,timespan).TryReportMarkdown(out string reportvalue,_reportConfig.errorReport.ReportMinTimes,_reportConfig.errorReport.SkipTime);

                if(reportvalue!="")
                {
                    //SendReportToDingDing(reportvalue,"测试markdown");
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
        /// <summary>
        /// 钉钉报警
        /// </summary>
        /// <param name="reportvalue"></param>
        /// <param name="msgtitle"></param>
        private void SendReportToDingDing(string reportvalue,string msgtitle)
        {
            MessageContent markdown = new()
            {
                title=msgtitle,
                text=reportvalue
            };

            MarkDownDingTalkClient md = new(
                markdown,
                _reportConfig.reportRule.DingTalkUri,
                _reportConfig.reportRule.Token,
                _reportConfig.reportRule.SecretInfo,
                new DingTalkAtSetting() { IsAtAll=true });

            md.IGetPostUri().SendMessageAsync();
        }

        #endregion
    }
}
