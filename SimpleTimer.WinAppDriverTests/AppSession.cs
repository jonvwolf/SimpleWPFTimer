using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace SimpleTimer.WinAppDriverTests
{
    public class AppSession
    {
        const string WinAppDriverUrl = "http://127.0.0.1:4723";
        protected static WindowsDriver<WindowsElement> session;

        [Fact]
        public void Setup()
        {
            if(session == null)
            {
                string appId = "";
                string folder = @"..\..\..\..\";
                string[] filesFound = Directory.GetFiles(folder, "*SimpleTimer.exe", SearchOption.AllDirectories);
                if(filesFound.Length == 0)
                {
                    throw new InvalidOperationException("Not able to found SimpleTimer.exe file");
                }
                if(filesFound.Length >= 2)
                {
                    foreach(var file in filesFound)
                    {
                        if (file.Contains("bin", StringComparison.OrdinalIgnoreCase))
                        {
                            if(file.Contains("debug", StringComparison.OrdinalIgnoreCase) || file.Contains("release", StringComparison.OrdinalIgnoreCase))
                            {
                                appId = Path.GetFullPath(file);
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(appId))
                {
                    throw new InvalidOperationException("No SimpleTimer.exe found in a bin/debug or bin/release folder");
                }

                var ok = new AppiumOptions();
                ok.AddAdditionalCapability("app", appId);
                
                session = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), ok);
                Thread.Sleep(500);
                Assert.NotNull(session);
                Assert.NotNull(session.SessionId);
                Thread.Sleep(500);
                session.Close();
                session.Quit();
                session = null;
            }
        }

    }
}
