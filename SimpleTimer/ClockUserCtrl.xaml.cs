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
        
        public ClockUserCtrl()
        {
            InitializeComponent();
            _clock = new TimerClock();
            //TODO:  : INotifyPropertyChanged

            _clock.Finished += Clock_Finished;
            _clock.TickHappened += Clock_TickHappened;

        }

        private void Clock_TickHappened(object sender, TickHappenedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TxtTime.Text = e.Left.ToString("c", null);
                SoundPlayer sp = new SoundPlayer(GetResourceStream(string.Format(null, "pack://application:,,,/{0};component//{1}", "SimpleTimer", "resources/tim-kahn__timer.wav")));
                sp.Load();
                sp.PlayLooping();
                sp.Dispose();
            });
        }

        private static Stream GetResourceStream(string resourcePath)
        {
            try
            {

                string s = System.IO.Packaging.PackUriHelper.UriSchemePack;
                var uri = new Uri(resourcePath);
                StreamResourceInfo sri = System.Windows.Application.GetResourceStream(uri);
                return sri.Stream;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void Clock_Finished(object sender, FinishedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TxtTime.Text = e.Left.ToString("c", null);
                
                
                //TODO: playsound
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
