using DryIoc;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    /// <summary>
    /// 这个类包含对应用程序中所有视图模型的静态引用，并提供绑定的入口点。
    /// </summary>
    public class ViewModelLocator
    {
        Container _container;
        public static IpSnifferConfig? _ipsnifferconfig;
        public static ReportConfig? _reportConfig;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewModelLocator()
        {
            //初始化容器
            _container=new Container();

            //注册到容器中
            _container.Register<MainViewModel>();
            _container.Register<IpSnifferViewModel>();
            _container.Register<MTRouterViewModel>();
        }

        /// <summary>
        /// MainViewModel视图模型的静态引用
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                //通过容器获取实例对象
                return _container.Resolve<MainViewModel>();
            }
        }

        public IpSnifferViewModel IpSniffer
        {
            get
            {
                return _container.Resolve<IpSnifferViewModel>();
            }
        }

        public MTRouterViewModel MTR
        {
            get
            {
                return _container.Resolve<MTRouterViewModel>();
            }
        }
    }
}