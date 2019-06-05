# MyAutoUpdater.WinCE

## 项目介绍
* MyAutoUpdater.WinCE是一个基于WinCE 6.0+和.NET Framework Compact 2.0+的自动升级程序，使用Visual Studio 2008开发，运行于WinCE 6.0+手持设备之上，依赖于.NET Framework Compact 2.0+，用于手持设备应用软件的自动升级。
* MyAutoUpdater.WinCE是一个无服务端的、独立的客户端应用程序，被第三方应用软件通过进程调用传递命令行参数的方式来运行，对第三方应用软件代码无侵入。
* MyAutoUpdater.WinCE使用预定义格式的XML配置文件来获得升级信息，只需将第三方应用软件的待升级文件打包成单独的压缩文件并配置可访问的XML文件即可。

## 命令行参数
* MyAutoUpdater.WinCE通过进程调用传递命令行参数的方式来运行，共5个命令行参数；
* 第1个命令行参数MainExeName：第三方应用软件的名称，用于显示在自动升级程序中；
* 第2个命令行参数CurVersion：第三方应用软件的当前版本号（格式如：1.5.2.123），用于与最新版本号进行比对决定是否需要升级；
* 第3个命令行参数UpdaterUrl：第三方应用软件升级配置信息XML文件的可访问URL（目前仅支持HTTP协议），用于被自动升级程序访问（格式详见后文）；
* 第4个命令行参数MainExePath：第三方应用软件主文件的绝对路径，用于升级处理完毕后自动运行第三方应用软件；
* 第5个命令行参数Silent：是否静默升级（可选值：true/false）；
* 以C#调用为例，大致代码如下：

```
using System.Diagnostics;

string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);//自动升级程序主文件所在目录（通常放在第三方应用软件的根目录下）
string updaterFile = Path.Combine(startupPath, "MyAutoUpdater.WinCE.exe");//自动升级程序主文件的绝对路径
string mainExeName = "演示";//第三方应用软件的名称
string curVersion = "1.5.2.123";//第三方应用软件的当前版本号
string updaterUrl = "http://www.demo.com/updater.xml";//第三方应用软件升级配置信息XML文件的可访问URL
string mainExePath = Path.Combine(startupPath, "demo.exe");//第三方应用主文件的绝对路径
string silent = "true";//是否静默升级

ProcessStartInfo processInfo = new ProcessStartInfo();
processInfo.FileName = updaterFile;
processInfo.WorkingDirectory = startupPath;
processInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", mainExeName, curVersion, updaterUrl, mainExePath, silent);

Process process = new Process();
process.StartInfo = processInfo;
process.Start();
```
* 本自动升级程序适用于在第三方应用软件初始化运行时调用，调用完毕后即退出（可自定义实现一定时段内再次运行不调用自动升级程序），待升级处理完毕后自动运行。

## 升级配置XML文件格式
* 采用XML格式，示例如下：

```
<?xml version="1.0" encoding="UTF-8"?>
<item>
    <version>1.5.2.123</version>
    <url>http://www.demo.com/updater.zip</url>
    <changelog>http://www.demo.com/updater.txt</changelog>
    <mandatory>true</mandatory>
</item>
```

* version：第三方应用软件的最新版本号；
* url：第三方应用软件的升级包URL（目前仅支持HTTP协议），目前仅支持.zip、.cab两种格式；
* changelog：升级日志的URL，用于说明本次升级做了哪些修改和优化，暂未实现可忽略；
* mandatory：是否必须升级，暂未实现可忽略；
* 若是.zip格式，自动升级程序将自动解压并覆盖原文件；若是.cab格式，自动升级程序将自动运行此安装程序并覆盖安装。
