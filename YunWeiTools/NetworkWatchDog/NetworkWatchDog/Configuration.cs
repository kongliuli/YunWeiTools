using System.Net.NetworkInformation;

namespace NetworkWatchDog
{
    public class Configuration
    {
        public BaseSetting baseSetting
        {
            get; set;
        } = new();

        public ErrorReport errorReport
        {
            get; set;
        } = new();
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

    public class IpGroup
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

            string message = $"{DateTime.Now:yyyy-MM-mm HH:mm:ss} {Ipconfig}";
            if(pr!=null)
            {
                if(pr.Status==IPStatus.Success)
                {
                    info.isSuccess=true;
                    if(pr.RoundtripTime>=timespan)
                    {
                        info.isSuccess=false;
                    }
                    message+=$": 字节={pr.Buffer.Length} 时间={pr.RoundtripTime}ms TTL={pr.Options?.Ttl}";
                }
                else
                {
                    info.isSuccess=false;
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

        public ErrorInfo GetDoneInfo() => doneInfo;
    }

    public class ErrorReport
    {
        public bool isReportError
        {
            get; set;
        }

        public int ReportMinTimes
        {
            get; set;
        }
        public int SkipTime
        {
            get; set;
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
