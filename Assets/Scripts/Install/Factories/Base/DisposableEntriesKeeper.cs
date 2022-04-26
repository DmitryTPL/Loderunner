using System;
using System.Collections.Generic;

namespace Loderunner.Install
{
    public abstract class DisposableEntriesKeeper<T>: IDisposable
        where T: IDisposable
    {
        protected readonly List<T> _createdEntries = new();
        
        public void Dispose()
        {
            foreach (var entry in _createdEntries)
            {
                entry.Dispose();
            }
            
            _createdEntries.Clear();
        }
    }
}