using System;

namespace SimpleTimer.Clocks
{
    public interface IClock : IClosable
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
