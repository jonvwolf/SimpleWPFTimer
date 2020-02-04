using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;
using System.Windows.Threading;

namespace SimpleTimer
{
    public class SimpleContainer : ISimpleContainer
    {
        readonly IConfigurationValues _config;
        readonly ILogger _logger;
        
        public SimpleContainer()
        {
            _config = new ConfigurationValues();
            _logger = new SeriLogger();
        }
        public IClockUserCtrl GetTimerClockUserControl(Dispatcher dispatcher)
        {
            var dispatcherForPlayer = new DispatcherTimerImpl(new DispatcherTimer(DispatcherPriority.Normal, dispatcher));
            var clockForPlayer = new TimerClock(_config, _logger, dispatcherForPlayer);

            var stream = Utils.GetResourceStream(_config.RingtoneFilename);
            var player = new LoopSoundPlayer(stream, _config, clockForPlayer);

            var dispatcherTimer = new DispatcherTimerImpl(new DispatcherTimer(DispatcherPriority.Normal, dispatcher));
            var clock = new TimerClock(_config, _logger, dispatcherTimer);

            var ctrl = new ClockUserCtrl();
            var vm = new TimerViewModel(ctrl, player, clock, _config, _logger);
            ctrl.SetViewModel(vm);
            
            return ctrl;
        }

        public IClockUserCtrl GetStopwatchClockUserControl(Dispatcher dispatcher)
        {
            var dispatcherTimer = new DispatcherTimerImpl(new DispatcherTimer(DispatcherPriority.Normal, dispatcher));
            var clockForPlayer = new StopwatchClock(_logger, _config, dispatcherTimer);

            var ctrl = new ClockUserCtrl();
            var vm = new StopwatchViewModel(ctrl, clockForPlayer, _config, _logger);
            ctrl.SetViewModel(vm);

            return ctrl;
        }

        public IConfigurationValues GetConfiguration()
        {
            return _config;
        }
    }
}
