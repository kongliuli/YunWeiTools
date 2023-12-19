using System;
using System.ComponentModel;
using System.Windows.Input;

using HandyControl.Tools.Command;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Shell.ViewModel
{
    public class WebBrowserViewModel:ObservableObject, INotifyPropertyChanged
    {
        private string _url_t;
        public string Url_Text
        {
            get => _url_t;
            set
            {
                if(_url_t!=value)
                {
                    _url_t=value;
                    OnPropertyChanged(nameof(Url_Text));
                }
            }
        }

        private Uri _uri;
        public Uri Url
        {
            get => _uri;
            set
            {
                if(_uri!=value)
                {
                    _uri=value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        public ICommand KeyDownCommand
        {
            get
            {
                return new RelayCommand(KeyDownEVENT);
            }
        }

        private void KeyDownEVENT(object obj)
        {
            if(Uri.TryCreate(_url_t,UriKind.Absolute,out Uri uri))
            {
                // 用户输入的文本是一个有效的URI格式
                _uri=uri;

            }
            else
            {
                // 用户输入的文本不是一个有效的URI格式
                // 在这里可以处理错误情况，比如显示一个错误消息给用户
            }
        }
    }
}
