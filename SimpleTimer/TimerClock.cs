using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace SimpleTimer
{
    public class TimerClock : IClock
    {
        static readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(1);
        string Text { get; set; }
        Timer _timer = new Timer();

        TimeSpan Left { get; set; }

        #region Events
        public event EventHandler<TickHappenedEventArgs> TickHappened;
        public event EventHandler<object> Finished;

        private void OnFinished(EventArgs e)
        {
            var handler = Finished;
            handler?.Invoke(this, e);
        }

        private void OnTickHappened(TickHappenedEventArgs e)
        {
            var handler = TickHappened;
            handler?.Invoke(this, e);
        }
        #endregion Events

        public TimerClock()
        {
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = TimerInterval.TotalMilliseconds;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //not in UI thread

            //This is not 100% accurate but it is good enough for this app
            //For better precision, you should do a delta
            Left -= TimerInterval;

            if(Left <= TimeSpan.Zero)
            {
                _timer.Stop();
                OnFinished(EventArgs.Empty);
            }
            else
            {
                OnTickHappened(new TickHappenedEventArgs(Left));
            }
            
        }

        private TimeSpan? _getTimeFromText(string text)
        {
            try
            {
                TimeSpan span = TimeSpan.ParseExact(text, "c", null);
                return span;
            }
            catch (Exception e) when (e is FormatException || e is ArgumentNullException)
            {
                //TODO: log e
            }

            return null;
        }

        public void NewStart(string textTime)
        {
            _timer.Stop();
            TimeSpan? ts = _getTimeFromText(textTime);
            if (ts.HasValue)
            {
                Left = ts.Value;
                _timer.Start();
            }
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void PrimaryButton(string textTime)
        {
            throw new NotImplementedException();
        }

        public void SecondaryButton()
        {
            throw new NotImplementedException();
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
    }
}
