using System;

namespace SimpleTimer.Clocks
{
    public class StopwatchClock : IClock
    {
        public event EventHandler<UiUpdatedEventArgs> TickHappened;
        public event EventHandler<UiUpdatedEventArgs> Finished;
        public event EventHandler<UiUpdatedEventArgs> UiUpdated;

        public void NewStart(string textTime)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void PressPrimaryButton(string textTime)
        {
            throw new NotImplementedException();
        }

        public void PressSecondaryButton()
        {
            throw new NotImplementedException();
        }

        public void Resume()
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
                    
                }

                disposedValue = true;
            }
        }

        ~StopwatchClock()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
