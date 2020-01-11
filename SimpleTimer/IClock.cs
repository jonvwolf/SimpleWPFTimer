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
        void Resume();

        event EventHandler<TickHappenedEventArgs> TickHappened;
        event EventHandler<FinishedEventArgs> Finished;
        event EventHandler<UiUpdatedEventArgs> UiUpdated;
    }
}
