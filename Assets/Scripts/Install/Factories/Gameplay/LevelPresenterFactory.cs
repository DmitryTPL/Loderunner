using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class LevelPresenterFactory : FactoryBase<LevelPresenter>
    {
        private readonly LevelData _levelData;
        private readonly IAsyncEnumerablePublisher _publisher;

        public LevelPresenterFactory(LevelData levelData, IAsyncEnumerablePublisher publisher)
        {
            _levelData = levelData;
            _publisher = publisher;
        }
        
        protected override LevelPresenter CreateEntryWithDependencies()
        {
            return new LevelPresenter(_levelData, _publisher);
        }
    }
}