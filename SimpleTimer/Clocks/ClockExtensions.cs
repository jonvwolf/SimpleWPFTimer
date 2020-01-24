using System;
using System.Globalization;

namespace SimpleTimer.Clocks
{
    public static class ClockExtensions
    {
        public static TimeSpan GetTimeSpan(this string text, string timeFormat, string timeFormatNoSymbols, string detectSymbol, char charFill)
        {
            text = text?.Trim() ?? "";

            string format = timeFormat;
            int fill = 6;
            char fillNumber = charFill;
            if (text.Contains(detectSymbol, StringComparison.InvariantCulture) == false)
            {
                if (text.Length < fill)
                {
                    text = text.PadLeft(fill, fillNumber);
                }
                format = timeFormatNoSymbols;
            }

            TimeSpan span = TimeSpan.ParseExact(text, format, CultureInfo.InvariantCulture);
            return span;
        }
    }
}
