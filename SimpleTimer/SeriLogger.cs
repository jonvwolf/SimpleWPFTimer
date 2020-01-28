using Serilog;
using System;

namespace SimpleTimer
{
    public class SeriLogger : ILogger
    {
        readonly Serilog.ILogger _logger;
        public SeriLogger()
        {
            _logger = Log.Logger;
        }
        public void LogError(string msg)
        {
            _logger.Error(msg);
        }

        public void LogError(string msg, Exception e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            _logger.Error(msg + Environment.NewLine + e.ToString());
        }
    }
}
