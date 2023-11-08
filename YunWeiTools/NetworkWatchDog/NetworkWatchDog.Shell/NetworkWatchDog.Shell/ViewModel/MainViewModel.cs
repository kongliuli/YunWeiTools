using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

using NetworkWatchDog.Shell.Model;
using NetworkWatchDog.Shell.View;

namespace NetworkWatchDog.Shell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        IConfigurationRoot builder;

        #region 界面绑定
        //顶部功能菜单栏
        //tabcontrol填充
        //listbox填充
        private ObservableCollection<string>? _menuItem;
        private ObservableCollection<string>? _tabItem;
        private BaseSetting? _listItem;

        private ObservableCollection<UserControl> _tabContents;
        public ObservableCollection<UserControl> TabContents
        {
            get => _tabContents;
            set
            {
                _tabContents=value;
                OnPropertyChanged(nameof(TabContents));
                UpdateTabContents();
            }
        }
        public BaseSetting? ListItem
        {
            get => _listItem; set => SetProperty(ref _listItem,value);
        }
        public ObservableCollection<string>? TabItem
        {
            get => _tabItem; set => SetProperty(ref _tabItem,value);
        }
        public ObservableCollection<string>? MenuItem
        {
            get => _menuItem; set => SetProperty(ref _menuItem,value);
        }

        private void InitUiFromConfig()
        {
            builder=new ConfigurationBuilder()
                         .AddJsonFile("Configuartions/UiConfig.json",optional: true,reloadOnChange: true)
                         .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true)
                         .Build();
#nullable disable
            MenuItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:Menu:Names").Get<string[]>());
            ListItem=(builder.GetSection("IpSnifferConfig:BaseSetting").Get<BaseSetting>());

            TabItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:TabControl:Names").Get<string[]>());
#nullable enable


        }

        // 在ViewModel中根据TabHeaders的选择来更新TabContents集合
        private void UpdateTabContents()
        {
            // 根据TabHeaders的选择来更新TabContents集合
            // 例如：
            TabContents.Clear();
            foreach(var header in TabItem)
            {
                if(header=="Ip监听")
                {
                    TabContents.Add(new IpSniffer());
                }
            }
        }



        #endregion

        #region 命令绑定
        public ICommand? MenuItemCommand
        {
            get; set;
        }
        public ICommand? ListItemCommand
        {
            get; set;
        }

        #region 事件集合
        private void MenuItemClicked(object? param)
        {
            // 执行菜单子项的点击事件处理或统一跳转事件处理
            string? menuItem = param as string;
            if(menuItem!=null)
            {

            }
        }

        private void ListItemClicked(object? param)
        {
            string? menuItem = param as string;
            if(menuItem!=null)
            {
                MessageBox.Show(menuItem);
            }
        }



        #endregion
        /// <summary>
        /// 绑定所有事件
        /// </summary>
        private void InitCommand()
        {
            MenuItemCommand=new RelayCommand<object>(MenuItemClicked);
            ListItemCommand=new RelayCommand<object>(ListItemClicked);
        }

        #endregion
        public MainViewModel()
        {
            InitUiFromConfig();
            InitCommand();
            UpdateTabContents();
        }

    }
}
