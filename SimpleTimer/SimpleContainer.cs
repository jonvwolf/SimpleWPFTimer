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
            var clockForPlayer = new TimerClock(_config, _logger, new DispatcherTimer(DispatcherPriority.Normal, dispatcher));

            var stream = Utils.GetResourceStream(_config.RingtoneFilename);
            var player = new LoopSoundPlayer(stream, _config, clockForPlayer);

            var clock = new TimerClock(_config, _logger, new DispatcherTimer(DispatcherPriority.Normal, dispatcher));

            var ctrl = new ClockUserCtrl();
            var vm = new TimerViewModel(ctrl, player, clock, _config, _logger);
            ctrl.SetViewModel(vm);
            
            return ctrl;
        }

        public IClockUserCtrl GetStopwatchClockUserControl(Dispatcher dispatcher)
        {
            var clockForPlayer = new StopwatchClock(_logger, _config, new DispatcherTimer(DispatcherPriority.Normal, dispatcher));

            var ctrl = new ClockUserCtrl();
            var vm = new StopwatchViewModel(ctrl, clockForPlayer, _config, _logger);
            ctrl.SetViewModel(vm);

            return ctrl;
        }

    }
}
