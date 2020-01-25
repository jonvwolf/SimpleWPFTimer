
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
        
    }
}
