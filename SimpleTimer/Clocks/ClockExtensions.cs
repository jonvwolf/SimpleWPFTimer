using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SimpleTimer.Clocks
{
    public static class ClockExtensions
    {
        public static TimeSpan GetTimeSpan(this string text)
        {
            text = text?.Trim() ?? "";

            string format = @"hh\:mm\:ss";
            int fill = 6;
            char fillNumber = '0';
            if (text.Contains(":", StringComparison.InvariantCulture) == false)
            {
                if (text.Length < fill)
                {
                    text = text.PadLeft(fill, fillNumber);
                }
                format = "hhmmss";
            }

            TimeSpan span = TimeSpan.ParseExact(text, format, CultureInfo.InvariantCulture);
            return span;
        }
    }
}
