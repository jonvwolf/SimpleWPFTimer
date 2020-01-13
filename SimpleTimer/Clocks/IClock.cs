using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTimer
{
    public interface IClock : IDisposable
    {
        bool IsRunning { get; }
        void PressPrimaryButton(string textTime);
        void PressSecondaryButton();

        void NewStart(string textTime);
        void Pause();
        void Resume();
        void Shutdown();

        event EventHandler<UiUpdatedEventArgs> TickHappened;
        event EventHandler<UiUpdatedEventArgs> Finished;
        event EventHandler<UiUpdatedEventArgs> UiUpdated;
    }
}
