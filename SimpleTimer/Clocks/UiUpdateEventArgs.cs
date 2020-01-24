using System;

namespace SimpleTimer.Clocks
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
