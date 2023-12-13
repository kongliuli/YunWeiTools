namespace NetworkWatchDog.littershell.Model
{
    public class TarouteModel
    {
        //序号
        public int ttl
        {
            get; set;
        }
        //ip地址
        public string IpAddress
        {
            get; set;
        }
        //接收到包的数量
        public int ReceivePackage
        {
            get; set;
        }
        //耗时
        public string nDeltaMs
        {
            get; set;
        }
    }
}
