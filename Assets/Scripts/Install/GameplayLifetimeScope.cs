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
            builder.RegisterInstance(_configsHolder.GuardianConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.GameConfig).AsSelf();
            builder.RegisterInstance(_configsHolder.WallBlockRemoveConfig).AsSelf();
            builder.RegisterInstance(_levelsConfigsHolder.Levels).As<IReadOnlyList<LevelConfig>>();
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<AsyncEnumerableMessageBus>(Lifetime.Singleton).As<IAsyncEnumerablePublisher, IAsyncEnumerableReceiver>();
            
            builder.Register<ITime, DefaultTimeHandler>(Lifetime.Singleton);
            
            builder.Register<LevelCreator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelData>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<LevelPresenter>(Lifetime.Transient);
            builder.Register<PrepareScenePresenter>(Lifetime.Singleton);
            builder.Register<CameraFollowPresenter>(Lifetime.Singleton);
            builder.Register<InitialCameraLevelPassagePresenter>(Lifetime.Singleton);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterFactory<int, LevelView>(container =>
                levelIndex => container.Instantiate(_levelPrefabs[levelIndex], _levelPosition), Lifetime.Singleton);
        }
    }
}