using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace NetworkWatchDog.Shell.Model
{
    public class StringStreamInfomation:INotifyPropertyChanged
    {
        private ObservableCollection<PingRelayInfo> _infos;

        public ObservableCollection<PingRelayInfo> Infos
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
            _infos=new ObservableCollection<PingRelayInfo>();
        }

        public int Delete1234
        {
            get; set;
        } = 10000;
        public int DeleteCount
        {
            get; set;
        } = 100;

        public void ADDInfo(PingRelayInfo info)
        {
            if(Infos.Count>Delete1234)
            {
                Infos.RemoveAt(0);
            }
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,new Action<PingRelayInfo>((x) => { Infos.Add(x); }),info);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
    public class PingRelayInfo
    {
        public PingRelayInfo(string ip)
        {
            this.IPConfig=ip;
        }
        public string GetContext()
        {
            return $"{DateTime:yyyy-MM-dd HH:mm:ss}  {IPConfig}  {IPName} : {Infomation}";
        }
        public DateTime DateTime
        {
            get; set;
        } = DateTime.Now;

        public string IPName
        {
            get; set;
        } = string.Empty;

        public string IPConfig
        {
            get; set;
        }

        public string Infomation
        {
            get; set;
        } = string.Empty;
    }
}
