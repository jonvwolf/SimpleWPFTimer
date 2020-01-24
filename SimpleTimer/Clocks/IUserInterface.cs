using SimpleTimer.ClockUserControls;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SimpleTimer.Clocks
{
    public interface IUserInterface
    {
        void BtnStartFocus();
        void TextFocus();
        bool IsTextFocused();
        void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
        DispatcherOperation InvokeAsync(Action action);

        event EventHandler<UIEventArgs> UiEventHappened;
    }
}
