using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace SimpleTimer.Clocks
{
    public abstract class BaseClock : IClosable
    {
        readonly DispatcherTimer _timer;
        readonly ILogger _logger;
        protected bool IsRunning { get { return _timer.IsEnabled; } }
        protected TimeSpan Interval { get { return _timer.Interval; } }
        public BaseClock(ILogger logger, IConfigurationValues config, DispatcherTimer timer)
        {
            _timer = timer ?? throw new ArgumentNullException(nameof(timer));
            _logger = logger;
            _timer.Interval = TimeSpan.FromSeconds(config?.TimerInterval ?? 1);
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _timer.Tick += Timer_Tick;
        }
        private void UnregisterEvents()
        {
            _timer.Tick -= Timer_Tick;
        }

        protected abstract void TimerTick();
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                TimerTick();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(BaseClock)} : {nameof(Timer_Tick)} Error.", ex);
            }
        }

        protected void StartClock()
        {
            _timer.Start();
        }
        protected void StopClock()
        {
            _timer.Stop();
        }

        public virtual void Close()
        {
            UnregisterEvents();
            _timer.Stop();
        }
    }
}
