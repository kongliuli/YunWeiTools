using System.Collections.ObjectModel;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Shell.View;

namespace Shell.ViewModel
{
    public class LoginViewModel:ObservableObject
    {
        public LoginViewModel()
        {
        }

        #region 初始化字段
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password,value);
            }
        }

        private ObservableCollection<string> _userid = new();
        public ObservableCollection<string> UserID
        {
            get => _userid;
            set
            {
                SetProperty(ref _userid,value);
            }
        }
        #endregion

        #region 方法绑定
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        public ICommand ChanelCommand
        {
            get
            {
                return new RelayCommand(Chanel);
            }
        }
        private void Login()
        {
            if(true)
            {
                var a = new MainView();
                a.Show();

            }
        }
        private void Chanel()
        {

        }
        #endregion

        #region

        #endregion


    }
}
