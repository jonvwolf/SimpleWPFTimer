using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTimer
{
    public class StopwatchClock : IClock
    {
        public event EventHandler<UiUpdatedEventArgs> TickHappened;
        public event EventHandler<UiUpdatedEventArgs> Finished;
        public event EventHandler<UiUpdatedEventArgs> UiUpdated;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

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

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
