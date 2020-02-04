using Moq;
using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Xunit;

namespace SimpleTimer.IntegrationTests
{
    public class HappyPath
    {
        Mock<IDispatcherTimer> CreateDispatcherTimer(IConfigurationValues config)
        {
            Mock<IDispatcherTimer> timer = new Mock<IDispatcherTimer>();
            bool isEnabled = false;
            timer.SetupGet(x => x.Interval).Returns(TimeSpan.FromSeconds(config.TimerInterval));
            timer.SetupGet(x => x.IsEnabled).Returns(() => { return isEnabled; });
            timer.Setup(x => x.Start()).Callback(() => { isEnabled = true; });
            timer.Setup(x => x.Stop()).Callback(() => { isEnabled = false; });

            return timer;
        }

        [Fact]
        public void Test_StopwatchViewModel_HappyPath()
        {
            //arrange
            string initialText = "";
            bool wasStopped = false;
            string afterTick = "";
            string afterStopped = "";
            string afterReset = "";
            Mock<IUserInterface> ui = new Mock<IUserInterface>();
            Mock<ILogger> logger = new Mock<ILogger>();
            
            var config = new ConfigurationValues();
            var timer = CreateDispatcherTimer(config);

            StopwatchClock clock = new StopwatchClock(logger.Object, config, timer.Object);
            
            StopwatchViewModel stopwatchvm = new StopwatchViewModel(ui.Object, clock, config, logger.Object);

            //act
            
            MethodInfo dynMethod = stopwatchvm.GetType().GetMethod("Ui_UiEventHappened", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(stopwatchvm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked) });
            initialText = stopwatchvm.Text;

            timer.Raise(x => x.Tick += null, this, new EventArgs() { });

            afterTick = stopwatchvm.Text;

            dynMethod.Invoke(stopwatchvm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked) });
            
            wasStopped = !timer.Object.IsEnabled;
            afterStopped = stopwatchvm.Text;

            dynMethod.Invoke(stopwatchvm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnResetClicked) });
            afterReset = stopwatchvm.Text;

            stopwatchvm.Dispose();
            //assert
            Assert.Equal("00:00:00", initialText);
            Assert.Equal("00:00:01", afterTick);
            Assert.True(wasStopped);
            Assert.Equal("00:00:01", afterStopped);
            Assert.Equal("00:00:00", afterReset);

            logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public void Test_TimerViewModel_HappyPath()
        {
            //arrange
            string startAt = "02";
            string initialText = "";
            string stoppedText = "";
            string afterTick = "";
            string finalText = "";
            string afterReset = "";

            Mock<IUserInterface> ui = new Mock<IUserInterface>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ILoopSoundPlayer> player = new Mock<ILoopSoundPlayer>();

            var config = new ConfigurationValues();

            var timer = CreateDispatcherTimer(config);

            TimerClock clock = new TimerClock(config, logger.Object, timer.Object);

            TimerViewModel timervm = new TimerViewModel(ui.Object, player.Object, clock, config, logger.Object);

            //act
            timervm.Text = startAt;
            MethodInfo dynMethod = timervm.GetType().GetMethod("Ui_UiEventHappened", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(timervm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked) });
            initialText = timervm.Text;

            timer.Raise(x => x.Tick += null, this, new EventArgs() { });
            afterTick = timervm.Text;

            dynMethod.Invoke(timervm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked) });
            timer.Raise(x => x.Tick += null, this, new EventArgs() { });

            stoppedText = timervm.Text;

            dynMethod.Invoke(timervm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked) });
            timer.Raise(x => x.Tick += null, this, new EventArgs() { });

            finalText = timervm.Text;

            dynMethod.Invoke(timervm, new object[] { new object(), new UIEventArgs(UIEventArgs.UIEventType.BtnResetClicked) });
            afterReset = timervm.Text;

            timervm.Dispose();

            //assert
            Assert.Equal("00:00:02", initialText);
            Assert.Equal("00:00:01", afterTick);
            Assert.Equal("00:00:01", stoppedText);
            Assert.Equal("00:00:00", finalText);
            Assert.Equal("00:00:01", afterReset);

            logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            player.Verify(x => x.Play(It.IsAny<int>()), Times.Once);
        }

    }
}
