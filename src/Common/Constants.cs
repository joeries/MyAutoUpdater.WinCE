namespace MyAutoUpdater.Common
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 主程序名称
        /// </summary>
        public static string MainExeName { get; set; }
        /// <summary>
        /// 当前版本号
        /// </summary>
        public static string CurVersion { get; set; }
        /// <summary>
        /// 升级URL
        /// </summary>
        public static string UpdaterUrl { get; set; }
        /// <summary>
        /// 主程序路径
        /// </summary>
        public static string MainExePath { get; set; }
        /// <summary>
        /// 是否静默升级
        /// </summary>
        public static bool Silent { get; set; }
    }
}
