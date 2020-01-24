using System;

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
