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
using Xunit.Abstractions;

namespace SimpleTimer.WinAppDriverTests
{
    public class AppSession : IDisposable
    {
        private const string WinAppDriverUrl = "http://127.0.0.1:4723";
        private static WindowsDriver<WindowsElement> _session { get; set; }
        private readonly static object _lock = new object();
        public WindowsDriver<WindowsElement> Session { get => _session; }
        public Dictionary<string, WindowsElement> Elements { get; private set; } = new Dictionary<string, WindowsElement>();
        public AppSession()
        {
            Setup();
        }

        protected void Setup()
        {
            lock (_lock)
            {
                if (_session == null)
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
                            if (file.Contains("bin", StringComparison.OrdinalIgnoreCase))
                            {
                                if (file.Contains("debug", StringComparison.OrdinalIgnoreCase) || file.Contains("release", StringComparison.OrdinalIgnoreCase))
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
                    appId = @"D:\a\r1\a\_jonwolfdev_SimpleWPFTimer\SimpleTimer\bin\Debug\netcoreapp3.1\SimpleTimer.exe";
                    var options = new AppiumOptions();
                    options.AddAdditionalCapability("app", appId);

                    _session = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), options);

                    Assert.Contains("Simple Timer", _session.Title);
                    Assert.NotNull(_session);
                    Assert.NotNull(_session.SessionId);

                    _session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
                }
            }
        }

        protected void Shutdown()
        {
            lock (_lock)
            {
                if (_session == null)
                    return;
                _session.Close();
                _session.Quit();
                _session = null;
            }
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
