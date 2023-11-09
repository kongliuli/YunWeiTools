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
        public string reportGroup
        {
            get; set;
        } = "";
        public string reportUser
        {
            get; set;
        } = "";
        public string reportUri
        {
            get; set;
        } = "";
    }

}
