using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace NetworkWatchDog.Shell.Model
{
    public class TabControlItemsGroup:INotifyPropertyChanged
    {
        private List<TabItem> _items = new();

        public string GroupTitle
        {
            get; set;
        } = "defaut";
        public List<TabItem> Items
        {
            get => _items;
            set
            {
                _items=value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TabItem:INotifyPropertyChanged
    {
        private StringStreamInfomation _info = new();

        public string? Header
        {
            get; set;
        }

        public UserControl? Content
        {
            get; set;
        } = null;

        public StringStreamInfomation Info
        {
            get => _info; set
            {
                _info=value;
                OnPropertyChanged(nameof(Info));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
