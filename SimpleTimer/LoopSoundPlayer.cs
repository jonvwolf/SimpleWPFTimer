using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;

namespace SimpleTimer
{
    public class LoopSoundPlayer : IDisposable
    {
        SoundPlayer _sound;

        public LoopSoundPlayer(Stream sound)
        {
            //sound player's dispose, also disposes the stream
            _sound = new SoundPlayer(sound);
            _sound.Load();
        }

        public void Play(int seconds)
        {
            _sound.Play();
        }

        public void Stop()
        {
            _sound.Stop();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
