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
    public partial class MainWindow : Window
    {
        readonly ClockUserCtrl _timer;
        readonly ClockUserCtrl _stopwatch;
        
        public MainWindow()
        {
            InitializeComponent();

            _timer = new ClockUserCtrl(ClockUserCtrl.Mode.Timer);
            _stopwatch = new ClockUserCtrl(ClockUserCtrl.Mode.Stopwatch);

            TimerContentCtrl.Content = _timer;
            StopwatchContentCtrl.Content = _stopwatch;

            RoutedCommand tabRightHotKeyCommand = new RoutedCommand();
            tabRightHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabRightHotKeyCommand, TabRightHotKey));

            RoutedCommand tabLeftHotKeyCommand = new RoutedCommand();
            tabLeftHotKeyCommand.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(tabLeftHotKeyCommand, TabLeftHotKey));
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
                if (tabCtrl.SelectedIndex == (tabCtrl.Items.Count - 1))
                    tabCtrl.SelectedIndex = 0;
                else
                    tabCtrl.SelectedIndex++;
            }
            else
            {
                if (tabCtrl.SelectedIndex == 0)
                    tabCtrl.SelectedIndex = tabCtrl.Items.Count - 1;
                else
                    tabCtrl.SelectedIndex--;
            }
            
        }

    }
}
