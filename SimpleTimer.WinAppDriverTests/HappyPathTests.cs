using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace SimpleTimer.WinAppDriverTests
{
    public class HappyPathTests : IClassFixture<AppSession>
    {
        AppSession _app;
        WindowsElement _timerTextBox;
        WindowsElement _timerStartButton;
        WindowsElement _timerResetButton;
        public HappyPathTests(AppSession app)
        {
            _app = app;

            _timerTextBox = _app.Session.FindElementByAccessibilityId("TextTime");
            _timerResetButton = _app.Session.FindElementByAccessibilityId("ResetButton");
            _timerStartButton = _app.Session.FindElementByAccessibilityId("StartButton");
        }
        [Fact]
        public void Test1_InitialValues()
        {
            //arrange
            
            //act

            //assert
            Assert.Equal("00:00:00", _timerTextBox.Text);
            Assert.Equal("Start", _timerStartButton.Text);
            Assert.Equal("Reset", _timerResetButton.Text);
        }

        [Fact]
        public void Test2_TimerWorks()
        {
            //arrange
            string buttonText = "";
            string okText = "";
            string startText = "";
            //act
            _timerTextBox.SendKeys(Keys.Control + "a" + Keys.Control);
            _timerTextBox.SendKeys(Keys.Delete);
            _timerTextBox.SendKeys("01");
            
            _timerStartButton.Click();
            Thread.Sleep(100);
            buttonText = _timerStartButton.Text;
            Thread.Sleep(1100);
            okText = _timerStartButton.Text;
            _timerStartButton.Click();
            Thread.Sleep(10);
            startText = _timerStartButton.Text;

            //assert
            Assert.Equal("Stop", buttonText);
            Assert.Equal("OK", okText);
            Assert.Equal("Start", startText);
        }
    }
}
