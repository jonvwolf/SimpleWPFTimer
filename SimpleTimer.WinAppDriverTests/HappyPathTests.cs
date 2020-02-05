using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

namespace SimpleTimer.WinAppDriverTests
{
    [TestClass]
    public class HappyPathTests
    {
        [TestMethod]
        public void Test0_InitialValues()
        {
            string dir = "";
            using (var app = new AppSession())
            {
                dir = app.FullExePath;
                var mainWindow = app.Session.FindElementByAccessibilityId("MainWindow");
                //first test
                //arrange
                WebDriverWait wait = new WebDriverWait(app.Session, TimeSpan.FromMilliseconds(1200));
                
                var timerTextBox = app.Session.FindElementByAccessibilityId("TextTime_timer");
                var timerStartButton = app.Session.FindElementByAccessibilityId("StartButton_timer");
                var timerResetButton = app.Session.FindElementByAccessibilityId("ResetButton_timer");

                //act

                //assert
                Assert.AreEqual("00:00:00", timerTextBox.Text);
                Assert.AreEqual("Start", timerStartButton.Text);
                Assert.AreEqual("Reset", timerResetButton.Text);

                //second test

                //arrange
                string buttonText = "";
                string okText = "";
                string startText = "";
                string okResetText = "";
                
                //act
                timerTextBox.SendKeys(Keys.Control + "a" + Keys.Control);
                timerTextBox.SendKeys(Keys.Delete);
                timerTextBox.SendKeys("01");

                timerStartButton.Click();
                wait.Until(x => { return timerStartButton.Text.Contains("Stop"); });
                buttonText = timerStartButton.Text;

                wait.Until(x => { return timerStartButton.Text.Contains("OK"); });
                okText = timerStartButton.Text;
                timerStartButton.Click();

                wait.Until(x => { return timerStartButton.Text.Contains("Start"); });

                startText = timerStartButton.Text;

                timerResetButton.Click();
                wait.Until(x => { return timerStartButton.Text.Contains("OK"); });
                okResetText = timerStartButton.Text;
                timerStartButton.Click();

                timerTextBox.SendKeys(Keys.Control + "a" + Keys.Control);
                timerTextBox.SendKeys(Keys.Delete);
                timerTextBox.SendKeys("01");

                //assert
                Assert.AreEqual("Stop", buttonText);
                Assert.AreEqual("OK", okText);
                Assert.AreEqual("Start", startText);
                Assert.AreEqual("OK", okResetText);


                //third test
                mainWindow.SendKeys(Keys.Alt + Keys.Right + Keys.Alt);
                WebDriverWait secondWait = new WebDriverWait(app.Session, TimeSpan.FromMilliseconds(2000));

                WindowsElement swTextBox = app.Session.FindElementByAccessibilityId("TextTime_sw");
                WindowsElement swStartButton = app.Session.FindElementByAccessibilityId("StartButton_sw");
                WindowsElement swResetButton = app.Session.FindElementByAccessibilityId("ResetButton_sw");

                secondWait.Until(x => { return !swTextBox.Enabled && swTextBox.Displayed; });

                Assert.AreEqual("00:00:00", swTextBox.Text);

                swStartButton.Click();
                secondWait.Until(x => { return swTextBox.Text.Contains("00:00:01"); });

                swResetButton.Click();
                secondWait.Until(x => { return swTextBox.Text.Contains("00:00:00"); });

                swStartButton.Click();
                secondWait.Until(x => { return swStartButton.Text.Contains("Start"); });
            }

            dir = Path.GetDirectoryName(dir);
            string[] files = Directory.GetFiles(dir, "*errorlog*", SearchOption.AllDirectories);
            Assert.AreEqual(0, files.Length);
        }

    }
}
