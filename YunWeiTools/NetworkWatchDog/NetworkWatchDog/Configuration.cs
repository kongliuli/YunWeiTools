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
        } = DateTime.Now;
        private List<ErrorInfo> infos
        {
            get; set;
        } = new List<ErrorInfo>();

        public IpGroup AddError(ErrorInfo errorInfo,int Min)
        {
            DateTime Mintime = DateTime.Now.AddMinutes(-Min);
            infos.Add(errorInfo);
            infos.RemoveAll(x => x.ErrorTime<Mintime);

            return this;
        }

        public bool TryReport()
        {
            if(this.infos.Count>=20)
            {
                return true;
            }
            return false;
        }

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
        public string ErrorReportContent
        {
            get; set;
        } = "";
    }
}
