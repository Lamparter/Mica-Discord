# Mica-Discord

An application with Mica background using transparent-background WebView2 to host webpage, and go to discord and inject some basic CSS.

# Requirements
Windows 11 Build 22523+ (Other Windows version are not officially tested)

# How to use?
1. Download Evergreen WebView2, .NET 6 Desktop Runtime, and Windows App SDK 1.0.3 if you haven't already
- Evergreen https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section
- .NET 6 Desktop Runtime https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Windows App SDK 1.0.3: https://aka.ms/windowsappsdk/1.0/1.0.3/windowsappruntimeinstall-1.0.3-x64.exe
2. Go to the release page, download it, and double click setup.exe (OR you can download the source and compile it yourself since the dev is ~lazy~ I mean busy.)

# Disclaimer
If you plan to use this with replace background enabled, I'm not sure if it violates Discord TOS or not. I'm not a lawyer. But here's how it works
1. The app itself is basically like a web browser, hosting the web content in WebView2 that points to discord.com
2. It injects a javascript code, which can be found somewhere here https://github.com/Get0457/Mica-Discord/blob/master/Mica%20Discord/MainWindow.xaml.cs.

**If you use any part of this software, regardless of having any option on or off, it's your responsibility if you get banned or punished by Discord.** But the app is not harmful itself and the security should as safe as other apps using Microsoft Edge WebView2.
