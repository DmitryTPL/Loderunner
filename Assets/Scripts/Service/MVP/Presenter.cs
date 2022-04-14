using System;

namespace Loderunner.Service
{
    public abstract class Presenter: IDisposable
    {
        public Guid Id { get; }

        public Presenter()
        {
            Id = Guid.NewGuid();
        }
        
        public virtual void Dispose()
        {
        }
    }
}