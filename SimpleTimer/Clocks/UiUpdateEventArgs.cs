using System;
using System.Collections.Generic;
using System.Text;
using static SimpleTimer.TimerClock;

namespace SimpleTimer
{
    public class UiUpdatedEventArgs : EventArgs
    {
        public enum PrimaryButtonMode
        {
            Stopped, Running
        }
        public PrimaryButtonMode? PrimaryBtn { get; set; }
        public TimeSpan? Left { get; set; }
    }
}
