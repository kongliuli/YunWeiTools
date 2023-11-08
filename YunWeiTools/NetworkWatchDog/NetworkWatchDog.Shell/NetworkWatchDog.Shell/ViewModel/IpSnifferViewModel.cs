using System.Collections.ObjectModel;

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
        private ObservableCollection<string>? _tabItem;

        public ObservableCollection<string>? TabItem
        {
            get => _tabItem; set => SetProperty(ref _tabItem,value);
        }

        private void InitUiFromConfig()
        {
#nullable disable
            TabItem=new ObservableCollection<string>(builder.GetSection("IpSnifferConfig:MainPage:TabControl:Names").Get<string[]>());
#nullable enable
        }
        #endregion

        #region 核心方法
        public void GetIpStart()
        {

        }


        #endregion


        public IpSnifferViewModel()
        {
            builder=new ConfigurationBuilder()
                       .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true).Build();

            InitUiFromConfig();

            _config=builder.GetSection("IpSnifferConfig:baseSetting").Get<IpSnifferConfig>();
        }
    }
}