using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    public abstract class BaseViewModel : IClockViewModel
    {
        protected IConfigurationValues Config { get; private set; }
        protected ActionCommand TextPressEnterCommand { get; set; }
        protected ActionCommand TextPressEscapeCommand { get; set; }
        
        string _text;
        string _primaryButtonText;
        bool _isTextEnabled;

        string _primaryBtnBackgroundColor;
        string _primaryBtnForegroundColor;
        string _primaryBtnMouseOverColor;
        string _primaryBtnPressedColor;

        public BaseViewModel(IConfigurationValues config)
        {
            Config = config;
        }

        #region DataContext
        public string PrimaryBtnBackgroundColor { get => _primaryBtnBackgroundColor; set { _primaryBtnBackgroundColor = value; OnPropertyChanged(nameof(PrimaryBtnBackgroundColor)); } }
        public string PrimaryBtnForegroundColor { get => _primaryBtnForegroundColor; set { _primaryBtnForegroundColor = value; OnPropertyChanged(nameof(PrimaryBtnForegroundColor)); } }
        public string PrimaryBtnMouseOverColor { get => _primaryBtnMouseOverColor; set { _primaryBtnMouseOverColor = value; OnPropertyChanged(nameof(PrimaryBtnMouseOverColor)); } }
        public string PrimaryBtnPressedColor { get => _primaryBtnPressedColor; set { _primaryBtnPressedColor = value; OnPropertyChanged(nameof(PrimaryBtnPressedColor)); } }
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public bool IsTextEnabled { get => _isTextEnabled; set { _isTextEnabled = value; OnPropertyChanged(nameof(IsTextEnabled)); } }
        public string PrimaryButtonText { get => _primaryButtonText; set { _primaryButtonText = value; OnPropertyChanged(nameof(PrimaryButtonText)); } }
        public ICommand TextPressEnter { get => TextPressEnterCommand; }
        public ICommand TextPressEscape { get => TextPressEscapeCommand; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        protected void ChangeButtonRed()
        {
            PrimaryBtnBackgroundColor = Config.RedColor;
            PrimaryBtnForegroundColor = Config.FontColorOnRed;
            PrimaryBtnMouseOverColor = Config.RedMouseOverColor;
            PrimaryBtnPressedColor = Config.RedPressedColor;
        }
        protected void ChangeButtonBlue()
        {
            PrimaryBtnBackgroundColor = Config.BlueColor;
            PrimaryBtnForegroundColor = Config.FontColorOnBlue;
            PrimaryBtnMouseOverColor = Config.BlueMouseOverColor;
            PrimaryBtnPressedColor = Config.BluePressedColor;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TextPressEnterCommand?.Dispose();
                    TextPressEscapeCommand?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~BaseViewModel()
        {
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
