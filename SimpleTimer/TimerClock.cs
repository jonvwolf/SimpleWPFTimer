using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Timers;

namespace SimpleTimer
{
    public class TimerClock : IClock
    {
        public enum PrimaryButtonMode
        {
            Stopped, Running
        }

        static readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(1);

        readonly Timer _timer = new Timer();
        TimeSpan _left = TimeSpan.Zero;
        TimeSpan _originalLeft = TimeSpan.Zero;

        PrimaryButtonMode _primaryBtnMode;
        
        #region Events
        public event EventHandler<TickHappenedEventArgs> TickHappened;
        public event EventHandler<FinishedEventArgs> Finished;
        public event EventHandler<UiUpdatedEventArgs> UiUpdated;

        private void OnFinished(FinishedEventArgs e)
        {
            var handler = Finished;
            handler?.Invoke(this, e);
        }

        private void OnTickHappened(TickHappenedEventArgs e)
        {
            var handler = TickHappened;
            handler?.Invoke(this, e);
        }
        private void OnUiUpdated(UiUpdatedEventArgs e)
        {
            var handler = UiUpdated;
            handler?.Invoke(this, e);
        }
        #endregion Events

        public TimerClock()
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = TimerInterval.TotalMilliseconds;

            _primaryBtnMode = PrimaryButtonMode.Stopped;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //not in UI thread

            //This is not 100% accurate but it is good enough for this app
            //For better precision, you should do a delta
            _left -= TimerInterval;

            if(_left <= TimeSpan.Zero)
            {
                _left = TimeSpan.Zero;
                _timer.Stop();
                //todo: change these events args (they also update primary button mode)
                OnFinished(new FinishedEventArgs(_left));
            }
            else
            {
                OnTickHappened(new TickHappenedEventArgs(_left));
            }
            
        }

        public void NewStart(string textTime)
        {
            //this always forces a new start
            _timer.Stop();
            _left = GetTimeFromText(textTime);
            _originalLeft = _left;
            _timer.Start();

            ChangePrimaryBtnMode(PrimaryButtonMode.Running);
        }

        public void Pause()
        {
            _timer.Stop();
            ChangePrimaryBtnMode(PrimaryButtonMode.Stopped);
        }

        public void Resume()
        {
            NewStart(_left.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        public void PrimaryButton(string textTime)
        {
            if (_primaryBtnMode == PrimaryButtonMode.Stopped)
            {
                NewStart(textTime);
            }
            else if (_primaryBtnMode == PrimaryButtonMode.Running)
            {
                Pause();
            }
        }

        public void SecondaryButton()
        {
            NewStart(_originalLeft.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~TimerClock()
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

        private void ChangePrimaryBtnMode(PrimaryButtonMode mode)
        {
            _primaryBtnMode = mode;
            OnUiUpdated(new UiUpdatedEventArgs()
            {
                PrimaryBtn = _primaryBtnMode
            });
        }

        private TimeSpan GetTimeFromText(string text)
        {
            try
            {
                text = text?.Trim() ?? "";
                
                string format = @"hh\:mm\:ss";
                if (text.Contains(":", StringComparison.InvariantCulture) == false)
                {
                    if (text.Length < 6)
                    {
                        text = text.PadLeft(6, '0');
                    }
                    format = "hhmmss";
                }

                TimeSpan span = TimeSpan.ParseExact(text, format, CultureInfo.InvariantCulture);
                return span;
            }
            catch (Exception e)
            {
                throw new HandledException("Error while parsing time. Time must be of format: hhmmss. Example: 0507 is translated to 5 minutes and 7 seconds", e);
            }
        }

    }
}
