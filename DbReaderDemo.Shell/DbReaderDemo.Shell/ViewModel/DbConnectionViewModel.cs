using System.Collections.ObjectModel;

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
