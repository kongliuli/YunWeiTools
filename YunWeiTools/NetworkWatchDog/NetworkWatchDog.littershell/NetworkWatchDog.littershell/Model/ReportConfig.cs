namespace NetworkWatchDog.Shell.Model
{
    public class ReportConfig
    {
        public ErrorReport errorReport
        {
            get; set;
        }

        public ReportRule reportRule
        {
            get; set;
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

    public class ReportRule
    {
        public string RetportGroupName
        {
            get; set;
        } = "";
        public string DingTalkUri
        {
            get; set;
        } = "https://oapi.dingtalk.com/robot/send";
        public string Token
        {
            get; set;
        } = "";
        public string SecretInfo
        {
            get; set;
        } = "";
    }

}
