using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        #region 属性字段
        private string? _userName;

        public string? UserName
        {
            get => _userName;
            set => SetProperty(ref _userName,value);
        }

        private int _age;

        public int Age
        {
            get => _age;

            set => SetProperty(ref _age,value);
        }
        #endregion

        /// <summary>
        /// 按钮点击命令
        /// </summary>
        public ICommand BtnClick
        {
            get; set;
        }

        public MainViewModel()
        {
            BtnClick=new RelayCommand<string>((obj) => DoRun(obj));
        }

        private void DoRun(string? obj)
        {
            Task.Run(() =>
            {
                int i = 0;
                while(true)
                {
                    i++;
                    Task.Delay(1000).GetAwaiter().GetResult();
                    Age=i;
                }
            });
        }



    }
}
