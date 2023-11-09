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
        private IpSnifferConfig _config;
        private IConfigurationRoot builder;

        #region 界面绑定
        //tabcontrol填充
        private TabControlItemsGroup? _tabItem;

        public TabControlItemsGroup? TabItem
        {
            get => _tabItem; set => SetProperty(ref _tabItem,value);
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
            foreach(var item in _tabItem.Items)
            {
                item.Info=new StringStreamInfomation()
                {
                    Infos=new List<string>() { "123","123","123","123","123","123","123","123","123","123",}
                }
            ;
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
            foreach(var ipAddress in _config.BaseSetting.NetworkGroup.Where(ip => !ListiningIP.Contains(ip.Ipconfig)))
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
                IpGroup ipGroup = _config.BaseSetting.NetworkGroup.FindLast(x => x.Ipconfig==ipconfig);
                if(ipGroup!=null)
                {
                    Ping ping = new();

                    try
                    {
                        PingReply reply = ping.SendPingAsync(ipGroup.Ipconfig,
                             _config.BaseSetting.TimeOut-_config.BaseSetting.PingTimer).Result;

                        UpdateUi(reply,ipGroup);
                    }
                    catch
                    {

                    }
                    ping.Dispose();
                    Thread.Sleep(_config.BaseSetting.PingTimer); // 等待1秒后再次执行ping命令
                }
                else
                {

                }
            }
        }


        private void UpdateUi(PingReply reply,IpGroup group)
        {

        }

        #endregion


        public IpSnifferViewModel()
        {
            builder=new ConfigurationBuilder()
                       .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true).Build();

            InitUiFromConfig();
            InitListView();
            _config=builder.GetSection("IpSnifferConfig").Get<IpSnifferConfig>();
        }
    }
}