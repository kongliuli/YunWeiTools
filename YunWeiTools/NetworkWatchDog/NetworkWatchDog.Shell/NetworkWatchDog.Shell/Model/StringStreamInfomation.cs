using System.Collections.Generic;

namespace NetworkWatchDog.Shell.Model
{
    public class StringStreamInfomation
    {
        public List<string> Infos
        {
            get; set;
        } = new();

        public int Delete1234
        {
            get; set;
        } = 10000;
        public int DeleteCount
        {
            get; set;
        } = 100;

        public void ADDInfo(string info)
        {
            if(Infos.Count>Delete1234)
            {
                Infos.RemoveRange(0,DeleteCount);
            }
            Infos.Add(info);
        }
    }
}
