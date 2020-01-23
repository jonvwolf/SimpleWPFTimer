using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockViewModel : IDisposable
    {
        string Text { get; set; }
        ICommand TextPressEnter { get; }
        ICommand TextPressEscape { get; }
    }
}