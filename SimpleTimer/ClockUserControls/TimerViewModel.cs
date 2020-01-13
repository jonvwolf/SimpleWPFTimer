using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using static SimpleTimer.UiUpdatedEventArgs;

namespace SimpleTimer.ClockUserControls
{
    public class TimerViewModel : IClockViewModel
    {
        readonly IClock _clock;
        readonly LoopSoundPlayer _sound;
        readonly ActionCommand _textPressEnterCommand;
        readonly ActionCommand _textPressEscapeCommand;
        readonly Dispatcher _dispatcher;
        public string Text { get; protected set; }
        public string PrimaryButtonText { get; protected set; }
        public ICommand TextPressEnter { get => _textPressEnterCommand; }
        public ICommand TextPressEscape { get => _textPressEscapeCommand; }

        public TimerViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _clock = new TimerClock();
            //TODO:  : INotifyPropertyChanged

            var stream = Utils.GetResourceStream("tim-kahn__timer.wav");
            _sound = new LoopSoundPlayer(stream);

            _textPressEnterCommand = new ActionCommand(TxtTime_EnterKeyDown);
            _textPressEscapeCommand = new ActionCommand(TxtTime_EscapeKeyDown);
            RegisterEvents();
        }
        public void Shutdown()
        {
            _clock.Shutdown();
            _sound.Shutdown();
            UnregisterEvents();
        }

        #region ClockEvents
        private void Clock_TickHappened(object sender, UiUpdatedEventArgs e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }

        private void Clock_Finished(object sender, UiUpdatedEventArgs e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                _sound.Play(60);
                UpdateIU(e, true);
            });
        }
        private void Clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            _dispatcher.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }
        #endregion

        private void UpdateIU(UiUpdatedEventArgs e, bool hasEnded = false)
        {
            if (e == null)
                return;
            if (e.Left.HasValue)
            {
                Text = e.Left.Value.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
            }
            if (e.PrimaryBtn.HasValue)
            {
                switch (e.PrimaryBtn.Value)
                {
                    case PrimaryButtonMode.Running:
                        PrimaryButtonText = "_Stop";
                        break;
                    case PrimaryButtonMode.Stopped:
                        if (hasEnded)
                        {
                            PrimaryButtonText = "_Ok";
                        }
                        else
                        {
                            PrimaryButtonText = "_Start";
                        }
                        break;
                    default:
                        PrimaryButtonText = e.PrimaryBtn.Value.ToString();
                        break;
                }
            }
        }

        #region Inner logic (private methods)
        private void RegisterEvents()
        {
            _clock.Finished += Clock_Finished;
            _clock.TickHappened += Clock_TickHappened;
            _clock.UiUpdated += Clock_UiUpdated;
        }
        private void UnregisterEvents()
        {
            _clock.Finished -= Clock_Finished;
            _clock.TickHappened -= Clock_TickHappened;
            _clock.UiUpdated -= Clock_UiUpdated;
        }

        private bool StopPlayer()
        {
            if (PrimaryButtonText == "_Ok")
            {
                PrimaryButtonText.Text = "_Start";
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
