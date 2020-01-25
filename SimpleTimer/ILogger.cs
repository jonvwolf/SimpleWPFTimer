using System;

namespace SimpleTimer
{
    public interface ILogger
    {
        void LogError(string msg);
        void LogError(string msg, Exception e);
    }
}
