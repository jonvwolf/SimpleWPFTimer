using SimpleTimer.ClockUserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        readonly IClockUserCtrl _timer;
        readonly IClockUserCtrl _stopwatch;
        
        public MainWindow()
        {
            InitializeComponent();

            var config = new ConfigurationValues();
            _timer = new ClockUserCtrl(config);
            _stopwatch = new ClockUserCtrl(config);

            TimerContentCtrl.Content = _timer;
            StopwatchContentCtrl.Content = _stopwatch;
            
            RoutedCommand tabRightHotKeyCommand = new RoutedCommand();
            tabRightHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabRightHotKeyCommand, TabRightHotKey));

            RoutedCommand tabLeftHotKeyCommand = new RoutedCommand();
            tabLeftHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabLeftHotKeyCommand, TabLeftHotKey));

            RoutedCommand enterHotKeyCommand = new RoutedCommand();
            enterHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Shift));
            CommandBindings.Add(new CommandBinding(enterHotKeyCommand, ShiftEnterHotKey));

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.KeyDown += MainWindow_KeyDown;
        }
        private void UnregisterEvents()
        {
            this.KeyDown -= MainWindow_KeyDown;
        }

        private void SwitchTabs(bool right)
        {
            GetCurrentUserCtrl()?.SwitchedToAnotherTab();
            if (right)
            {
                if (TabCtrl.SelectedIndex == (TabCtrl.Items.Count - 1))
                    TabCtrl.SelectedIndex = 0;
                else
                    TabCtrl.SelectedIndex++;
            }
            else
            {
                if (TabCtrl.SelectedIndex == 0)
                    TabCtrl.SelectedIndex = TabCtrl.Items.Count - 1;
                else
                    TabCtrl.SelectedIndex--;
            }
        }

        private ClockUserCtrl GetCurrentUserCtrl()
        {
            var obj = TabCtrl.SelectedItem as System.Windows.Controls.TabItem;
            string tag = (string)obj.Tag ?? "";

            var ctrl = obj.FindName(tag) as ContentControl;
            var clockctrl = ctrl?.Content as ClockUserCtrl;
            return clockctrl;
        }

        #region Keyboards events
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            //numbers and keypad numbers (0-9)
            if ((key >= 34 && key <= 43) || (key >= 74 && key <= 83))
            {
                GetCurrentUserCtrl()?.WindowNumberKeyDown(e);
            }
            else if (e.Key == Key.Back)
            {
                GetCurrentUserCtrl()?.WindowBackspaceKeyDown(e);
            }
        }

        private void ShiftEnterHotKey(object sender, ExecutedRoutedEventArgs e)
        {
            GetCurrentUserCtrl()?.WindowShiftEnterKeyDown(e);
        }
        private void TabRightHotKey(object sender, ExecutedRoutedEventArgs e)
        {
            SwitchTabs(true);
        }

        private void TabLeftHotKey(object sender, ExecutedRoutedEventArgs e)
        {
            SwitchTabs(false);
        }
        #endregion
        
        #region Events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnregisterEvents();
            CommandBindings.Clear();
            Dispose();
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
                    _timer?.Dispose();
                    _stopwatch?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
