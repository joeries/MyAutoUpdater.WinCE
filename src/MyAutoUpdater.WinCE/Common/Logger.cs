using System;
using System.IO;
using System.Reflection;

namespace MyAutoUpdater.Common
{
    public class Logger
    {
        public static void Log(string level, string message, Exception ex)
        {
            string logFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "updater.log");
            using (StreamWriter sw = File.AppendText(logFileName))
            {
                sw.WriteLine(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), level, message));
                if (null != ex)
                {
                    sw.WriteLine("Exception: " + ex.StackTrace);
                    if (null != ex.InnerException)
                    {
                        sw.WriteLine("InnerException: " + ex.InnerException.StackTrace);
                    }
                }
                sw.WriteLine();
            }
        }
    }
}