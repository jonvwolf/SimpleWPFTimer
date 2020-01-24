using System;
using System.Windows.Input;

namespace SimpleTimer
{
    public interface IClockUserCtrl : IDisposable
    {
        void SwitchedToAnotherTab();
        void WindowNumberKeyDown(KeyEventArgs e);
        void WindowBackspaceKeyDown(KeyboardEventArgs e);
        void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e);
    }
}
