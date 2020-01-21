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
using static SimpleTimer.UiUpdatedEventArgs;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IDisposable, IClockUserCtrl
    {

        IClockViewModel _vm;
        public ClockUserCtrl()
        {
            InitializeComponent();
            _vm = new TimerViewModel(Dispatcher);
            DataContext = _vm;

        }

        public void Shutdown()
        {
            _vm.Shutdown();
        }

        

        

        #region WindowEvents
        public void WindowNumberKeyDown(KeyEventArgs e)
        {
            //`if` so it doesn't erase text everytime user presses number keys down
            if (!TxtTime.IsFocused)
            {
                StopPlayer();
                _clock.Pause();
                TxtTime.Text = "";
                TxtTime.Focus();
            }
        }
        public void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e)
        {
            //This will toggle primary (between start/stop)
            //But if txttime is focused, and enter is pressed,
            //it should explicitely start a new timer (that's why the `if`)
            if (!TxtTime.IsFocused)
            {
                PressPrimaryButton();
            }
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            StopPlayer();
            _clock.PressSecondaryButton();
        }

        public void SwitchedToAnotherTab()
        {
            StopPlayer();
            _clock.Pause();
        }
        #endregion

        #region UI events
        
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
