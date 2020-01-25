using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public interface ISimpleContainer
    {
        IClockUserCtrl GetTimerClockUserControl();
        IClockUserCtrl GetStopwatchClockUserControl();
    }
}
