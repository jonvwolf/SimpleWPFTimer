using SimpleTimer.Clocks;
using System;
using System.Globalization;
using System.IO;
using System.Media;

namespace SimpleTimer
{
    public class LoopSoundPlayer : IDisposable
    {
        readonly ConfigurationValues _config;
        readonly object _lock = new object();
        readonly SoundPlayer _sound;
        readonly IClock _timer;

        public LoopSoundPlayer(Stream sound, ConfigurationValues config)
        {
            _config = config;
            //sound player's dispose, also disposes the stream
            _sound = new SoundPlayer(sound);
            _sound.Load();

            _timer = new TimerClock(config);
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
                _timer.NewStart(TimeSpan.FromSeconds(seconds).ToString(_config.TimeFormatNoSymbols, CultureInfo.InvariantCulture));
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer.Finished -= Timer_Finished;
                    Stop();

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
