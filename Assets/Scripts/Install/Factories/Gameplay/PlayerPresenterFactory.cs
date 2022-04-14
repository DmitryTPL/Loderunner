using System;
using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Install
{
    public class PlayerPresenterFactory : FactoryBase<PlayerPresenter>
    {
        private readonly PlayerStateContext _playerStateContext;
        private readonly IAsyncEnumerableReceiver _receiver;
        private readonly ICharacterFallObserver _characterFallObserver;
        private readonly IWallBlockRemover _wallBlockRemoveObserver;
        private readonly GameConfig _gameConfig;

        public PlayerPresenterFactory(PlayerStateContext playerStateContext, IAsyncEnumerableReceiver receiver,
            Func<ICharacterFallObserver> fallingOpportunityObserverFactory, Func<IWallBlockRemover> wallBlockRemoverFactory, GameConfig gameConfig)
        {
            _playerStateContext = playerStateContext;
            _receiver = receiver;
            _characterFallObserver = fallingOpportunityObserverFactory();
            _wallBlockRemoveObserver = wallBlockRemoverFactory();
            _gameConfig = gameConfig;
        }

        protected override PlayerPresenter CreateEntryWithDependencies()
        {
            return new PlayerPresenter(_playerStateContext, _receiver, _characterFallObserver, _wallBlockRemoveObserver, _gameConfig);
        }
    }
}