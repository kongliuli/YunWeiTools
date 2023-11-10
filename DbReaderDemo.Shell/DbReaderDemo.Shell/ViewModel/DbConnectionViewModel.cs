using System.Collections.ObjectModel;
using System.Data;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace DbReaderDemo.Shell.ViewModel
{
    public class DbConnectionViewModel:ObservableObject
    {
        private ObservableCollection<TempleData>? _dataList;
        public ObservableCollection<TempleData>? DataList
        {
            get => _dataList;
            set
            {
                _dataList=value;
            }
        }

        private DataTable _myDataTable;

        public DataTable MyDataTable
        {
            get
            {
                return _myDataTable;
            }
            set
            {
                _myDataTable=value;
                OnPropertyChanged("MyDataTable");
            }
        }

        public DbConnectionViewModel()
        {
            _dataList=new()
            {
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 },
                new TempleData() { Age=0 }
            };
        }

        public void LoadExcelFile(string fullpath)
        {

        }






    }


    public class TempleData
    {
        public string Name
        {
            get; set;
        } = "defaut";
        public int Age
        {
            get; set;
        }
        public bool IsTrue
        {
            get; set;
        } = false;
        public bool IsMan
        {
            get; set;
        } = true;
    }
}
