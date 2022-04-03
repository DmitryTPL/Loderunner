using System.Collections.Generic;
using Loderunner.Gameplay;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [Header("Player")] [SerializeField] private PlayerView _playerViewPrefab;
        [Header("Levels")] [SerializeField] private List<LevelView> _levelPrefabs = new();
        [SerializeField] private ConfigsHolder _configsHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterScriptableObjects(builder);
            RegisterTypes(builder);
            RegisterPresenters(builder);
            RegisterPrefabs(builder);
            RegisterSceneComponents(builder);
            RegisterFactories(builder);
        }

        private void RegisterScriptableObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_configsHolder.PlayerConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.GameConfig).AsSelf();
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<AsyncEnumerableMessageBus>(Lifetime.Singleton).As<IAsyncEnumerablePublisher, IAsyncEnumerableReceiver>();
            builder.Register<PlayerStateContext>(Lifetime.Scoped).AsSelf();
            builder.Register<PlayerStateData>(Lifetime.Scoped).AsSelf();
            builder.Register<FallingOpportunityObserver>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.Register<PrepareScenePresenter>(Lifetime.Scoped);
            builder.Register<LevelPresenter>(Lifetime.Scoped);
            builder.Register<PlayerPresenter>(Lifetime.Scoped);
        }

        private void RegisterPrefabs(IContainerBuilder builder)
        {
            builder.RegisterComponent(_playerViewPrefab);
        }

        private void RegisterSceneComponents(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PrepareSceneView>();
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterFactory<Transform, PlayerView>(container =>
                {
                    return parentTransform => container.Instantiate(_playerViewPrefab, parentTransform).GetComponent<PlayerView>();
                },
                Lifetime.Scoped);
            
            builder.RegisterFactory<Transform, int, LevelView>(container =>
                {
                    return (parentTransform, levelNumber) => container.Instantiate(_levelPrefabs[levelNumber - 1], parentTransform).GetComponent<LevelView>();
                },
                Lifetime.Scoped);
        }
    }
}