using System;
using System.Windows.Controls;
using System.Windows.Input;

using DbReaderDemo.Shell.View;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace DbReaderDemo.Shell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        public UserControl? Done
        {
            get; set;
        }

        public MainViewModel()
        {
            Done=new DbConnection();
        }

        private RelayCommand openFileAndRead;
        public ICommand OpenFileAndRead => openFileAndRead??=new RelayCommand(PerformOpenFileAndRead);

        private void PerformOpenFileAndRead()
        {
            OpenFileDialog ofd = new()
            {
                InitialDirectory=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), // 设置初始目录
                Filter="All files (*.*)|*.*", // 设置文件过滤器
                FilterIndex=1, // 设置默认的文件过滤器索引
                Multiselect=false, // 是否允许多选
                Title="选择一个文件并添加到程序中", // 对话框标题
                CheckFileExists=true, // 是否检查文件是否存在
                CheckPathExists=true, // 是否检查路径是否存在
                RestoreDirectory=true // 是否在对话框关闭之后恢复当前目录
            };

            if(ofd.ShowDialog() is true)
            {
                var path = ofd.FileName;

                if(Done is DbConnection)
                {
                    var vm = (Done as DbConnection).DataContext as DbConnection;





                }




            }
        }
    }
}
