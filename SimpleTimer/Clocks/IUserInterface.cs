using SimpleTimer.ClockUserControls;
using System;
using System.Windows;

namespace SimpleTimer.Clocks
{
    public interface IUserInterface
    {
        void BtnStartFocus();
        void TextFocus();
        bool IsTextFocused();
        void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
        void ChangeWindowTitle(string title);

        event EventHandler<UIEventArgs> UiEventHappened;
    }
}
