﻿using System.Windows.Input;

using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        private IpSnifferConfig? _ipsnifferconfig;
        private IConfigurationRoot? builder;
        private ReportConfig? _reportConfig;

        public ICommand RefalshCommand
        {
            get
            {
                return new RelayCommand(InitConfig);
            }
        }

        public MainViewModel()
        {
            InitConfig();
        }

        private void InitConfig()
        {
            //配置文件读取
            builder=new ConfigurationBuilder()
                       .AddJsonFile("Configuartions/IpSnifferConfig.json",optional: true,reloadOnChange: true)
                       .AddJsonFile("Configuartions/ReportConfig.json",optional: true,reloadOnChange: true)
                       .Build();

            _ipsnifferconfig=builder.GetSection("IpSnifferConfig").Get<IpSnifferConfig>();
            _reportConfig=builder.GetSection("ReportConfig").Get<ReportConfig>();

            //配置传递
            ViewModelLocator._ipsnifferconfig=_ipsnifferconfig;
            ViewModelLocator._reportConfig=_reportConfig;

        }
    }
}
