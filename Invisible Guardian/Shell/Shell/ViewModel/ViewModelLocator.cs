using DryIoc;

namespace Shell.ViewModel
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
            _container.Register<LoginViewModel>();
            _container.Register<MainViewModel>();
        }

        /// <summary>
        /// MainViewModel��ͼģ�͵ľ�̬����
        /// </summary>
        public LoginViewModel Login
        {
            get
            {
                //ͨ��������ȡʵ������
                return _container.Resolve<LoginViewModel>();
            }
        }
        public MainViewModel Main
        {
            get
            {
                //ͨ��������ȡʵ������
                return _container.Resolve<MainViewModel>();
            }
        }

    }
}