using Serilog;
using System.Windows;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File("errorlog.txt", rollingInterval: RollingInterval.Month, retainedFileCountLimit: 3, fileSizeLimitBytes: 1048576)
                .CreateLogger();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Error($"Unhandled app exception: {e.Exception.ToString()}");
            Log.CloseAndFlush();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.CloseAndFlush();
        }
    }
}
