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
        readonly ActionCommand _textPressEnterCommand;
        readonly ActionCommand _textPressEscapeCommand;
        readonly IUserInterface _ui;
        readonly ILogger _logger;
        string _text;
        string _primaryButtonText;
        bool _isTextEnabled;

        #region Datacontext
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public bool IsTextEnabled { get => _isTextEnabled; set { _isTextEnabled = value; OnPropertyChanged(nameof(IsTextEnabled)); } }
        public string PrimaryButtonText { get => _primaryButtonText; set { _primaryButtonText = value; OnPropertyChanged(nameof(PrimaryButtonText)); } }
        public ICommand TextPressEnter { get => _textPressEnterCommand; }
        public ICommand TextPressEscape { get => _textPressEscapeCommand; }

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

            //todo: see if these are needed to be instantiated
            _textPressEnterCommand = new ActionCommand((e) => { });
            _textPressEscapeCommand = new ActionCommand((e) => { });
            RegisterEvents();

            _text = _config?.InitialText ?? "";
            _primaryButtonText = _config?.PrimaryButtonStart ?? "";
            IsTextEnabled = false;
        }

        private void RegisterEvents()
        {
            _clock.TickHappened += _clock_TickHappened;
            _clock.UiUpdated += _clock_UiUpdated;

            _ui.UiEventHappened += _ui_UiEventHappened;
        }

        private void UnregisterEvents()
        {
            _clock.TickHappened -= _clock_TickHappened;
            _clock.UiUpdated -= _clock_UiUpdated;

            _ui.UiEventHappened -= _ui_UiEventHappened;
        }

        #region clock events
        private void _clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            _ui.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }

        private void _clock_TickHappened(object sender, UiUpdatedEventArgs e)
        {
            _ui.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }
        #endregion

        #region ui events
        private void _ui_UiEventHappened(object sender, UIEventArgs e)
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
            if (e.Left.HasValue)
            {
                Text = e.Left.Value.ToString(_config.TimeFormat, CultureInfo.InvariantCulture);
            }
            if (e.PrimaryBtn.HasValue)
            {
                switch (e.PrimaryBtn.Value)
                {
                    case PrimaryButtonMode.Running:
                        PrimaryButtonText = _config.PrimaryButtonStop;
                        break;
                    case PrimaryButtonMode.Stopped:
                        PrimaryButtonText = _config.PrimaryButtonStart;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnregisterEvents();

                    _clock?.Dispose();
                    _textPressEnterCommand?.Dispose();
                    _textPressEscapeCommand?.Dispose();
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
