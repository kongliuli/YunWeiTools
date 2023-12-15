using System.Collections.ObjectModel;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Shell.Model
{
    public class LoginUtil:ObservableObject
    {
        ///获取全部用户id
        ///账号密码macip的多类校验
        ///管理员权限的托管 

        public ObservableCollection<string> UserID
        {
            get => _userid; set
            {
                SetProperty(ref _userid,value);
            }
        }
        private ObservableCollection<string> _userid = new();
        /// <summary>
        /// 获取全部用户id
        /// </summary>
        /// <returns></returns>
        public LoginUtil GetUserId()
        {
            _userid=new ObservableCollection<string>
            {
                "test","test1","test2","test3"
            };
            return this;
        }





    }
}
