using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class MTRouterViewModel:ObservableObject
    {
        private IpSnifferConfig? _ipsnifferconfig;
        private ReportConfig? _reportConfig;
        static byte[] PING_BUFFER = new byte[] { 0 };

        static int g_nHops = 30;
        static int g_nTimeout = 3000;
        static bool g_bCanceled = false;
        public struct ICMP_PARAM
        {
            internal long m_nSendTicks;
            internal IPAddress m_IPAddress;
            internal PingOptions m_PingOptions;
        }

        private ObservableCollection<IpGroup> ipGroups = new ObservableCollection<IpGroup>();
        public ObservableCollection<IpGroup> _ipgroups
        {
            get => ipGroups;
            set
            {
                SetProperty(ref ipGroups,value);
            }
        }

        private ObservableCollection<string[]> taroutes = new ObservableCollection<string[]>();
        public ObservableCollection<string[]> _taroutes
        {
            get => taroutes;
            set
            {
                SetProperty(ref taroutes,value);
            }
        }

        public MTRouterViewModel()
        {
            _ipsnifferconfig=ViewModelLocator._ipsnifferconfig;
            _reportConfig=ViewModelLocator._reportConfig;

            foreach(var i in _ipsnifferconfig.BaseSetting.NetworkGroup)
            {
                ipGroups.Add(i);
            }
        }
        private string _messagetext = "";
        public string MessageText
        {
            get => _messagetext;
            set
            {
                SetProperty(ref _messagetext,value);
            }
        }

        public ICommand StartPing
        {
            get
            {
                return new RelayCommand(StartTracerouteButton_Click);
            }
        }
        private void StartTracerouteButton_Click()
        {
            taroutes=new ObservableCollection<string[]>();
            string szDomain = "www.baidu.com";

            ICMP_PARAM param = new ICMP_PARAM();
            param.m_PingOptions=new PingOptions(1,false);
            if(!IPAddress.TryParse(szDomain,out param.m_IPAddress))
            {
                // 解析域名
                try
                {
                    Regex regEx = new Regex("\\d+\\.\\d+\\.\\d+\\.\\d+");
                    IPHostEntry hostEntry = Dns.GetHostEntry(szDomain);
                    foreach(IPAddress ipAddr in hostEntry.AddressList)
                    {
                        if(regEx.IsMatch(ipAddr.ToString()))
                        {
                            param.m_IPAddress=ipAddr;
                            break;
                        }
                    }
                    _messagetext=$"正在跟踪到 {szDomain}[{param.m_IPAddress}] 间的路由：";
                }
                catch(Exception ex)
                {
                    _messagetext=(ex.ToString());
                    return;
                }
            }
            else
            {
                _messagetext=("正在跟踪到 {0} 间的路由：");
            }

            Ping icmp = new Ping();
            icmp.PingCompleted+=new PingCompletedEventHandler(icmp_PingCompleted);

            param.m_nSendTicks=DateTime.Now.Ticks;
            icmp.SendAsync(param.m_IPAddress,g_nTimeout,PING_BUFFER,param.m_PingOptions,param);
        }

        private void icmp_PingCompleted(object sender,PingCompletedEventArgs e)
        {
            ICMP_PARAM param = (ICMP_PARAM)e.UserState;
            long nDeltaMS = (DateTime.Now.Ticks-param.m_nSendTicks)/TimeSpan.TicksPerMillisecond;
            _messagetext=($"{param.m_PingOptions.Ttl}:\t{((e.Reply.Status==IPStatus.TimedOut) ? "*" : nDeltaMS.ToString())} ms\t{e.Reply.Address}");


            taroutes.Add(new string[]
            {
                param.m_PingOptions.Ttl.ToString(),
                param.m_IPAddress.ToString(),
                "",
                "丢包率",
                PING_BUFFER.Length.ToString(),
                nDeltaMS.ToString()
            });


            if(param.m_IPAddress.Equals(e.Reply.Address))
            {
                _messagetext=("已到达目标地址！");
                g_bCanceled=true;
                return;
            }

            if(param.m_PingOptions.Ttl>=g_nHops)
            {
                _messagetext=("已达到最大跃点计数！");
                g_bCanceled=true;
                return;
            }

            Ping icmp = (Ping)sender;
            param.m_PingOptions.Ttl++;
            param.m_nSendTicks=DateTime.Now.Ticks;
            icmp.SendAsync(param.m_IPAddress,g_nTimeout,PING_BUFFER,param.m_PingOptions,param);
        }
    }
}
