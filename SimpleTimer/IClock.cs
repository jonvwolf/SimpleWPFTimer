using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimpleTimer
{
    public interface IClock : INotifyPropertyChanged
    {
        string Text { get; set; }
        void PrimaryButton();
        void SecondaryButton();
        void FocusText();
    }
}
