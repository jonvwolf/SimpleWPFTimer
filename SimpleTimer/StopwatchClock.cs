using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public class StopwatchClock : IClock
    {
        public string Text { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void FocusText()
        {
            
        }

        public void PrimaryButton()
        {
            
        }

        public void SecondaryButton()
        {
            
        }
    }
}
