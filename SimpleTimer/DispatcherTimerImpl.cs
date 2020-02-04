using System;
using System.Windows.Threading;

namespace SimpleTimer
{
    public class DispatcherTimerImpl : IDispatcherTimer, IClosable
    {
        readonly DispatcherTimer _timer;
        public TimeSpan Interval { get => _timer.Interval; set => _timer.Interval = value; }

        public bool IsEnabled => _timer.IsEnabled;

        public event EventHandler Tick;

        private void RegisterEvents()
        {
            _timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var handler = Tick;
            handler?.Invoke(sender, e);
        }

        private void UnregisterEvents()
        {
            _timer.Tick -= OnTimerTick;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Close()
        {
            UnregisterEvents();
        }

        public DispatcherTimerImpl(DispatcherTimer timer)
        {
            _timer = timer;
            RegisterEvents();
        }

    }
}
