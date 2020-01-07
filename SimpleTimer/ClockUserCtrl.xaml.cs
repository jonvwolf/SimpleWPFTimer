using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl
    {
        public enum Mode
        {
            Timer, Stopwatch
        }

        readonly IClock _clock;
        readonly Mode _mode;
        public ClockUserCtrl(Mode mode)
        {
            InitializeComponent();

            _mode = mode;
            //TODO:  : INotifyPropertyChanged
        }

        public void NumberKeyDown(KeyEventArgs e)
        {
            if (!TxtTime.IsFocused)
            {
                _clock.Pause();
                TxtTime.Text = "";
                TxtTime.Focus();
            }
        }
        public void EnterKeyDown(KeyboardEventArgs e)
        {
            _clock.PrimaryButton(TxtTime.Text);
        }

        public void BackspaceKeyDown(KeyboardEventArgs e)
        {
            _clock.SecondaryButton();
        }

        private void TextPressEnter(object sender, ExecutedRoutedEventArgs e)
        {
            _clock.NewStart(TxtTime.Text);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            _clock.PrimaryButton(TxtTime.Text);
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _clock.SecondaryButton();
        }
    }
}
