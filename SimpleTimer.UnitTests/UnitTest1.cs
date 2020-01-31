using SimpleTimer.ClockUserControls;
using System;
using System.Threading;
using Xunit;

namespace SimpleTimer.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var ok = new UIEventArgs(UIEventArgs.UIEventType.BtnResetClicked);
            Assert.Equal(UIEventArgs.UIEventType.BtnResetClicked, ok.Type);
        }

        [Fact]
        public void Test2()
        {
            MainWindow window = null;
            var thread = new Thread(() =>
            {
                window = new MainWindow();
                window.Closed += (s, e) => window.Dispatcher.InvokeShutdown();
                window.Show();
                Thread.Sleep(1000);
                window.Close();
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
