using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockViewModel : IDisposable, INotifyPropertyChanged
    {
        string Text { get; set; }
        string PrimaryButtonText { get; set; }
        bool IsTextEnabled { get; set; }
        ICommand TextPressEnter { get; }
        ICommand TextPressEscape { get; }
    }
}