
namespace SimpleTimer
{
    public class ConfigurationValues : IConfigurationValues
    {
        public string TimeFormatNoSymbols { get => "hhmmss"; }
        public string DetectSymbolInFormat { get => ":"; }
        public string TimeFormat { get => @"hh\:mm\:ss"; }
        public char FillCharInTimeFormat { get => '0'; }
        public string RingtoneFilename { get => "tim-kahn__timer.wav"; }
        public string ErrorTitle { get => "Error"; }
        public string InitialText { get => "00:00:00"; }
        public string PrimaryButtonStart { get => "Start"; }
        public string PrimaryButtonStop { get => "Stop"; }
        public string PrimaryButtonOK { get => "OK"; }
        public int TimerBeepingSeconds { get => 60; }
        public int TimerInterval { get => 1; }

        public string NoticeFilename => "NOTICE.txt";

        public string AuthorCreditsTitle => "Author & Credits";

        public string KeybindingsTitle => "Keybindings";

        public string Author => "github.com/jonwolfdev";

        public string BlueColor => "#4d90fe";
        public string FontColorOnBlue => "White";
        public string BlueMouseOverColor => "#2f5bb7";
        public string BluePressedColor => "#4d90fe";

        public string RedColor => "#d45d79";
        public string FontColorOnRed => "White";
        public string RedMouseOverColor => "#ea9085";
        public string RedPressedColor => "#d45d79";
    }
}
