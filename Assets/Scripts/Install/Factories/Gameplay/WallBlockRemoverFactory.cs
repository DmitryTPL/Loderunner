using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class WallBlockRemoverFactory : FactoryBase<WallBlockRemover>
    {
        private readonly IAsyncEnumerableReceiver _receiver;

        public WallBlockRemoverFactory(IAsyncEnumerableReceiver receiver)
        {
            _receiver = receiver;
        }
        
        protected override WallBlockRemover CreateEntryWithDependencies()
        {
            return new WallBlockRemover(_receiver);
        }
    }
}