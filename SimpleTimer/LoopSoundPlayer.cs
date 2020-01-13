using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Media;
using System.Text;

namespace SimpleTimer
{
    public class LoopSoundPlayer : IDisposable
    {
        readonly object _lock = new object();
        SoundPlayer _sound;
        IClock _timer;

        public LoopSoundPlayer(Stream sound)
        {
            //sound player's dispose, also disposes the stream
            _sound = new SoundPlayer(sound);
            _sound.Load();

            _timer = new TimerClock();
            _timer.Finished += Timer_Finished;
        }

        private void Timer_Finished(object sender, UiUpdatedEventArgs e)
        {
            lock (_lock)
            {
                _sound.Stop();
            }
        }

        public void Play(int seconds)
        {
            lock (_lock)
            {
                _timer.NewStart(TimeSpan.FromSeconds(seconds).ToString("hhmmss", CultureInfo.InvariantCulture));
                _sound.Stop();
                _sound.PlayLooping();
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                _timer.Pause();
                _sound.Stop();
            }
        }

        public void Shutdown()
        {
            lock (_lock)
            {
                _timer.Shutdown();
                _timer.Finished -= Timer_Finished;
                _sound.Stop();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                    _sound?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~LoopSoundPlayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
