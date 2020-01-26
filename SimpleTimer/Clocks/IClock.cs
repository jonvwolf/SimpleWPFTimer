using System;

namespace SimpleTimer.Clocks
{
    public interface IClock : IDisposable
    {
        void PressPrimaryButton(string textTime = null);
        void PressSecondaryButton();

        void NewStart(string textTime);
        void Pause();
        void Resume();

        event EventHandler<UiUpdatedEventArgs> TickHappened;
        event EventHandler<UiUpdatedEventArgs> Finished;
        event EventHandler<UiUpdatedEventArgs> UiUpdated;
    }
}
