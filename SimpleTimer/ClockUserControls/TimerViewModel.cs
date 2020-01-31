using SimpleTimer.Clocks;
using System;
using System.Globalization;
using System.Windows;
using static SimpleTimer.Clocks.UiUpdatedEventArgs;

namespace SimpleTimer.ClockUserControls
{
    public class TimerViewModel : BaseViewModel
    {
        readonly IClock _clock;
        readonly ILoopSoundPlayer _sound;
        
        readonly IUserInterface _ui;
        readonly ILogger _logger;
        
        public TimerViewModel(IUserInterface ui, ILoopSoundPlayer player, IClock timerclock, IConfigurationValues config, ILogger logger) : base(config)
        {
            _logger = logger;
            _ui = ui;
            _clock = timerclock;
            
            _sound = player;

            TextPressEnterCommand = new ActionCommand(TxtTime_EnterKeyDown);
            TextPressEscapeCommand = new ActionCommand(TxtTime_EscapeKeyDown);
            RegisterEvents();

            Text = Config?.InitialText ?? "";
            PrimaryButtonText = Config?.PrimaryButtonStart ?? "";
            IsTextEnabled = true;
            ChangeButtonBlue();
            _ui?.ChangeWindowTitle(Config.WindowTitle);
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
            //runs on UI thread
            UpdateIU(e);
        }

        private void Clock_Finished(object sender, UiUpdatedEventArgs e)
        {
            //runs on UI thread
            _sound.Play(Config.TimerBeepingSeconds);
            UpdateIU(e, true);
        }
        private void Clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            //runs on UI thread
            UpdateIU(e);
        }
        #endregion

        #region UI events
        private void Ui_UiEventHappened(object sender, UIEventArgs e)
        {
            switch (e.Type)
            {
                case UIEventArgs.UIEventType.BtnStartClicked:
                    PressPrimaryButton();
                    break;
                case UIEventArgs.UIEventType.TabLostFocus:
                case UIEventArgs.UIEventType.TextGotFocus:
                    StopPlayer();
                    _clock.Pause();
                    break;
                case UIEventArgs.UIEventType.BtnResetClicked:
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
                    _logger.LogError($"{nameof(TimerViewModel)} Unkown event type: {e.Type.ToString()}");
                    break;
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
                        if (hasEnded)
                        {
                            PrimaryButtonText = Config.PrimaryButtonOK;
                        }
                        else
                        {
                            PrimaryButtonText = Config.PrimaryButtonStart;
                        }
                        ChangeButtonBlue();
                        _ui.ChangeWindowTitle(Config.WindowTitle);
                        break;
                    default:
                        PrimaryButtonText = e.PrimaryBtn.Value.ToString();
                        break;
                }
            }
        }

        private bool StopPlayer()
        {
            if (PrimaryButtonText == Config.PrimaryButtonOK)
            {
                PrimaryButtonText = Config.PrimaryButtonStart;
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
                _ui.ShowMessageBox(ex.Message, Config.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
                _ui.ShowMessageBox(ex.Message, Config.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _sound?.Dispose();
                }

                disposedValue = true;
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
