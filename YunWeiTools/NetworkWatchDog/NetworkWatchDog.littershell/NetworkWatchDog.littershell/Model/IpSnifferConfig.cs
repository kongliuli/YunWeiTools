﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;

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
    public class BaseSetting:ObservableObject
    {
        public int IntranettripTime
        {
            get; set;
        }
        public ObservableCollection<IpGroup> NetworkGroup
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
        } = string.Empty;
        public bool isMachine
        {
            get; set;
        } = false;
        public string IpName
        {
            get; set;
        } = string.Empty;

        public string MachineInfo
        {
            get; set;
        } = string.Empty;

        public string MachineLocation
        {
            get; set;
        } = string.Empty;

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
                if(_isLastTrue!=value)
                {
                    _isLastTrue=value;
                    OnPropertyChanged(nameof(IsLastTrue));
                }
            }
        }


        private bool _isPingEnable = true;
        public bool IsPingEnabled
        {
            get => _isPingEnable;
            set
            {
                if(_isPingEnable!=value)
                {
                    _isPingEnable=value;
                    OnPropertyChanged(nameof(IsPingEnabled));
                }
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

        private string pingFormat = "字节={0} 时间={1}ms TTL={2}";
        public IpGroup GetReply(PingReply pr,int timespan)
        {
            ErrorInfo info = new()
            {
                ErrorTime=DateTime.Now
            };

            PingRelayInfo msg = new(Ipconfig)
            {
                IPName=IpName
            };
            if(pr!=null)
            {
                if(pr.Status==IPStatus.Success)
                {
                    info.isSuccess=true;
                    if(pr.RoundtripTime>=timespan)
                    {
                        info.isSuccess=false;
                    }
                    msg.Infomation=string.Format(pingFormat,pr.Buffer.Length,pr.RoundtripTime,pr.Options?.Ttl);
                }
                else
                {
                    if(pr.Status==IPStatus.DestinationHostUnreachable)
                    {
                        msg.Infomation=$"主机不可达";
                    }
                    else
                    {
                        msg.Infomation=$" {pr.Status}";
                    }
                }
            }
            info.ErrorReportContent=msg;
            doneInfo=info;

            return this.AddError(info.isSuccess);
        }
        /// <summary>
        /// 添加错误日志,并且规制错误日志数量
        /// </summary>
        /// <param name="issuccess">是否失败</param>
        /// <param name="Min">限制错误日志有效时间</param>
        /// <returns></returns>
        private IpGroup AddError(bool issuccess,int Min = 1)
        {
            IsLastTrue=issuccess;
            DateTime Mintime = DateTime.Now.AddMinutes(-Min);
            if(!issuccess)
            {
                infos.Add(doneInfo);
            }
            try
            {
                infos.RemoveAll(x => x.ErrorTime<Mintime);
            }
            catch
            {

            }
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
                StringBuilder markdown = new();
                markdown.AppendLine("### 网络连通性报警");
                markdown.AppendLine($"报警设备:无线ap:{IpName}");
                markdown.AppendLine($"IP地址:{Ipconfig}");
                markdown.AppendLine($"设备信息:{MachineInfo}");
                markdown.AppendLine($"设备位置:{MachineLocation}");
                markdown.AppendLine(" ");
                markdown.AppendLine($"报警内容:{(isintra ? "内网" : "外网")}测试连通性发生多次异常");
                markdown.AppendLine($"报警阈值:在一分钟内访问IP地址失败超过{reporttimes}次,失败标准为延迟超过{(isintra ? 100 : 150)}ms");
                markdown.AppendLine(" ");
                markdown.AppendLine("失败信息");

                foreach(var info in this.infos)
                {
                    markdown.AppendLine($">- {info.ErrorReportContent.GetContext()}");
                }
                reportvalue=markdown.ToString();
                ErrorReportTime=DateTime.Now;
                infos=new();
                return true;
            }
            reportvalue="";
            return false;
        }

        public ErrorInfo GetDoneInfo() => doneInfo;

        public new event PropertyChangedEventHandler? PropertyChanged;
        protected new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }


    public class ErrorInfo
    {
        public DateTime ErrorTime
        {
            get; set;
        } = DateTime.Now;
        public bool isSuccess
        {
            get; set;
        } = false;
        public PingRelayInfo ErrorReportContent
        {
            get; set;
        }
    }

}
