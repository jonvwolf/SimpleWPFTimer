using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SimpleTimer
{
    public interface IClockUserCtrl : IDisposable
    {
        void SwitchedToAnotherTab();
        void WindowNumberKeyDown(KeyEventArgs e);
        void WindowBackspaceKeyDown(KeyboardEventArgs e);
        void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e);
        void Shutdown();
    }
}
