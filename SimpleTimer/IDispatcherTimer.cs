using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public interface IDispatcherTimer
    {
        TimeSpan Interval { get; set; }
        bool IsEnabled { get; }
        event EventHandler Tick;
        void Start();
        void Stop();
    }
}
