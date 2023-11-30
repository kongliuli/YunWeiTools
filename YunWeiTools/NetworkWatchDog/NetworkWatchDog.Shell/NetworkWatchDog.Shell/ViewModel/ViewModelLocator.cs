using DryIoc;

namespace NetworkWatchDog.Shell.ViewModel
{
    /// <summary>
    /// ����������Ӧ�ó�����������ͼģ�͵ľ�̬���ã����ṩ�󶨵���ڵ㡣
    /// </summary>
    public class ViewModelLocator
    {
        Container _container;

        /// <summary>
        /// ���캯��
        /// </summary>
        public ViewModelLocator()
        {
            //��ʼ������
            _container=new Container();

            //ע�ᵽ������
            _container.Register<MainViewModel>();
            //ע�ᵽ������
            _container.Register<IpSnifferViewModel>();
            _container.Register<TraceRouteViewModel>();
        }

        /// <summary>
        /// MainViewModel��ͼģ�͵ľ�̬����
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                //ͨ��������ȡʵ������
                return _container.Resolve<MainViewModel>();
            }
        }

        public IpSnifferViewModel IpSniffer
        {
            get
            {
                //ͨ��������ȡʵ������
                return _container.Resolve<IpSnifferViewModel>();
            }
        }

        public TraceRouteViewModel TraceRoute
        {
            get
            {
                return _container.Resolve<TraceRouteViewModel>();
            }
        }
    }
}