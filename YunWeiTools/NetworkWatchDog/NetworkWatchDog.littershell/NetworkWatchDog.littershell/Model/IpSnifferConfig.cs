using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace NetworkWatchDog.Shell.Model
{
    public class IpSnifferConfig
    {
        public BaseSetting BaseSetting
        {
            get; set;
        }
    }
    public class BaseSetting
    {
        public int IntranettripTime
        {
            get; set;
        }
        public List<IpGroup> NetworkGroup
        {
            get; set;
        }
        public int ExternaltripTime
        {
            get; set;
        }

        public int TimeOut
        {
            get; set;
        } = 5000;

        public int PingTimer
        {
            get; set;
        } = 1000;
        public int BuffurMaxLine
        {
            get; set;
        } = 10000;
    }

    public class IpGroup:ObservableObject, INotifyPropertyChanged
    {
        #region 基础信息
        public string Ipconfig
        {
            get; set;
        }
        public bool isMachine
        {
            get; set;
        }
        public string IpName
        {
            get; set;
        }

        public string MachineInfo
        {
            get; set;
        }

        public string MachineLocation
        {
            get; set;
        }

        public bool isintra
        {
            get; set;
        } = true;
        private bool _isLastTrue = false;
        public bool IsLastTrue
        {
            get => _isLastTrue;
            set
            {
                SetProperty(ref _isLastTrue,value);
            }
        }
        #endregion

        public DateTime ErrorReportTime
        {
            get; set;
        } = DateTime.Now.AddDays(-1);
        private List<ErrorInfo> infos
        {
            get; set;
        } = new List<ErrorInfo>();

        private ErrorInfo doneInfo
        {
            get; set;
        } = new();

        public IpGroup GetReply(PingReply pr,int timespan)
        {
            ErrorInfo info = new()
            {
                ErrorTime=DateTime.Now
            };

            string message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {Ipconfig}";
            if(pr!=null)
            {
                if(pr.Status==IPStatus.Success)
                {
                    info.isSuccess=true;
                    IsLastTrue=true;
                    if(pr.RoundtripTime>=timespan)
                    {
                        IsLastTrue=false;
                        info.isSuccess=false;
                    }
                    message+=$": 字节={pr.Buffer.Length} 时间={pr.RoundtripTime}ms TTL={pr.Options?.Ttl}";
                }
                else
                {
                    info.isSuccess=false;
                    IsLastTrue=false;
                    message+=$" {pr.Status}";
                }
            }
            info.ErrorReportContent=message;
            doneInfo=info;

            return this.AddError(info.isSuccess);
        }

        private IpGroup AddError(bool issuccess,int Min = 1)
        {
            DateTime Mintime = DateTime.Now.AddMinutes(-Min);
            if(!issuccess)
            {
                infos.Add(doneInfo);
            }
            infos.RemoveAll(x => x.ErrorTime<Mintime);
            return this;
        }

        public bool TryReport(out string reportvalue,int reporttimes = 5,int timespan = 20)
        {
            if(this.infos.Count>=reporttimes&&ErrorReportTime<DateTime.Now.AddMinutes(-timespan)&&isMachine)
            {
                ErrorReportTime=DateTime.Now;
                infos=new();
                reportvalue=$"测试信息  {IpName} ap连接故障报警：\r\n\r\nip：{Ipconfig}\r\n型号：{MachineInfo}\r\n位置：{MachineLocation}\r\n信息：此ap一分钟内出现了{reporttimes}次连接故障。请登录向日葵查看详情\r\n";
                return true;
            }
            reportvalue="";
            return false;
        }
        public bool TryReportMarkdown(out string reportvalue,int reporttimes = 5,int timespan = 20)
        {
            if(this.infos.Count>=reporttimes&&ErrorReportTime<DateTime.Now.AddMinutes(-timespan)&&isMachine)
            {
                reportvalue=$@"### 网络连通性报警
>- 报警设备:无线ap-{IpName}
>- IP地址:{Ipconfig}
>- 设备信息:{MachineInfo}
>- 设备位置:{MachineLocation}

>- 报警内容:{(isintra ? "内网" : "外网")}测试连通性发生多次异常
>- 报警阈值:在一分钟内访问基准地址超过失败{reporttimes}次,失败标准为延迟超过{(isintra ? 100 : 150)}ms

失败信息
";
                foreach(var info in this.infos)
                {
                    reportvalue+=$"{info.ErrorReportContent}\n";
                }

                ErrorReportTime=DateTime.Now;
                infos=new();
                return true;
            }
            reportvalue="";
            return false;
        }

        public ErrorInfo GetDoneInfo() => doneInfo;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }


    public class ErrorInfo
    {
        public DateTime ErrorTime
        {
            get; set;
        }
        public bool isSuccess
        {
            get; set;
        } = false;
        public string ErrorReportContent
        {
            get; set;
        } = "";
    }

}
