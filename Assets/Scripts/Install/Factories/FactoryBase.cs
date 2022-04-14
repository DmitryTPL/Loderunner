using System;
using System.Collections.Generic;

namespace Loderunner.Install
{
    public abstract class FactoryBase<T> : IDisposable, IFactory<T>
        where T: IDisposable
    {
        private readonly List<IDisposable> _createdEntries = new();

        public virtual void Dispose()
        {
            foreach (var entry in _createdEntries)
            {
                entry.Dispose();
            }
            
            _createdEntries.Clear();
        }

        public T Create()
        {
            var entry = CreateEntryWithDependencies();
            
            _createdEntries.Add(entry);

            return entry;
        }

        protected abstract T CreateEntryWithDependencies();

    }
}