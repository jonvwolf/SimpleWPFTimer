using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public class StopwatchClock : IClock
    {
        public event EventHandler<TickHappenedEventArgs> TickHappened;
        public event EventHandler<FinishedEventArgs> Finished;

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

        public void PrimaryButton(string textTime)
        {
            throw new NotImplementedException();
        }

        public void SecondaryButton()
        {
            throw new NotImplementedException();
        }
    }
}
