using DryIoc;

namespace Shell.ViewModel
{
    /// <summary>
    /// 这个类包含对应用程序中所有视图模型的静态引用，并提供绑定的入口点。
    /// </summary>
    public class ViewModelLocator
    {
        Container _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewModelLocator()
        {
            //初始化容器
            _container=new Container();

            //注册到容器中
            _container.Register<LoginViewModel>();
            _container.Register<MainViewModel>();
        }

        /// <summary>
        /// MainViewModel视图模型的静态引用
        /// </summary>
        public LoginViewModel Login
        {
            get
            {
                //通过容器获取实例对象
                return _container.Resolve<LoginViewModel>();
            }
        }
        public MainViewModel Main
        {
            get
            {
                //通过容器获取实例对象
                return _container.Resolve<MainViewModel>();
            }
        }

    }
}