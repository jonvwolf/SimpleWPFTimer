using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTimer
{
    public class HandledException : Exception
    {
        public HandledException(string message) : base(message)
        {
        }

        public HandledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HandledException()
        {
        }
    }
}
