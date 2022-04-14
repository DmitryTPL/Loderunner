using System;
using System.Collections.Generic;
using Loderunner.Gameplay;
using Loderunner.Service;
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
            RegisterPrefabs(builder);
            RegisterSceneComponents(builder);
            RegisterFactories(builder);
        }

        private void RegisterScriptableObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_configsHolder.PlayerConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.GameConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.WallBlockRemoveConfig).AsSelf();
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<ITime, DefaultTimeHandler>(Lifetime.Singleton);
            builder.Register<AsyncEnumerableMessageBus>(Lifetime.Singleton).As<IAsyncEnumerablePublisher, IAsyncEnumerableReceiver>();
            builder.Register<PlayerStateData>(Lifetime.Scoped).AsSelf();
            builder.Register<PlayerStateContext>(Lifetime.Scoped).AsSelf();

            builder.Register<PrepareScenePresenterFactory>(Lifetime.Singleton);
            builder.Register<LevelPresenterFactory>(Lifetime.Singleton);
            builder.Register<PlayerPresenterFactory>(Lifetime.Singleton);
            builder.Register<CharacterFallObserverFactory>(Lifetime.Singleton);
            builder.Register<WallBlockRemoverFactory>(Lifetime.Singleton);
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
            builder.RegisterFactory<Transform, PlayerView>(
                container => { return parentTransform => container.Instantiate(_playerViewPrefab, parentTransform).GetComponent<PlayerView>(); },
                Lifetime.Scoped);

            builder.RegisterFactory<Transform, int, LevelView>(
                container => { return (parentTransform, levelNumber) => container.Instantiate(_levelPrefabs[levelNumber - 1], parentTransform).GetComponent<LevelView>(); },
                Lifetime.Scoped);
            
            builder.RegisterFactory<PrepareScenePresenterFactory, PrepareScenePresenter>(Lifetime.Scoped);
            builder.RegisterFactory<LevelPresenterFactory, LevelPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<PlayerPresenterFactory, PlayerPresenter>(Lifetime.Scoped);

            builder.RegisterFactory<CharacterFallObserverFactory, ICharacterFallObserver>(Lifetime.Scoped);
            builder.RegisterFactory<WallBlockRemoverFactory, IWallBlockRemover>(Lifetime.Scoped);
        }
    }
}