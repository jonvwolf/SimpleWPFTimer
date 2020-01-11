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
        private readonly object _lock = new object();
        PrimaryButtonMode _primaryBtnMode;

        #region Left var
        /// <summary>
        /// Variable to know how long is left to finish the timer
        /// This variable is accessed by UI and timer thread
        /// </summary>
        TimeSpan _left = TimeSpan.Zero;
        /// <summary>
        /// Used for reset
        /// </summary>
        TimeSpan _originalLeft = TimeSpan.Zero;
        /// <summary>
        /// A simple check to avoid race conditions.
        /// Fallback if timer is running. Stops, modifies value then starts again
        /// </summary>
        TimeSpan Left
        {
            get
            {
                return _left;
            }
            set
            {
                //check always if timer is running
                if (_timer != null && _timer.Enabled == false)
                {
                    _left = value;
                }
                else
                {
                    //log bug

                    //this shouldnt happen but still modify for graceful degradation
                    _timer.Stop();
                    _left = value;
                    _timer.Start();
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<UiUpdatedEventArgs> TickHappened;
        public event EventHandler<UiUpdatedEventArgs> Finished;
        public event EventHandler<UiUpdatedEventArgs> UiUpdated;

        private void OnFinished(UiUpdatedEventArgs e)
        {
            var handler = Finished;
            handler?.Invoke(this, e);
        }

        private void OnTickHappened(UiUpdatedEventArgs e)
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
            //Inside timer thread

            //This is not 100% accurate but it is good enough for this app
            //For better precision, you should do a delta
            lock (_lock)
            {
                try
                {
                    Left -= TimerInterval;

                    if (Left <= TimeSpan.Zero)
                    {
                        _timer.Stop();
                        Left = TimeSpan.Zero;
                        _primaryBtnMode = PrimaryButtonMode.Stopped;
                        OnFinished(new UiUpdatedEventArgs { Left = Left, PrimaryBtn = _primaryBtnMode });
                    }
                    else
                    {
                        OnTickHappened(new UiUpdatedEventArgs() { Left = Left });
                    }
                }
                catch (Exception ex)
                {
                    //todo log exception
                }
            }
        }

        /// <summary>
        /// Called outside Timer thread (Timer_Elapsed)
        /// </summary>
        private void SafeStop()
        {
            lock (_lock)
            {
                _timer.Stop();
            }
        }

        public void NewStart(string textTime)
        {
            //this always forces a new start
            SafeStop();
            Left = GetTimeFromText(textTime);
            _originalLeft = Left;
            _timer.Start();

            _primaryBtnMode = PrimaryButtonMode.Running;
            OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode });
        }

        public void Pause()
        {
            if (_primaryBtnMode == PrimaryButtonMode.Running)
            {
                SafeStop();

                _primaryBtnMode = PrimaryButtonMode.Stopped;
                OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode });
            }
        }

        public void Resume()
        {
            NewStart(Left.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
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
                    if(_timer != null)
                    {
                        _timer.Elapsed -= Timer_Elapsed;
                        //SafeStop();
                        _timer.Stop();
                        _timer.Dispose();
                    }
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

        private static TimeSpan GetTimeFromText(string text)
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
