using System.Collections.Generic;
using Loderunner.Gameplay;
using Loderunner.Gameplay.Logic.Gameplay;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [Header("Player"), SerializeField] private PlayerView _playerViewPrefab;
        [Header("Levels"), SerializeField] private List<LevelView> _levelPrefabs = new();
        [SerializeField] private Transform _levelPosition;
        [Header("Configs"), SerializeField] private ConfigsHolder _configsHolder;
        [SerializeField] private LevelsConfigsHolder _levelsConfigsHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterScriptableObjects(builder);
            RegisterTypes(builder);
            RegisterFactories(builder);
        }

        private void RegisterScriptableObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_configsHolder.PlayerConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.GameConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.WallBlockRemoveConfig).AsSelf();
            builder.RegisterInstance(_levelsConfigsHolder.Levels).As<IReadOnlyList<LevelConfig>>();
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<LevelCreator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerCreator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelData>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ITime, DefaultTimeHandler>(Lifetime.Singleton);
            builder.Register<AsyncEnumerableMessageBus>(Lifetime.Singleton).As<IAsyncEnumerablePublisher, IAsyncEnumerableReceiver>();
            builder.Register<PlayerStateData>(Lifetime.Scoped).AsSelf();
            builder.Register<PlayerStateContext>(Lifetime.Scoped).AsSelf();
            builder.Register<LevelFinishedObserver>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PrepareScenePresenterFactory>(Lifetime.Singleton);
            builder.Register<LevelPresenterFactory>(Lifetime.Singleton);
            builder.Register<PlayerPresenterFactory>(Lifetime.Singleton);
            builder.Register<CharacterFallObserverFactory>(Lifetime.Singleton);
            builder.Register<WallBlockRemoverFactory>(Lifetime.Singleton);
            builder.Register<CameraFollowPresenterFactory>(Lifetime.Singleton);
            builder.Register<PlayerSpawnerPresenterFactory>(Lifetime.Singleton);
            builder.Register<InitialCameraLevelPassagePresenterFactory>(Lifetime.Singleton);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterFactory<Transform, PlayerView>(container =>
                parent => container.Instantiate(_playerViewPrefab, parent), Lifetime.Scoped);

            builder.RegisterFactory<int, LevelView>(container =>
                levelNumber => container.Instantiate(_levelPrefabs[levelNumber - 1], _levelPosition), Lifetime.Scoped);

            builder.RegisterFactory<PrepareScenePresenterFactory, PrepareScenePresenter>(Lifetime.Scoped);
            builder.RegisterFactory<LevelPresenterFactory, LevelPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<PlayerPresenterFactory, PlayerPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<CameraFollowPresenterFactory, CameraFollowPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<PlayerSpawnerPresenterFactory, PlayerSpawnerPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<InitialCameraLevelPassagePresenterFactory, InitialCameraLevelPassagePresenter>(Lifetime.Scoped);

            builder.RegisterFactory<CharacterFallObserverFactory, ICharacterFallObserver>(Lifetime.Scoped);
            builder.RegisterFactory<WallBlockRemoverFactory, IWallBlockRemover>(Lifetime.Scoped);
        }
    }
}