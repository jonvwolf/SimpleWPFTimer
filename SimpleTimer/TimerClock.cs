using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        readonly System.Timers.Timer _timer = new System.Timers.Timer();
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
            _timer.Interval = TimerInterval.TotalMilliseconds;
            _primaryBtnMode = PrimaryButtonMode.Stopped;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _timer.Elapsed += Timer_Elapsed;
        }
        private void UnregisterEvents()
        {
            _timer.Elapsed -= Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Inside timer thread

            bool finished = false;
            UiUpdatedEventArgs args = null;
            lock (_lock)
            {
                if (!_timer.Enabled)
                    return;

                try
                {
                    //This is not 100% accurate but it is good enough for this app
                    //For better precision, you should do a delta
                    Left -= TimerInterval;

                    if (Left <= TimeSpan.Zero)
                    {
                        _timer.Stop();
                        Left = TimeSpan.Zero;

                        _primaryBtnMode = PrimaryButtonMode.Stopped;
                        finished = true;
                        args = new UiUpdatedEventArgs { Left = Left, PrimaryBtn = _primaryBtnMode };
                    }
                    else
                    {
                        finished = false;
                        args = new UiUpdatedEventArgs() { Left = Left };
                    }
                }
                catch (Exception ex)
                {
                    //todo log exception
                }
            }

            if (finished)
            {
                OnFinished(args);
            }
            else
            {
                OnTickHappened(args);
            }
        }

        public void NewStart(string textTime)
        {
            //this always forces a new start
            if (_timer.Enabled)
            {
                Stop();
            }
            Left = GetTimeFromText(textTime);
            _originalLeft = Left;
            _timer.Start();

            _primaryBtnMode = PrimaryButtonMode.Running;
            OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode });
        }

        private void Stop()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_lock, TimerInterval * 2, ref lockTaken);
                if (lockTaken)
                {
                    _timer.Stop();
                }
                else
                {
                    //fallback
                    _timer.Stop();
                    //todo log error
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lock);
                }
            }
        }

        public void Pause()
        {
            if (_timer.Enabled)
            {
                Stop();

                _primaryBtnMode = PrimaryButtonMode.Stopped;
                OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode });
            }
        }

        public void Resume()
        {
            NewStart(Left.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        public void PressPrimaryButton(string textTime)
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

        public void PressSecondaryButton()
        {
            NewStart(_originalLeft.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        public void Shutdown()
        {
            Stop();
            UnregisterEvents();
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
