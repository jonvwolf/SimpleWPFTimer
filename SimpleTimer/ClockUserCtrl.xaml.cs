using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
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

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IDisposable
    {
        readonly IClock _clock;
        SoundPlayer _sound;

        public ClockUserCtrl()
        {
            InitializeComponent();
            _clock = new TimerClock();
            //TODO:  : INotifyPropertyChanged

            _clock.Finished += Clock_Finished;
            _clock.TickHappened += Clock_TickHappened;


            var stream = Utils.GetResourceStream("tim-kahn__timer.wav");
            //sound player's dispose, also disposes the stream
            _sound = new SoundPlayer(stream);
            _sound.Load();
        }

        private void Clock_TickHappened(object sender, TickHappenedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TxtTime.Text = e.Left.ToString("c", null);
            });
        }

        

        private void Clock_Finished(object sender, FinishedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TxtTime.Text = e.Left.ToString("c", null);

                _sound.PlayLooping();
            });
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
        public void WindowEnterKeyDown(KeyboardEventArgs e)
        {
            _clock.PrimaryButton(TxtTime.Text);
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            _clock.SecondaryButton();
        }

        private void TextPressEnter(object sender, ExecutedRoutedEventArgs e)
        {
            //not needed
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            _clock.PrimaryButton(TxtTime.Text);
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _clock.SecondaryButton();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _clock?.Dispose();
                    _sound?.Dispose();
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
