using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public interface ILoopSoundPlayer : IDisposable
    {
        void Play(int seconds);
        void Stop();
    }
}
