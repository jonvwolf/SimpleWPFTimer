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

        event EventHandler<UiUpdatedEventArgs> TickHappened;
        event EventHandler<UiUpdatedEventArgs> Finished;
        event EventHandler<UiUpdatedEventArgs> UiUpdated;
    }
}
