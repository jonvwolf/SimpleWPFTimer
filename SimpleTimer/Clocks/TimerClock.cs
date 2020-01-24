using SimpleTimer.Clocks;
using System;
using System.Globalization;
using System.Timers;
using static SimpleTimer.UiUpdatedEventArgs;

namespace SimpleTimer
{
    public class TimerClock : IClock
    {
        readonly ConfigurationValues _config;
        readonly TimeSpan TimerInterval;

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
                if (_timer.Enabled)
                {
                    //log bug
                    //this shouldnt happen but still modify for graceful degradation   
                }
                _left = value;
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

        public TimerClock(ConfigurationValues config)
        {
            _config = config;
            TimerInterval = TimeSpan.FromSeconds(_config?.TimerInterval ?? 1);
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

        #region Timer related
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Inside timer thread
            try
            {
                bool finished = false;
                UiUpdatedEventArgs args = null;
                lock (_lock)
                {
                    if (!_timer.Enabled)
                        return;

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

                if (finished)
                {
                    OnFinished(args);
                }
                else
                {
                    OnTickHappened(args);
                }
            }
            catch (Exception)
            {
                //todo log
            }
        }
        private void Stop()
        {
            bool lockTaken = false;
            try
            {
                System.Threading.Monitor.TryEnter(_lock, TimerInterval * 2, ref lockTaken);
                if (!lockTaken)
                {
                    //log bug
                    //this shouldn't happen
                }
                //graceful degradation...
                _timer.Stop();
            }
            finally
            {
                if (lockTaken)
                {
                    System.Threading.Monitor.Exit(_lock);
                }
            }
        }
        #endregion

        #region Clock interface
        public void NewStart(string textTime)
        {
            //this always forces a new start
            if (_timer.Enabled)
            {
                Stop();
            }
            Left = textTime.GetTimeSpan(_config.TimeFormat, _config.TimeFormatNoSymbols, _config.DetectSymbolInFormat, _config.FillCharInTimeFormat);
            _originalLeft = Left;
            _timer.Start();

            _primaryBtnMode = PrimaryButtonMode.Running;
            OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode });
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
            if (Left == TimeSpan.Zero)
                return;
            NewStart(Left.ToString(_config.TimeFormat, CultureInfo.InvariantCulture));
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
            NewStart(_originalLeft.ToString(_config.TimeFormat, CultureInfo.InvariantCulture));
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
                    UnregisterEvents();
                    Stop();
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


    }
}
