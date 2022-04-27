using System;
using System.Threading;

namespace Loderunner.Service
{
    public abstract class Presenter: IDisposable
    {
        private readonly CancellationTokenSource _disposeCancellationTokenSource = new();

        protected CancellationToken DisposeCancellationToken => _disposeCancellationTokenSource.Token;
        
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