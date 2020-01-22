using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using static SimpleTimer.UiUpdatedEventArgs;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IDisposable, IClockUserCtrl, IUserInterface
    {

        IClockViewModel _vm;
        public ClockUserCtrl()
        {
            InitializeComponent();
            _vm = new TimerViewModel(this);
            DataContext = _vm;
        }

        public void Shutdown()
        {
            _vm.Shutdown();
        }

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
        public void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        

        #region WindowEvents
        public void WindowNumberKeyDown(KeyEventArgs e)
        {
            _vm.WindowNumberKeyDown(e);
        }
        public void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e)
        {
            _vm.WindowShiftEnterKeyDown(e);
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            _vm.WindowBackspaceKeyDown(e);
        }

        public void SwitchedToAnotherTab()
        {
            _vm.TabLostFocus();
        }
        #endregion

        #region UI events
        //https://stackoverflow.com/questions/18117294/how-does-this-button-click-work-in-wpf-mvvm
        private void TxtTime_GotFocus(object sender, RoutedEventArgs e)
        {
            StopPlayer();
            _clock.Pause();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            PressPrimaryButton();
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            StopPlayer();
            _clock.PressSecondaryButton();
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
