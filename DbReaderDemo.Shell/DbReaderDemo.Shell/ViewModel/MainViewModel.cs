using System.Windows.Controls;

using DbReaderDemo.Shell.View;

using Microsoft.Toolkit.Mvvm.ComponentModel;

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
    }
}
