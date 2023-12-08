using NetworkWatchDog.Shell.Model;

namespace NetworkWatchDog.littershell.ViewModel
{
    public class MTRouterViewModel
    {
        public IpGroup DoneIP
        {
            get; set;
        }

        public MTRouterViewModel()
        {
            DoneIP=new();

        }


    }
}
