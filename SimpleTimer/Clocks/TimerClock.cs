using System;
using System.Globalization;
using System.Windows.Threading;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.Clocks
{
    public class TimerClock : BaseClock, IClock
    {
        readonly IConfigurationValues _config;
        PrimaryButtonMode _primaryBtnMode;

        #region Left var
        /// <summary>
        /// Used for reset
        /// </summary>
        TimeSpan _originalLeft = TimeSpan.Zero;
        /// <summary>
        /// Variable to know how long is left to finish the timer
        /// This variable is accessed by UI and timer thread
        /// </summary>
        TimeSpan Left { get; set; } = TimeSpan.Zero;

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

        public TimerClock(IConfigurationValues config, ILogger logger, IDispatcherTimer timer) : base(logger, config, timer)
        {
            _config = config;
            _primaryBtnMode = PrimaryButtonMode.Stopped;
        }

        #region Timer related
        protected override void TimerTick()
        {
            //Inside timer
            if (!base.IsRunning)
                return;

            //not exact but good enough for this app
            Left -= base.Interval;

            if (Left <= TimeSpan.Zero)
            {
                base.StopClock();
                Left = TimeSpan.Zero;

                _primaryBtnMode = PrimaryButtonMode.Stopped;
                OnFinished(new UiUpdatedEventArgs { Time = Left, PrimaryBtn = _primaryBtnMode });
            }
            else
            {
                OnTickHappened(new UiUpdatedEventArgs() { Time = Left });
            }
        }
        #endregion

        #region Clock interface
        public void NewStart(string textTime)
        {
            //this always forces a new start
            if (base.IsRunning)
            {
                base.StopClock();
            }
            Left = textTime.GetTimeSpan(_config.TimeFormat, _config.TimeFormatNoSymbols, _config.DetectSymbolInFormat, _config.FillCharInTimeFormat);
            _originalLeft = Left;
            base.StartClock();

            _primaryBtnMode = PrimaryButtonMode.Running;
            OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = _primaryBtnMode, Time = Left });
        }
        public void Pause()
        {
            if (base.IsRunning)
            {
                base.StopClock();

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

        public void PressPrimaryButton(string textTime = null)
        {
            if (textTime == null)
            {
                textTime = "";
            }

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


    }
}
