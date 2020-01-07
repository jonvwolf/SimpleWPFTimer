using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public interface IClock
    {
        string Text { get; set; }
        void PrimaryButton(string textTime);
        void SecondaryButton();

        void NewStart(string textTime);
        void Pause();

        event EventHandler<object> TickHappened;
        event EventHandler<object> Finished;
    }
}
