using System;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.Clocks
{
    public class StopwatchClock : BaseClock, IClock
    {
        #region Events
        public event EventHandler<UiUpdatedEventArgs> TickHappened;
        public event EventHandler<UiUpdatedEventArgs> Finished { add => throw new NotSupportedException(); remove => throw new NotSupportedException(); }
        public event EventHandler<UiUpdatedEventArgs> UiUpdated;

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

        TimeSpan _time = TimeSpan.Zero;

        public StopwatchClock(ILogger logger, IConfigurationValues config, IDispatcherTimer timer) : base(logger, config, timer)
        {
            
        }

        protected override void TimerTick()
        {
            _time += base.Interval;
            OnTickHappened(new UiUpdatedEventArgs() { Time = _time });
        }
        
        public void Pause()
        {
            if (base.IsRunning)
            {
                PressPrimaryButton();
            }
        }

        public void PressPrimaryButton(string textTime = null)
        {
            if (base.IsRunning)
            {
                base.StopClock();
                OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = PrimaryButtonMode.Stopped });
            }
            else
            {
                base.StartClock();
                OnUiUpdated(new UiUpdatedEventArgs() { PrimaryBtn = PrimaryButtonMode.Running, Time = _time });
            }
        }

        public void PressSecondaryButton()
        {
            _time = TimeSpan.Zero;
            if (!base.IsRunning)
            {
                PressPrimaryButton();
            }
            else
            {
                OnUiUpdated(new UiUpdatedEventArgs() { Time = _time });
            }
        }

        #region Not supported
        public void Resume()
        {
            throw new NotSupportedException();
        }
        public void NewStart(string textTime)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
