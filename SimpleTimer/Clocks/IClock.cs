using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTimer
{
    public interface IClock : IDisposable
    {
        void PressPrimaryButton(string textTime);
        void PressSecondaryButton();

        void NewStart(string textTime);
        void Pause();
        void Resume();

        event EventHandler<UiUpdatedEventArgs> TickHappened;
        event EventHandler<UiUpdatedEventArgs> Finished;
        event EventHandler<UiUpdatedEventArgs> UiUpdated;
    }
}
