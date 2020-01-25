using SimpleTimer.ClockUserControls;
using System;
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
    }
}
