using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        readonly ClockUserCtrl _timer;
        readonly ClockUserCtrl _stopwatch;
        
        public MainWindow()
        {
            InitializeComponent();

            _timer = new ClockUserCtrl();
            _stopwatch = new ClockUserCtrl();

            TimerContentCtrl.Content = _timer;
            StopwatchContentCtrl.Content = _stopwatch;
            
            RoutedCommand tabRightHotKeyCommand = new RoutedCommand();
            tabRightHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabRightHotKeyCommand, TabRightHotKey));

            RoutedCommand tabLeftHotKeyCommand = new RoutedCommand();
            tabLeftHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabLeftHotKeyCommand, TabLeftHotKey));

            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            //numbers and keypad numbers (0-9)
            if ((key >= 34 && key <= 43) || (key >= 74 && key <= 83))
            {
                GetCurrentUserCtrl()?.WindowNumberKeyDown(e);
            }
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                GetCurrentUserCtrl()?.WindowEnterKeyDown(e);
            }
            else if (e.Key == Key.Back)
            {
                GetCurrentUserCtrl()?.WindowBackspaceKeyDown(e);
            }
        }

        private void TabRightHotKey(object sender, ExecutedRoutedEventArgs e)
        {
            SwitchTabs(true);
        }

        private void TabLeftHotKey(object sender, ExecutedRoutedEventArgs e)
        {
            SwitchTabs(false);
        }

        private void SwitchTabs(bool right)
        {
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stopwatch?.Dispose();
                    _timer?.Dispose();
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
