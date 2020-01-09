using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public class FinishedEventArgs : EventArgs
    {
        public TimeSpan Left { get; private set; }

        public FinishedEventArgs(TimeSpan left)
        {
            Left = left;
        }
    }
}
