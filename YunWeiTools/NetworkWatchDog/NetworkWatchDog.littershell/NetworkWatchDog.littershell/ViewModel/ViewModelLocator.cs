using DryIoc;

namespace NetworkWatchDog.littershell.ViewModel
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
            _container.Register<IpSnifferViewModel>();
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
                return _container.Resolve<IpSnifferViewModel>();
            }
        }
    }
}