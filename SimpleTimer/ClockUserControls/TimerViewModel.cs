using SimpleTimer.Clocks;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.ClockUserControls
{
    public class TimerViewModel : IClockViewModel
    {
        readonly ConfigurationValues _config;
        readonly IClock _clock;
        readonly LoopSoundPlayer _sound;
        readonly ActionCommand _textPressEnterCommand;
        readonly ActionCommand _textPressEscapeCommand;
        readonly IUserInterface _ui;
        string _text;
        string _primaryButtonText;

        #region DataContext
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
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

        public TimerViewModel(IUserInterface ui, ConfigurationValues config)
        {
            _config = config;
            _ui = ui;
            _clock = new TimerClock(config);
            
            var stream = Utils.GetResourceStream(_config.RingtoneFilename);
            _sound = new LoopSoundPlayer(stream, config);

            _textPressEnterCommand = new ActionCommand(TxtTime_EnterKeyDown);
            _textPressEscapeCommand = new ActionCommand(TxtTime_EscapeKeyDown);
            RegisterEvents();

            _text = _config?.InitialText ?? "";
            _primaryButtonText = _config?.PrimaryButtonStart ?? "";
        }

        private void RegisterEvents()
        {
            _clock.Finished += Clock_Finished;
            _clock.TickHappened += Clock_TickHappened;
            _clock.UiUpdated += Clock_UiUpdated;

            _ui.UiEventHappened += Ui_UiEventHappened;
        }

        private void UnregisterEvents()
        {
            _clock.Finished -= Clock_Finished;
            _clock.TickHappened -= Clock_TickHappened;
            _clock.UiUpdated -= Clock_UiUpdated;

            _ui.UiEventHappened -= Ui_UiEventHappened;
        }

        #region ClockEvents
        private void Clock_TickHappened(object sender, UiUpdatedEventArgs e)
        {
            _ui.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }

        private void Clock_Finished(object sender, UiUpdatedEventArgs e)
        {
            _ui.InvokeAsync(() =>
            {
                _sound.Play(_config.TimerBeepingSeconds);
                UpdateIU(e, true);
            });
        }
        private void Clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            _ui.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }
        #endregion

        #region UI events
        private void Ui_UiEventHappened(object sender, UIEventArgs e)
        {
            switch (e.Type)
            {
                case UIEventArgs.UIEventType.BtnResetClicked:
                    StopPlayer();
                    _clock.PressSecondaryButton();
                    break;
                case UIEventArgs.UIEventType.BtnStartClicked:
                    PressPrimaryButton();
                    break;
                case UIEventArgs.UIEventType.TabLostFocus:
                case UIEventArgs.UIEventType.TextGotFocus:
                    StopPlayer();
                    _clock.Pause();
                    break;
                case UIEventArgs.UIEventType.WindowBackspaceKeyDown:
                    StopPlayer();
                    _clock.PressSecondaryButton();
                    break;
                case UIEventArgs.UIEventType.WindowNumberKeyDown:
                    //`if` so it doesn't erase text everytime user presses number keys down
                    if (!_ui.IsTextFocused())
                    {
                        StopPlayer();
                        _clock.Pause();
                        Text = "";
                        _ui.TextFocus();
                    }
                    break;
                case UIEventArgs.UIEventType.WindowShiftEnterKeyDown:
                    //This will toggle primary (between start/stop)
                    //But if txttime is focused, and enter is pressed,
                    //it should explicitely start a new timer (that's why the `if`)
                    if (!_ui.IsTextFocused())
                    {
                        PressPrimaryButton();
                    }
                    break;
                default:
                    //TODO log
                    throw new InvalidOperationException($"Unkown event type: {e.Type.ToString()}");
            }
        }

        private void TxtTime_EnterKeyDown(object parameters)
        {
            NewStart();
            _ui.BtnStartFocus();
        }
        private void TxtTime_EscapeKeyDown(object parameters)
        {
            //this will make txttime loses focus
            _ui.BtnStartFocus();
            //when user clicks text (focus), it pauses so he/she can edit
            //when escape is pressed, resume clock. (It cannot be start a new because text may have changed)
            //(because when enter is pressed it will start a new one but escape just resumes)
            _clock.Resume();
        }
        #endregion

        #region Inner logic (private methods)
        
        private void UpdateIU(UiUpdatedEventArgs e, bool hasEnded = false)
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
                        if (hasEnded)
                        {
                            PrimaryButtonText = _config.PrimaryButtonOK;
                        }
                        else
                        {
                            PrimaryButtonText = _config.PrimaryButtonStart;
                        }
                        break;
                    default:
                        PrimaryButtonText = e.PrimaryBtn.Value.ToString();
                        break;
                }
            }
        }

        private bool StopPlayer()
        {
            if (PrimaryButtonText == _config.PrimaryButtonOK)
            {
                PrimaryButtonText = _config.PrimaryButtonStart;
                _sound.Stop();
                return true;
            }
            return false;
        }
        private void PressPrimaryButton()
        {
            if (StopPlayer())
                return;
            try
            {
                _clock.PressPrimaryButton(Text);
            }
            catch (Exception ex)
            {
                _ui.ShowMessageBox(ex.Message, _config.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void NewStart()
        {
            StopPlayer();
            try
            {
                _clock.NewStart(Text);
            }
            catch (Exception ex)
            {
                _ui.ShowMessageBox(ex.Message, _config.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _sound?.Dispose();
                    _textPressEnterCommand?.Dispose();
                    _textPressEscapeCommand?.Dispose();
                }

                disposedValue = true;
            }
        }

        ~TimerViewModel()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
