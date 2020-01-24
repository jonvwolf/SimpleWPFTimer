using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IClockUserCtrl, IUserInterface
    {
        public event EventHandler<UIEventArgs> UiEventHappened;
        readonly IClockViewModel _vm;
        public ClockUserCtrl(ConfigurationValues config)
        {
            InitializeComponent();
            _vm = new TimerViewModel(this, config);
            DataContext = _vm;
        }

        private void OnUiEventHappened(UIEventArgs e)
        {
            var handler = UiEventHappened;
            handler?.Invoke(this, e);
        }

        #region IUserInterface
        public DispatcherOperation InvokeAsync(Action action)
        {
            return Dispatcher.InvokeAsync(action);
        }
        public void BtnStartFocus()
        {
            BtnStart.Focus();
        }
        public void TextFocus()
        {
            TxtTime.Focus();
        }
        public bool IsTextFocused()
        {
            return TxtTime.IsFocused;
        }
        public void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        #endregion

        #region WindowEvents
        public void WindowNumberKeyDown(KeyEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowNumberKeyDown));
        }
        public void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowShiftEnterKeyDown));
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowBackspaceKeyDown));
        }

        public void SwitchedToAnotherTab()
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.TabLostFocus));
        }
        #endregion

        #region UI events
        private void TxtTime_GotFocus(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.TextGotFocus));
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked));
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.BtnResetClicked));
        }
        
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _vm?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~ClockUserCtrl()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        
    }
}
