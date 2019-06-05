# MyAutoUpdater.WinCE

## Project Introduction
* MyAutoUpdater.WinCE is an auto updater program which is based on WinCE 6.0+ and .NET Framework Compact 2.0+,developed on Visual Studio 2008,designed for WinCE PDA devices,depending on .NET Framework Compact 2.0+, and used for auto updating for applications on WinCE PDA devices.
* MyAutoUpdater.WinCE is a serverless and independent client program which is called by third party applications via process calling with command line args and it's non-intrusive to third party applications.
* MyAutoUpdater.WinCE uses preformatted XML file to store updating parameters.

## Command Line Parameters
* The way to run MyAutoUpdater.WinCE is that you could call process with 5 command line parameters;
* 1st MainExeName:third party application's name for displaying in the winforms of MyAutoUpdater;
* 2nd CurVersion:third party application's current version number(like 1.5.2.123) for comparing with its latest version number to determine upgrading or not;
* 3rd UpdaterUrl:third party application's xml file url(only for HTTP) storing updating params for called by MyAutoUpdater (XML's format follows below);
* 4th MainExePath:third party application's main executable file path for auto running after dealed;
* 5th Silent:if upgrading silent or not(Optional Value:true/false);
* Code by C# follows:

```
using System.Diagnostics;

string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);//MyAutoUpdater.WinCE's startupPath(usually the same as third party application's root folder path)
string updaterFile = Path.Combine(startupPath, "MyAutoUpdater.WinCE.exe");//MyAutoUpdater.WinCE's file path
string mainExeName = "Demo";//third party application's name
string curVersion = "1.5.2.123";//third party application's current version number
string updaterUrl = "http://www.demo.com/updater.xml";//third party application's updating XML file URL
string mainExePath = Path.Combine(startupPath, "demo.exe");//third party application's main executable file path
string silent = "true";//if upgrading silent or not

ProcessStartInfo processInfo = new ProcessStartInfo();
processInfo.FileName = updaterFile;
processInfo.WorkingDirectory = startupPath;
processInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", mainExeName, curVersion, updaterUrl, mainExePath, silent);

Process process = new Process();
process.StartInfo = processInfo;
process.Start();
```
* MyAutoUpdater.WinCE is suitable to called in the init stage of third party application and the third party application should exit after calling.

## XML Format
* XML format like below:

```
<?xml version="1.0" encoding="UTF-8"?>
<item>
    <version>1.5.2.123</version>
    <url>http://www.demo.com/updater.zip</url>
    <changelog>http://www.demo.com/updater.txt</changelog>
    <mandatory>true</mandatory>
</item>
```

* version:third party application's latest version number;
* url:hird party application's packing file URL(only for HTTP) ending with .zip or .cab;
* changelog:url of changelog file(not implemented);
* mandatory:if necessary to upgrade(not implemented);
* MyAutoUpdater will unpack it and cover old files if .zip suffix and install it if .cab suffix.
