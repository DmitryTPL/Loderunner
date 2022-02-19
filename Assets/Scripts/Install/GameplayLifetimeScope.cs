using Loderunner.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [Header("Player")] [SerializeField] private PlayerView _playerViewPrefab;
        [SerializeField] private GameConfig _gameConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfig.PlayerConfig);

            RegisterPresenters(builder);
            RegisterPrefabs(builder);
            RegisterSceneComponents(builder);
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.Register<PlayerPresenter>(Lifetime.Scoped);
            builder.Register<PrepareScenePresenter>(Lifetime.Scoped);
        }

        private void RegisterPrefabs(IContainerBuilder builder)
        {
            builder.RegisterComponent(_playerViewPrefab);
        }

        private void RegisterSceneComponents(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PrepareSceneView>();
        }
    }
}