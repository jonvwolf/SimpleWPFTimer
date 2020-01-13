using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using static SimpleTimer.UiUpdatedEventArgs;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IDisposable
    {
        readonly IClock _clock;
        readonly LoopSoundPlayer _sound;

        readonly ActionCommand _textPressEnterCommand;
        readonly ActionCommand _textPressEscapeCommand;

        public ICommand TextPressEnter { get => _textPressEnterCommand; }
        public ICommand TextPressEscape { get => _textPressEscapeCommand; }
        public ClockUserCtrl()
        {
            InitializeComponent();
            DataContext = this;

            _clock = new TimerClock();
            //TODO:  : INotifyPropertyChanged

            var stream = Utils.GetResourceStream("tim-kahn__timer.wav");
            _sound = new LoopSoundPlayer(stream);

            _textPressEnterCommand = new ActionCommand(TxtTime_EnterKeyDown);
            _textPressEscapeCommand = new ActionCommand(TxtTime_EscapeKeyDown);
            RegisterEvents();
        }

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
        public void Shutdown()
        {
            _clock.Shutdown();

            UnregisterEvents();
            _sound.Stop();
        }

        private void UpdateIU(UiUpdatedEventArgs e)
        {
            if (e == null)
                return;
            if (e.Left.HasValue)
            {
                TxtTime.Text = e.Left.Value.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
            }
            if (e.PrimaryBtn.HasValue)
            {
                switch (e.PrimaryBtn.Value)
                {
                    case PrimaryButtonMode.Running:
                        ((AccessText)BtnStart.Content).Text = "_Stop";
                        break;
                    case PrimaryButtonMode.Stopped:
                        ((AccessText)BtnStart.Content).Text = "_Start";
                        break;
                    default:
                        ((AccessText)BtnStart.Content).Text = e.PrimaryBtn.Value.ToString();
                        break;
                }
            }
        }

        private void PressPrimaryButton()
        {
            try
            {
                _clock.PressPrimaryButton(TxtTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void NewStart()
        {
            try
            {
                _clock.NewStart(TxtTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region ClockEvents
        private void Clock_TickHappened(object sender, UiUpdatedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }

        private void Clock_Finished(object sender, UiUpdatedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                UpdateIU(e);
                _sound.Play(60);
            });
        }
        private void Clock_UiUpdated(object sender, UiUpdatedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                UpdateIU(e);
            });
        }
        #endregion

        #region WindowEvents
        public void WindowNumberKeyDown(KeyEventArgs e)
        {
            //`if` so it doesn't erase text everytime user presses number keys down
            if (!TxtTime.IsFocused)
            {
                _clock.Pause();
                TxtTime.Text = "";
                TxtTime.Focus();
            }
        }
        public void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e)
        {
            //This will toggle primary (between start/stop)
            //But if txttime is focused, and enter is pressed,
            //it should explicitely start a new timer (that's why the `if`)
            if (!TxtTime.IsFocused)
            {
                PressPrimaryButton();
            }
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            _clock.PressSecondaryButton();
        }

        public void SwitchedToAnotherTab()
        {
            _clock.Pause();
        }
        #endregion

        #region UI events
        private void TxtTime_EnterKeyDown(object parameters)
        {
            NewStart();
            BtnStart.Focus();
        }
        private void TxtTime_EscapeKeyDown(object parameters)
        {
            //this will make txttime loses focus
            BtnStart.Focus();
            //when user clicks text (focus), it pauses so he/she can edit
            //when escape is pressed, resume clock. (It cannot be start a new because text may have changed)
            //(because when enter is pressed it will start a new one but escape just resumes)
            _clock.Resume();
        }
        private void TxtTime_GotFocus(object sender, RoutedEventArgs e)
        {
            _clock.Pause();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            PressPrimaryButton();
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _clock.PressSecondaryButton();
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
                    _clock?.Dispose();
                    _sound?.Dispose();
                    _textPressEnterCommand?.Dispose();
                    _textPressEscapeCommand?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~ClockUserCtrl()
        {
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
