using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer.ClockUserControls
{
    public class UIEventArgs : EventArgs
    {
        public enum UIEventType
        {
            TabLostFocus, WindowBackspaceKeyDown, WindowShiftEnterKeyDown, WindowNumberKeyDown,
            TextGotFocus, BtnStartClicked, BtnResetClicked
        }

        public UIEventType Type { get; set; }

        public UIEventArgs(UIEventType type)
        {
            Type = type;
        }
    }
}
