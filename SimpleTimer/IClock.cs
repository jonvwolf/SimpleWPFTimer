using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public interface IClock : IDisposable
    {
        string Text { get; set; }
        void PrimaryButton(string textTime);
        void SecondaryButton();

        void NewStart(string textTime);
        void Pause();

        event EventHandler<TickHappenedEventArgs> TickHappened;
        event EventHandler<object> Finished;
    }
}
