using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class CharacterFallObserverFactory : FactoryBase<CharacterFallObserver>
    {
        private readonly IAsyncEnumerableReceiver _receiver;

        public CharacterFallObserverFactory(IAsyncEnumerableReceiver receiver)
        {
            _receiver = receiver;
        }
        
        protected override CharacterFallObserver CreateEntryWithDependencies()
        {
            return new CharacterFallObserver(_receiver);
        }
    }
}