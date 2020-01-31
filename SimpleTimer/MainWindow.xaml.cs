using SimpleTimer.ClockUserControls;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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
        readonly IConfigurationValues _config;
        public static string AppVersion
        {
            get
            {
                var version = typeof(MainWindow).Assembly.GetName().Version;
                return $"v{version.Major}.{version.Minor}";
            }
        }
        
        public MainWindow() : this(null)
        {
            
        }
        public MainWindow(ISimpleContainer container)
        {
            InitializeComponent();
            if (container == null)
            {
                container = new SimpleContainer();
            }
            DataContext = this;
            _config = container.GetConfiguration();
            _timer = container.GetTimerClockUserControl(Dispatcher);
            _stopwatch = container.GetStopwatchClockUserControl(Dispatcher);

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

        private void TabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetCurrentUserCtrl()?.SwitchedToAnotherTab();
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

        private void KeybindingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string keybindings = "- Alt+left or right arrow key: Switch between tabs";
            keybindings += Environment.NewLine + "- Shift+Enter: Primary button";
            keybindings += Environment.NewLine + "- Backspace: Secondary button";
            keybindings += Environment.NewLine + "- Key numbers/pad [0-9]: Automatically pauses Timer and selects Text box";
            MessageBox.Show(keybindings, _config.KeybindingsTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AuthorCreditsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string notice = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + _config.NoticeFilename;
            string author = "Author: " + _config.Author;
            author += Environment.NewLine + "App license: see License.txt file";
            author += Environment.NewLine + Environment.NewLine + File.ReadAllText(notice);
            MessageBox.Show(author, _config.AuthorCreditsTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
