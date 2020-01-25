using SimpleTimer.Clocks;
using SimpleTimer.ClockUserControls;

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
        public ILogger GetLogger()
        {
            return _logger;
        }
        public IClockUserCtrl GetTimerClockUserControl()
        {
            var clockForPlayer = new TimerClock(_config, _logger);

            var stream = Utils.GetResourceStream(_config.RingtoneFilename);
            var player = new LoopSoundPlayer(stream, _config, clockForPlayer);

            var clock = new TimerClock(_config, _logger);

            var ctrl = new ClockUserCtrl();
            var vm = new TimerViewModel(ctrl, player, clock, _config, _logger);
            ctrl.SetViewModel(vm);
            
            return ctrl;
        }

        public IClockUserCtrl GetStopwatchClockUserControl()
        {
            var clockForPlayer = new TimerClock(_config, _logger);

            var stream = Utils.GetResourceStream(_config.RingtoneFilename);
            var player = new LoopSoundPlayer(stream, _config, clockForPlayer);

            var clock = new TimerClock(_config, _logger);

            var ctrl = new ClockUserCtrl();
            var vm = new TimerViewModel(ctrl, player, clock, _config, _logger);
            ctrl.SetViewModel(vm);

            return ctrl;
        }

    }
}
