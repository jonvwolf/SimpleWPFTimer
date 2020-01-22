using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockViewModel : IDisposable
    {
        string Text { get; set; }
        ICommand TextPressEnter { get; }
        ICommand TextPressEscape { get; }
        void Shutdown();
        void TabLostFocus();
        void WindowBackspaceKeyDown(KeyboardEventArgs e);
        void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e);
        void WindowNumberKeyDown(KeyEventArgs e);
    }
}
