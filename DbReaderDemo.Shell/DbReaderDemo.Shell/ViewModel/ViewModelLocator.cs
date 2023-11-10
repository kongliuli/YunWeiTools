using DryIoc;

namespace DbReaderDemo.Shell.ViewModel
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
            _container.Register<DbConnectionViewModel>();
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

        public DbConnectionViewModel Db
        {
            get
            {
                return _container.Resolve<DbConnectionViewModel>();
            }
        }
    }
}