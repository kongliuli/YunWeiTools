using System;
using System.IO;

namespace NetworkWatchDog.Shell.Model
{
    public static class LogManager
    {
        public static DateTime time = DateTime.Now;
        public static string Path = Directory.GetCurrentDirectory();
        static LogManager()
        {

        }

        public static void WriteLog(string Log,string LogName = "info")
        {
            string dirPath = $"{Path}/{time:yyyy}/{LogName}";
            string filename = $"{dirPath}/{time:MMdd}.log";

            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            bool fileCreated = false;
            FileStream fileStream = null;
            try
            {
                if(!File.Exists(filename))
                {
                    fileStream=File.Create(filename);
                }
            }
            finally
            {
                fileStream?.Close();
                fileCreated=true;
            }

            if(fileCreated)
            {
                using StreamWriter sw = File.AppendText(filename);
                sw.WriteLine(Log);
            }
        }
    }
}
