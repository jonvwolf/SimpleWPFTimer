using System;

namespace SimpleTimer.Clocks
{
    public class StopwatchClock : IClock
    {
        IClock _timer;
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

        public StopwatchClock(IClock timer)
        {
            _timer = timer;
        }

        public void NewStart(string textTime)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            _timer.Pause();
        }

        public void PressPrimaryButton(string textTime)
        {
            _timer.PressPrimaryButton("");
        }

        public void PressSecondaryButton()
        {
            _timer.PressPrimaryButton("00");
            _timer.Pause();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
