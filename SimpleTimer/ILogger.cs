using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public interface ILogger
    {
        void LogError(string msg);
        void LogError(string msg, Exception e);
    }
}
