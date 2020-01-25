using SimpleTimer.ClockUserControls;

namespace SimpleTimer
{
    public interface ISimpleContainer
    {
        IClockUserCtrl GetTimerClockUserControl();
        IClockUserCtrl GetStopwatchClockUserControl();
    }
}
