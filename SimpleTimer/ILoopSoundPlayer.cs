using System;

namespace SimpleTimer
{
    public interface ILoopSoundPlayer : IDisposable
    {
        void Play(int seconds);
        void Stop();
    }
}
