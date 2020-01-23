using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockViewModel : IDisposable, INotifyPropertyChanged
    {
        string Text { get; set; }
        string PrimaryButtonText { get; set; }
        ICommand TextPressEnter { get; }
        ICommand TextPressEscape { get; }
    }
}