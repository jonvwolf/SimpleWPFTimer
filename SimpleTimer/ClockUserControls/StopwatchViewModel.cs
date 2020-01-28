using SimpleTimer.Clocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.ClockUserControls
{
    public class StopwatchViewModel : IClockViewModel
    {
        readonly IConfigurationValues _config;
        readonly IClock _clock;
        readonly IUserInterface _ui;
        readonly ILogger _logger;
        string _text;
        string _primaryButtonText;
        bool _isTextEnabled;

        #region Datacontext
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public bool IsTextEnabled { get => _isTextEnabled; set { _isTextEnabled = value; OnPropertyChanged(nameof(IsTextEnabled)); } }
        public string PrimaryButtonText { get => _primaryButtonText; set { _primaryButtonText = value; OnPropertyChanged(nameof(PrimaryButtonText)); } }
        public ICommand TextPressEnter { get => null; }
        public ICommand TextPressEscape { get => null; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public StopwatchViewModel(IUserInterface ui, IClock stopwatchclock, IConfigurationValues config, ILogger logger)
        {
            _config = config;
            _ui = ui;
            _clock = stopwatchclock;
            _logger = logger;

            RegisterEvents();

            _text = _config?.InitialText ?? "";
            _primaryButtonText = _config?.PrimaryButtonStart ?? "";
            IsTextEnabled = false;
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
                Text = e.Time.Value.ToString(_config.TimeFormat, CultureInfo.InvariantCulture);
            }
            if (e.PrimaryBtn.HasValue)
            {
                PrimaryButtonText = e.PrimaryBtn.Value switch
                {
                    PrimaryButtonMode.Running => _config.PrimaryButtonStop,
                    PrimaryButtonMode.Stopped => _config.PrimaryButtonStart,
                    _ => e.PrimaryBtn.Value.ToString(),
                };
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
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
        }

        ~StopwatchViewModel()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
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
