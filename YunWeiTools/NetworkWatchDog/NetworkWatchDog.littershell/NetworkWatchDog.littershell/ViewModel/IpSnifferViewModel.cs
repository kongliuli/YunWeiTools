using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using DingTalkLib;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class IpSnifferViewModel:ObservableObject
    {
        public IpSnifferConfig? IpSnifferConfig
        {
            get => _ipsnifferconfig;
            set
            {
                SetProperty(ref _ipsnifferconfig,value);
            }
        }
        private IpSnifferConfig? _ipsnifferconfig;
        private ReportConfig? _reportConfig;

        public bool isSnifferContiue = false;
        public Thread? SnifferDog;
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

        private string _addipcontent = "请输入ip";
        public string AddIpContent
        {
            get => _addipcontent;
            set
            {
                SetProperty(ref _addipcontent,value);
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

        public ICommand SwichPingCommand => new RelayCommand<object>(SwichPing);
        public ICommand SelectAllIpGroup
        {
            get
            {
                return new RelayCommand(() => ChangeIpGroupSelect(false));
            }
        }
        public ICommand UnSelectAllIpGroup
        {
            get
            {
                return new RelayCommand(() => ChangeIpGroupSelect(true));
            }
        }
        public ICommand AddIpInGroupCommand
        {
            get
            {
                return new RelayCommand(AddIpInGroup);
            }
        }
        #endregion
        private void TryReport()
        {
            SendReportToDingDing($"### 网络连通性报警\r\n>- 报警设备:无线ap-35-6\r\n>- IP地址:192.168.0.19\r\n>- 设备信息:913\r\n>- 设备位置:\r\n\r\n>- 报警内容:内网测试连通性发生多次异常\r\n>- 报警阈值:在一分钟内访问基准地址超过失败10次,失败标准为延迟超过100ms\r\n\r\n失败信息\r\n2023-12-08 14:36:29 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:33 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:37 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:41 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:45 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:49 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:53 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:36:57 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:37:01 192.168.0.19 DestinationHostUnreachable\r\n2023-12-08 14:37:05 192.168.0.19 DestinationHostUnreachable\r\n\r\n","测试报警");
        }

        private void SwichPing(object? param)
        {
            try
            {
                if(param is IpGroup gr)
                {
                    if(gr.IsPingEnabled)
                    {
                        ListiningIp.Add(gr);
                    }
                    else
                    {
                        var addingip = ListiningIp.FirstOrDefault(x => x.Ipconfig==gr.Ipconfig);
                        if(addingip!=null)
                            ListiningIp.Remove(addingip);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex.Message,"switcherror");
            }

        }
        private void ClearClick()
        {
            Info.Infos=new();
            ErrorInfo.Infos=new();
        }
        private void ConfigInit()
        {
            _ipsnifferconfig=ViewModelLocator._ipsnifferconfig??new();
            _reportConfig=ViewModelLocator._reportConfig??new();
        }
        private void ClearCommand_CanExecuteChanged(object? sender,System.EventArgs e)
        {
            Info.Infos=new ObservableCollection<PingRelayInfo>();
            ErrorInfo.Infos=new ObservableCollection<PingRelayInfo>();
        }
        private void ChangeIpGroupSelect(bool isSelect)
        {
            if(_ipsnifferconfig!=null)
            {
                if(isSelect)
                {
                    foreach(var i in _ipsnifferconfig.BaseSetting.NetworkGroup)
                    {
                        i.IsPingEnabled=false;
                    }
                }
                else
                {
                    foreach(var i in _ipsnifferconfig.BaseSetting.NetworkGroup)
                    {
                        i.IsPingEnabled=true;
                    }
                }
                ReflashListenIp();
            }
        }

        private void ReflashListenIp()
        {
            if(_ipsnifferconfig!=null)
            {
                ListiningIp=new();
                foreach(var ip in _ipsnifferconfig.BaseSetting.NetworkGroup.Where(x => x.IsPingEnabled))
                {
                    ListiningIp.Add(ip);
                }
            }
        }

        private void AddIpInGroup()
        {
            if(_ipsnifferconfig!=null)
            {
                if(!_ipsnifferconfig.BaseSetting.NetworkGroup.Any(x => x.Ipconfig==AddIpContent))
                {
                    try
                    {
                        using Ping p = new();
                        p.Send(AddIpContent);
                        p.Dispose();
                        _ipsnifferconfig.BaseSetting.NetworkGroup.Add(new IpGroup() { Ipconfig=AddIpContent });
                        ReflashListenIp();
                    }
                    catch
                    {
                        MessageBox.Show("ip格式不合法");
                    }
                }
            }
        }
        #region 核心方法
        /// <summary>
        /// 启动方法
        /// </summary>
        public void GetIpStart()
        {
            ReflashListenIp();
            if(!isSnifferContiue)
            {
                isSnifferContiue=true;
                SnifferDog=new Thread(() => PingConnecting());
                SnifferDog.Start();
            }
        }

        private void PingConnecting()
        {
            while(true)
            {
                foreach(var ipgroup in ListiningIp)
                {
                    new Thread(() => PingPingCompleted(ipgroup.Ipconfig)).Start();
                }
                Thread.Sleep(_ipsnifferconfig.BaseSetting.PingTimer); // 等待1秒后再次执行ping命令
            }
        }

        private void PingPingCompleted(object ip)
        {
            IpGroup? ipGroup = _ipsnifferconfig.BaseSetting.NetworkGroup.First(x => x.Ipconfig==(string)ip);
            if(ipGroup!=null)
            {
                using Ping ping = new();

                try
                {
                    UpdateUi(
                        reply: ping.SendPingAsync(
                            hostNameOrAddress: ipGroup.Ipconfig,
                            timeout: _ipsnifferconfig.BaseSetting.TimeOut-_ipsnifferconfig.BaseSetting.PingTimer).Result,
                        group: ipGroup);
                }
                catch(Exception e)
                {
                    LogManager.WriteLog(e.Message,"error");
                }
                ping.Dispose();
            }
        }

        /// <summary>
        /// ui更新
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="group"></param>
        private void UpdateUi(PingReply reply,IpGroup group)
        {
            if(reply.Status!=IPStatus.Success)
            {
                var log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -回溯地址:{reply.Address} -ip源头{group.Ipconfig} -通信状态{reply.Status}";
                LogManager.WriteLog(log,"replyInfo");
                if(reply.Address.ToString()=="0.0.0.0")
                {
                    return;
                }
            }

            int timespan = group.isintra ? _ipsnifferconfig.BaseSetting.IntranettripTime : _ipsnifferconfig.BaseSetting.ExternaltripTime;
            if(_reportConfig.errorReport.isReportError)
            {
                group.GetReply(reply,timespan).TryReportMarkdown(out string reportvalue,_reportConfig.errorReport.ReportMinTimes,_reportConfig.errorReport.SkipTime);

                if(reportvalue!="")
                {
                    SendReportToDingDing(reportvalue,"网络连通性报警");
                }
            }

            var showvalue = group.GetDoneInfo().ErrorReportContent;
            if(showvalue is not null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Info.ADDInfo(showvalue);
                    if(!group.GetDoneInfo().isSuccess)
                    {
                        ErrorInfo.ADDInfo(showvalue);
                        LogManager.WriteLog(showvalue.GetContext(),"IpSniffer");
                    }
                });

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

            //MessageBox.Show(md.MarkDown.text);
            //md.IGetPostUri().SendMessageAsync();
        }

        #endregion
    }
}
