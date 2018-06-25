using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using MyAutoUpdater.Common;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;

namespace MyAutoUpdater.Core
{
    public class BaseEventArgs : EventArgs
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public bool NoRun { get; set; }
    }

    public class ResultEventArgs : BaseEventArgs
    {
        public Exception Exception { get; set; }
    }

    public class CheckedEventArgs : ResultEventArgs
    {
        public string Version { get; set; }
        public string FileUrl { get; set; }
    }

    public class DownloadedEventArgs : ResultEventArgs
    {
        public string FilePath { get; set; }
    }

    public class ProgressEventArgs : BaseEventArgs
    {
        public int Percent { get; set; }
    }

    public delegate void EndHandler(Exception ex, bool noRun);
    public delegate void ProgressHandler(ProgressEventArgs e);

    public static class UpdateHelper
    {
        public static event EndHandler OnEnd;
        public static event ProgressHandler OnProgress;

        private static CheckedEventArgs Check(string curVersion, string updaterUrl)
        {
            CheckedEventArgs args = new CheckedEventArgs();

            try
            {
                HttpWebRequest reqUrl = (HttpWebRequest)WebRequest.Create(updaterUrl);
                using (HttpWebResponse respUrl = (HttpWebResponse)reqUrl.GetResponse())
                {
                    using (Stream stream = respUrl.GetResponseStream())
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(stream);
                        args.Version = xmlDoc.SelectSingleNode("item/version").InnerText.Trim();
                        args.FileUrl = xmlDoc.SelectSingleNode("item/url").InnerText.Trim();
                        args.Code = args.Version == "0.0.0.0" || args.Version.Equals(curVersion) ? "Unavailable" : "Available";
                        args.Desc = args.Version == "0.0.0.0" || args.Version.Equals(curVersion) ? "无可用更新版本" : "有可用更新版本";
                    }
                }
            }
            catch (Exception ex)
            {
                args.Exception = ex;
            }

            return args;
        }

        private static DownloadedEventArgs Download(string fileUrl)
        {
            DownloadedEventArgs args = new DownloadedEventArgs();
            try
            {
                HttpWebRequest reqUrl = (HttpWebRequest)WebRequest.Create(fileUrl);
                using (HttpWebResponse respUrl = (HttpWebResponse)(WebResponse)reqUrl.GetResponse())
                {
                    using (BinaryReader br = new BinaryReader(respUrl.GetResponseStream()))
                    {
                        long fileLenth = respUrl.ContentLength;
                        byte[] fileContent = br.ReadBytes((Int32)fileLenth);
                        string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), Path.GetFileName(fileUrl));
                        BinaryWriter bw = new BinaryWriter(new FileStream(filePath, FileMode.Create));
                        bw.Write(fileContent, 0, (Int32)fileLenth);
                        bw.Close();

                        args.Code = "OK";
                        args.Desc = "下载成功";
                        args.FilePath = filePath;
                    }
                }
            }
            catch (Exception ex)
            {
                args.Exception = ex;
            }
            return args;
        }

        private static ResultEventArgs UnZip(string filePath)
        {
            ResultEventArgs args = new ResultEventArgs();

            string runPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory  
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(Path.Combine(runPath, directoryName)))
                    {
                        Directory.CreateDirectory(Path.Combine(runPath, directoryName));
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            using (FileStream streamWriter = File.Create(Path.Combine(runPath, theEntry.Name + ".bak")))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            File.Copy(Path.Combine(runPath, theEntry.Name + ".bak"), Path.Combine(runPath, theEntry.Name), true);
                            File.Delete(Path.Combine(runPath, theEntry.Name + ".bak"));
                        }
                        catch (Exception ex)
                        {
                            args.Exception = ex;
                        }
                    }
                }
            }
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                args.Exception = ex;
            }

            args.Code = "OK";
            args.Desc = "解压成功";
            return args;
        }

        private static ResultEventArgs ExecuteCab(string filePath)
        {
            ResultEventArgs args = new ResultEventArgs();
            try
            {
                Process.Start(filePath, "");
                args.Code = "OK";
                args.Desc = "执行成功";
                args.NoRun = true;
            }
            catch (Exception ex)
            {
                args.Exception = ex;
            }
            return args;
        }

        private static void DoWork()
        {
            CheckedEventArgs argsCheck = Check(Constants.CurVersion, Constants.UpdaterUrl);
            if (argsCheck.Exception != null)
            {
                if (null != OnEnd) OnEnd(argsCheck.Exception, false);
                return;
            }
            if (argsCheck.Code == "Unavailable")
            {
                if (null != OnProgress) OnProgress(new ProgressEventArgs { Code = "Finished", Desc = argsCheck.Desc, Percent = 100 });
                return;
            }
            if (null != OnProgress) OnProgress(new ProgressEventArgs { Code = "Progressing", Desc = argsCheck.Desc, Percent = 33 });
            DownloadedEventArgs argsDownload = Download(argsCheck.FileUrl);
            if (argsDownload.Exception != null)
            {
                if (null != OnEnd) OnEnd(argsDownload.Exception, false);
                return;
            }
            if (null != OnProgress) OnProgress(new ProgressEventArgs { Code = "Progressing", Desc = argsDownload.Desc, Percent = 66 });
            ResultEventArgs argsResult = null;
            if (argsDownload.FilePath.EndsWith(".zip"))
            {
                argsResult = UnZip(argsDownload.FilePath);
            }
            else if (argsDownload.FilePath.EndsWith(".cab"))
            {
                argsResult = ExecuteCab(argsDownload.FilePath);
            }
            if (null == argsResult)
            {
                if (null != OnProgress) OnProgress(new ProgressEventArgs { Code = "Finished", Desc = "升级成功", Percent = 100 });
                return;
            }
            if (argsResult.Exception != null)
            {
                if (null != OnEnd) OnEnd(argsResult.Exception, false);
                return;
            }
            if (null != OnProgress) OnProgress(new ProgressEventArgs { Code = "Finished", Desc = argsResult.Desc, Percent = 100, NoRun = argsResult.NoRun });
        }

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                DoWork();
            }));
        }
    }
}