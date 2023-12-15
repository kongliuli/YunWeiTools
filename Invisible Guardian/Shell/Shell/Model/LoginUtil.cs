using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Shell.Model
{
    public class LoginUtil:ObservableObject
    {
        ///获取全部用户id
        ///账号密码macip的多类校验
        ///管理员权限的托管 

        private ObservableCollection<string> _userid = new();
        private ObservableCollection<string> _macAddress = new();
        private ObservableCollection<string> _ipAddress = new();


        #region 属性
        public ObservableCollection<string> UserID
        {
            get => _userid;
            set
            {
                SetProperty(ref _userid,value);
            }
        }

        public ObservableCollection<string> IpAddress
        {
            get => _ipAddress;
            set
            {
                SetProperty(ref _ipAddress,value);
            }
        }
        public ObservableCollection<string> MacAddress
        {
            get => _macAddress;
            set
            {
                SetProperty(ref _macAddress,value);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取全部用户id
        /// </summary>
        /// <returns></returns>
        public LoginUtil GetUserId()
        {
            _userid=new ObservableCollection<string>
            {
                "test","test1","test2","test3"
            };
            return this;
        }

        public LoginUtil GetMacByMachine()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach(NetworkInterface networkInterface in networkInterfaces)
            {
                if(networkInterface.OperationalStatus==OperationalStatus.Up)
                {
                    PhysicalAddress macAddress = networkInterface.GetPhysicalAddress();
                    _macAddress.Add(macAddress.ToString());
                }
            }
            return this;
        }
        public LoginUtil GetIpConfigByMachine()
        {
            string hostName = Dns.GetHostName();

            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            var ipv4 = addresses.FirstOrDefault(a => a.AddressFamily.ToString().Equals("InterNetwork"));

            foreach(IPAddress address in addresses)
            {
                _ipAddress.Add(address.ToString());
            }
            return this;
        }

        public bool TryLoginByPassword(string userid,string psd,string macadress = "",string ipadress = "")
        {
            if(macadress=="")
            {

            }

            if(ipadress=="")
            {

            }

            if(userid==""&&psd=="")
            {
                return true;
            }

            return true;
        }
        #endregion
    }
}
