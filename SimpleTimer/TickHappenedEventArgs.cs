using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public class TickHappenedEventArgs : EventArgs
    {
        public TimeSpan Left { get; private set; }

        public TickHappenedEventArgs(TimeSpan left)
        {
            Left = left;
        }
    }
}
