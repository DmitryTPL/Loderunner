using System;

namespace Loderunner.Install
{
    public abstract class FactoryBase<TEntity> : DisposableEntriesKeeper<TEntity>, IFactory<TEntity>
        where TEntity : IDisposable
    {
        public TEntity Create()
        {
            var entry = CreateEntryWithDependencies();

            _createdEntries.Add(entry);

            return entry;
        }

        protected abstract TEntity CreateEntryWithDependencies();
    }
}