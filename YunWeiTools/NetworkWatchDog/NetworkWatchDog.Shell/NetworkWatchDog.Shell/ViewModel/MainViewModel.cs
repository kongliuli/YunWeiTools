using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

namespace NetworkWatchDog.Shell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        #region 界面绑定
        //顶部功能菜单栏
        //tabcontrol填充
        //listbox填充
        private ObservableCollection<string>? _menuItem;
        private ObservableCollection<string>? _tabItem;
        private ObservableCollection<string>? _listItem;

        public ObservableCollection<string>? ListItem
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
            var builder = new ConfigurationBuilder()
                                .AddJsonFile("Configuartions/UiConfig.json",optional: true,reloadOnChange: true).Build();
#nullable disable
            MenuItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:Menu:Names").Get<string[]>());
            ListItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:ListBox:Names").Get<string[]>());
            TabItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:TabControl:Names").Get<string[]>());
#nullable enable


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
        }

    }
}
