using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

using NetworkWatchDog.Shell.Model;
using NetworkWatchDog.Shell.View;

using Window = System.Windows.Window;

namespace NetworkWatchDog.Shell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        IConfigurationRoot builder;
        public Window? View
        {
            get; set;
        }
        #region 界面绑定
        //顶部功能菜单栏
        //tabcontrol填充
        //listbox填充
        private ObservableCollection<string>? _menuItem;
        private TabControlItemsGroup? _tabGroup;
        private BaseSetting? _listItem;

        public ObservableCollection<UserControl> TabContents
        {
            get;
            set;
        } = new ObservableCollection<UserControl>();
        public BaseSetting? ListItem
        {
            get => _listItem; set => SetProperty(ref _listItem,value);
        }
        public TabControlItemsGroup? TabGroup
        {
            get => _tabGroup; set => SetProperty(ref _tabGroup,value);
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
            MenuItem=new ObservableCollection<string>(builder.GetSection("UiConfig:MainPage:Menu:Names")
                                                             .Get<string[]>());
            ListItem=builder.GetSection("IpSnifferConfig:BaseSetting")
                            .Get<BaseSetting>();

            TabGroup=builder.GetSection("UiConfig:MainPage:TabControlItemsGroup")
                           .Get<TabControlItemsGroup>();
#nullable enable


        }

        private void InitTabControl()
        {
            if(_tabGroup!=null)
            {
                var a = _tabGroup.Items.FirstOrDefault(predicate: x => x.Header=="Ip监听");
                if(a!=null)
                {
                    a.Content=new IpSniffer();
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
                if(menuItem=="测试TraceRoute")
                {
                    var tabs = (View as MainView).tabcontrol1;
                    tabs.SelectedIndex=1;


                    var a = tabs.Items[1];
                    var b = a as System.Windows.Controls.TabItem;

                    var cc = b.Content as RouteTrain;
                    var dd = cc.DataContext as TraceRouteViewModel;




                    dd.AddTraceRoute("192.168.0.5");
                }
            }
        }

        private void ListItemClicked(object? param)
        {
            string? menuItem = param as string;
            if(menuItem!=null)
            {

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
            InitTabControl();
        }

    }
}
