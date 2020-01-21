using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockViewModel : IDisposable
    {
        ICommand TextPressEnter { get; }
        ICommand TextPressEscape { get; }

        void ClockTickHappened(object sender, UiUpdatedEventArgs e);
        void ClockFinished(object sender, UiUpdatedEventArgs e);
        void ClockUiUpdated(object sender, UiUpdatedEventArgs e);
    }
}
