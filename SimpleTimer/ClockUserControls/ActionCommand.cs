using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SimpleTimer
{
    public class ActionCommand : ICommand, IDisposable
    {
        public event EventHandler CanExecuteChanged { add { throw new NotSupportedException(); } remove { } }

        Action<object> _action;
        public ActionCommand(Action<object> action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var action = _action;
            action?.Invoke(parameter);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _action = null;
                }
                disposedValue = true;
            }
        }

        ~ActionCommand()
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
