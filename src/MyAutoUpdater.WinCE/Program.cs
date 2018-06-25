using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MyAutoUpdater.Common;
using System.IO;
using System.Diagnostics;

namespace MyAutoUpdater
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main(string[] args)
        {
            if (null == args || args.Length < 5)
            {
                Logger.Log("ERROR", "Lacks of Startup Params", null);
                return;
            }
            Constants.MainExeName = args[0].Trim();
            Constants.CurVersion = args[1].Trim();
            Constants.UpdaterUrl = args[2].Trim();
            Constants.MainExePath = args[3].Trim();
            Constants.Silent = bool.Parse(args[4].Trim());

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);          
            Application.Run(new FormMain());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException((Exception)(e.ExceptionObject));
        }

        /// <summary>
        /// 记录代码异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        static void LogException(Exception ex)
        {
            Logger.Log("ERROR", ex.Message, ex);

            if (File.Exists(Constants.MainExePath))
            {
                Process.Start(Constants.MainExePath, "");
            }
            else
            {
                Logger.Log("WARN", "Non-existent of MainExePath", null);
            }
            Application.Exit();
        }
    }
}