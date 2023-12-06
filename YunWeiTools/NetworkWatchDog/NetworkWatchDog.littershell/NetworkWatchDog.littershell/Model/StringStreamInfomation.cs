using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace NetworkWatchDog.Shell.Model
{
    public class StringStreamInfomation:INotifyPropertyChanged
    {
        private ObservableCollection<string> _infos;

        public ObservableCollection<string> Infos
        {
            get => _infos;
            set
            {
                _infos=value;
                OnPropertyChanged(nameof(Infos));
            }
        }

        public StringStreamInfomation()
        {
            _infos=new ObservableCollection<string>();
        }

        public int Delete1234
        {
            get; set;
        } = 10000;
        public int DeleteCount
        {
            get; set;
        } = 100;

        public void ADDInfo(string info)
        {
            if(Infos.Count>Delete1234)
            {
                Infos.RemoveAt(0);
            }
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,new Action<string>((x) => { Infos.Add(x); }),info);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
