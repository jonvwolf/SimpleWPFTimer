using SimpleTimer.ClockUserControls;
using System.Windows.Threading;

namespace SimpleTimer
{
    public interface ISimpleContainer
    {
        IClockUserCtrl GetTimerClockUserControl(Dispatcher dispatcher);
        IClockUserCtrl GetStopwatchClockUserControl(Dispatcher dispatcher);
    }
}
