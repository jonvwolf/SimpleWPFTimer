using SimpleTimer.Clocks;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.ClockUserControls
{
    public class StopwatchViewModel : BaseViewModel
    {
        readonly IClock _clock;
        readonly IUserInterface _ui;
        readonly ILogger _logger;
        
        public StopwatchViewModel(IUserInterface ui, IClock stopwatchclock, IConfigurationValues config, ILogger logger) : base(config)
        {
            _ui = ui;
            _clock = stopwatchclock;
            _logger = logger;

            RegisterEvents();

            Text = Config?.InitialText ?? "";
            PrimaryButtonText = Config?.PrimaryButtonStart ?? "";
            IsTextEnabled = false;

            ChangeButtonBlue();
            _ui?.ChangeWindowTitle(Config.WindowTitle);
        }

        private void RegisterEvents()
        {
            _clock.TickHappened += Clock_TickHappened;
            _clock.UiUpdated += Clock_UiUpdated;

            _ui.UiEventHappened += Ui_UiEventHappened;
        }

        private void UnregisterEvents()
        {
            _clock.TickHappened -= Clock_TickHappened;
            _clock.UiUpdated -= Clock_UiUpdated;

            _ui.UiEventHappened -= Ui_UiEventHappened;
        }

        #region clock events
        private void Clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            //runs on UI thread
            UpdateIU(e);
        }

        private void Clock_TickHappened(object sender, UiUpdatedEventArgs e)
        {
            //runs on UI thread
            UpdateIU(e);
        }
        #endregion

        #region ui events
        private void Ui_UiEventHappened(object sender, UIEventArgs e)
        {
            switch (e.Type)
            {
                case UIEventArgs.UIEventType.TextGotFocus:
                case UIEventArgs.UIEventType.WindowNumberKeyDown:
                    //do nothing
                    break;
                case UIEventArgs.UIEventType.BtnStartClicked:
                    _clock.PressPrimaryButton();
                    break;
                case UIEventArgs.UIEventType.TabLostFocus:
                    _clock.Pause();
                    break;
                case UIEventArgs.UIEventType.BtnResetClicked:
                case UIEventArgs.UIEventType.WindowBackspaceKeyDown:
                    _clock.PressSecondaryButton();
                    break;
                case UIEventArgs.UIEventType.WindowShiftEnterKeyDown:
                    _clock.PressPrimaryButton();
                    break;
                default:
                    _logger.LogError($"{nameof(TimerViewModel)} Unkown event type: {e.Type.ToString()}");
                    break;
            }
        }

        #endregion

        #region private (inner log)
        private void UpdateIU(UiUpdatedEventArgs e)
        {
            if (e == null)
                return;
            if (e.Time.HasValue)
            {
                Text = e.Time.Value.ToString(Config.TimeFormat, CultureInfo.InvariantCulture);
            }
            if (e.PrimaryBtn.HasValue)
            {
                switch (e.PrimaryBtn.Value)
                {
                    case PrimaryButtonMode.Running:
                        PrimaryButtonText = Config.PrimaryButtonStop;
                        ChangeButtonRed();
                        _ui.ChangeWindowTitle(Config.WindowTitleRunning);
                        break;
                    case PrimaryButtonMode.Stopped:
                        PrimaryButtonText = Config.PrimaryButtonStart;
                        ChangeButtonBlue();
                        _ui.ChangeWindowTitle(Config.WindowTitle);
                        break;
                    default:
                        PrimaryButtonText = e.PrimaryBtn.Value.ToString();
                        break;
                }
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnregisterEvents();

                    _clock?.Close();
                }

                disposedValue = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
