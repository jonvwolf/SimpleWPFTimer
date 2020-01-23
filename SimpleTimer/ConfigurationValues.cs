using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public class ConfigurationValues
    {
        /// <summary>
        /// Exact as format but without symbols. Example: hhmmss
        /// </summary>
        public string TimeFormatNoSymbols { get; set; } = "hhmmss";
        /// <summary>
        /// 
        /// </summary>
        public string DetectSymbolInFormat { get; set; } = ":";
        /// <summary>
        /// Format to be used to parse text into a Timespan
        /// </summary>
        public string TimeFormat { get; set; } = @"hh\:mm\:ss";
        /// <summary>
        /// 
        /// </summary>
        public char FillCharInTimeFormat { get; set; } = '0';
        /// <summary>
        /// Filename of beeping sound
        /// </summary>
        public string RingtoneFilename { get; set; } = "tim-kahn__timer.wav";
        public string ErrorTitle { get; set; } = "Error";
        /// <summary>
        /// Initial text for textbox (must match time format)
        /// </summary>
        public string InitialText { get; set; } = "00:00:00";

        public string PrimaryButtonStart { get; set; } = "Start";
        public string PrimaryButtonStop { get; set; } = "Stop";
        public string PrimaryButtonOK { get; set; } = "OK";
        /// <summary>
        /// How many seconds should beeping should play
        /// </summary>
        public int TimerBeepingSeconds { get; set; } = 60;
        public int TimerInterval { get; set; } = 1;
        
    }
}
