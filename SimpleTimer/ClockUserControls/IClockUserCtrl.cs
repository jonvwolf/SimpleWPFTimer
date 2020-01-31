using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public interface IClockUserCtrl : IDisposable, INotifyPropertyChanged
    {
        string WindowTitle { get; }
        static string AppVersion { get; }
        void SwitchedToAnotherTab();
        void WindowNumberKeyDown(KeyEventArgs e);
        void WindowBackspaceKeyDown(KeyboardEventArgs e);
        void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e);
    }
}
