using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SimpleTimer.WinAppDriverTests
{
    public class AppSession : IDisposable
    {
        private const string WinAppDriverUrl = "http://127.0.0.1:4723";
        
        public WindowsDriver<WindowsElement> Session { get; private set; }
        public string FullExePath { get; private set; }
        public AppSession()
        {
            Setup();
        }

        private void Setup()
        {
            if (Session == null)
            {
                string appId = "";
                string folder = @"..\..\..\..\";
                string[] filesFound = Directory.GetFiles(folder, "*SimpleTimer.exe", SearchOption.AllDirectories);
                if (filesFound.Length == 0)
                {
                    throw new InvalidOperationException("Not able to found SimpleTimer.exe file");
                }
                if (filesFound.Length >= 2)
                {
                    foreach (var file in filesFound)
                    {
                        if (file.Contains(@"SimpleTimer" + Path.DirectorySeparatorChar + "bin", StringComparison.OrdinalIgnoreCase))
                        {
                            if (file.Contains("debug", StringComparison.OrdinalIgnoreCase) || file.Contains("release", StringComparison.OrdinalIgnoreCase))
                            {
                                appId = Path.GetFullPath(file);
                            }
                        }
                    }
                }
                else
                {
                    appId = Path.GetFullPath(filesFound[0]);
                }
                if (string.IsNullOrEmpty(appId))
                {
                    throw new InvalidOperationException("No SimpleTimer.exe found in a bin/debug or bin/release folder");
                }
                
                var options = new AppiumOptions();
                options.AddAdditionalCapability("app", appId);
                options.AddAdditionalCapability("deviceReadyTimeout", 5);
                Session = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);

                Assert.AreEqual("Simple Timer", Session.Title);
                Assert.IsNotNull(Session);
                Assert.IsNotNull(Session.SessionId);

                Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
                FullExePath = appId;
            }
        }

        private void Shutdown()
        {
            if (Session == null)
                return;
            Session.Close();
            Session.Quit();
            Session = null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Shutdown();
                }

                disposedValue = true;
            }
        }

        ~AppSession()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
