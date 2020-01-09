using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public interface IClock : IDisposable
    {
        void PrimaryButton(string textTime);
        void SecondaryButton();

        void NewStart(string textTime);
        void Pause();

        event EventHandler<TickHappenedEventArgs> TickHappened;
        event EventHandler<FinishedEventArgs> Finished;
    }
}
