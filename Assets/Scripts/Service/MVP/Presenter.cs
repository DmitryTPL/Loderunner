using System;
using System.Threading;

namespace Loderunner.Service
{
    public abstract class Presenter: IDisposable
    {
        protected CancellationTokenSource _disposeCancellationTokenSource = new();
        
        public Guid Guid { get; }

        protected Presenter()
        {
            Guid = Guid.NewGuid();
        }
        
        public virtual void Dispose()
        {
            _disposeCancellationTokenSource.Cancel();
        }
    }
}