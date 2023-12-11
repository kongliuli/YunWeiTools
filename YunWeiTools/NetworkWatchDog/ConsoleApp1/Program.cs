using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        static byte[] PING_BUFFER = new byte[] { 0 };

        static int g_nHops = 30;
        static int g_nTimeout = 3000;
        static bool g_bCanceled = false;

        public struct ICMP_PARAM
        {
            internal long m_nSendTicks;
            internal IPAddress m_IPAddress;
            internal PingOptions m_PingOptions;
        }

        static void icmp_PingCompleted(object sender,PingCompletedEventArgs e)
        {
            ICMP_PARAM param = (ICMP_PARAM)e.UserState;
            long nDeltaMS = (DateTime.Now.Ticks-param.m_nSendTicks)/TimeSpan.TicksPerMillisecond;
            Console.WriteLine("{0}:\t{1} ms\t{2}",
                    param.m_PingOptions.Ttl,
                    (e.Reply.Status==IPStatus.TimedOut) ? "*" : nDeltaMS.ToString(),
                    e.Reply.Address.ToString());

            if(param.m_IPAddress.Equals(e.Reply.Address))
            {
                Console.WriteLine("已到达目标地址！");
                g_bCanceled=true;
                return;
            }

            if(param.m_PingOptions.Ttl>=g_nHops)
            {
                Console.WriteLine("已达到最大跃点计数！");
                g_bCanceled=true;
                return;
            }

            Ping icmp = (Ping)sender;
            param.m_PingOptions.Ttl++;
            param.m_nSendTicks=DateTime.Now.Ticks;
            icmp.SendAsync(param.m_IPAddress,g_nTimeout,PING_BUFFER,param.m_PingOptions,param);
        }

        static void Usage()
        {
            Console.WriteLine("Usage: {0} [-h max_hops] [-w timeout] ip/domain",Process.GetCurrentProcess().ProcessName);
        }

        static void Main(string[] args)
        {
            string szDomain = "www.baidu.com";

            Console.CancelKeyPress+=new ConsoleCancelEventHandler(Console_CancelKeyPress);

            ICMP_PARAM param = new ICMP_PARAM();
            param.m_PingOptions=new PingOptions(1,false);
            if(!IPAddress.TryParse(szDomain,out param.m_IPAddress))
            {
                // 解析域名
                try
                {
                    Regex regEx = new Regex("\\d+\\.\\d+\\.\\d+\\.\\d+");
                    IPHostEntry hostEntry = Dns.GetHostEntry(szDomain);
                    foreach(IPAddress ipAddr in hostEntry.AddressList)
                    {
                        if(regEx.IsMatch(ipAddr.ToString()))
                        {
                            param.m_IPAddress=ipAddr;
                            break;
                        }
                    }

                    if(param.m_IPAddress==null)
                    {
                        Usage();
                        return;
                    }

                    Console.WriteLine("正在跟踪到 {0}[{1}] 间的路由：",szDomain,param.m_IPAddress.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }
            else
            {
                Console.WriteLine("正在跟踪到 {0} 间的路由：",param.m_IPAddress.ToString());
            }

            Ping icmp = new Ping();
            icmp.PingCompleted+=new PingCompletedEventHandler(icmp_PingCompleted);

            param.m_nSendTicks=DateTime.Now.Ticks;
            icmp.SendAsync(param.m_IPAddress,g_nTimeout,PING_BUFFER,param.m_PingOptions,param);

            while(!g_bCanceled)
            {
                Thread.Sleep(1);
            }
        }

        static void Console_CancelKeyPress(object sender,ConsoleCancelEventArgs e)
        {
            Console.WriteLine("程序被终止！");
        }

    }
}