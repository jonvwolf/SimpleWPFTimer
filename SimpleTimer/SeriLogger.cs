using Serilog;
using System;

namespace SimpleTimer
{
    public class SeriLogger : ILogger
    {
        Serilog.ILogger _logger;
        public SeriLogger()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File("errorlog.txt", rollingInterval: RollingInterval.Month, retainedFileCountLimit: 3, fileSizeLimitBytes: 1048576)
                .CreateLogger();
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
