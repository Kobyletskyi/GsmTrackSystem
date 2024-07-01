using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900.DTO
{
    public class IncCallResponse : IIncCallResponse
    {
        public event Action<string> OnFinish;

        public event Action<char, TimeSpan> OnDTMF;

        public event Action OnAudioStop;

        public string Phone { get; set; }

        private object _lock = new object();
        protected internal void FireOnFinish(string message)
        {
            lock (_lock)
            {
                if (OnFinish != null)
                {
                    Task.Factory.StartNew(() => OnFinish.Invoke(message));
                }
            }
        }
        protected internal void FireOnDTMF(char key, TimeSpan duration)
        {
            lock (_lock)
            {
                if (OnDTMF != null)
                {
                    Task.Factory.StartNew(() => OnDTMF(key, duration));
                }
            }
        }
        protected internal void FireOnAudioStop()
        {
            lock (_lock)
            {
                if (OnAudioStop != null)
                {
                    Task.Factory.StartNew(() => OnAudioStop());
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                //Delegate d = OnFinish as Delegate;
                //OnFinish = Delegate.RemoveAll(d, d) as Action<string>;
                OnFinish = null;
                OnAudioStop = null;
                OnDTMF = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~IncCallResult() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
