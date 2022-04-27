using Loderunner.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [Header("Gold"), SerializeField] private GoldView _goldViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterTypes(builder);
            RegisterFactories(builder);
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<GoldCreator>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LadderPresenterFactory>(Lifetime.Singleton);
            builder.Register<WallBlockPresenterFactory>(Lifetime.Singleton);
            builder.Register<RemovedWallPresenterFactory>(Lifetime.Singleton);
            builder.Register<BorderPresenterFactory>(Lifetime.Singleton);
            builder.Register<SideToFallPresenterFactory>(Lifetime.Singleton);
            builder.Register<FloorPresenterFactory>(Lifetime.Singleton);
            builder.Register<CrossbarPresenterFactory>(Lifetime.Singleton);
            builder.Register<GoldPresenterFactory>(Lifetime.Singleton);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterFactory<Transform, GoldView>(container =>
                parent => container.Instantiate(_goldViewPrefab, parent), Lifetime.Scoped);

            builder.RegisterFactory<LadderPresenterFactory, LadderPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<WallBlockPresenterFactory, WallBlockPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<RemovedWallPresenterFactory, RemovedWallPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<BorderPresenterFactory, BorderPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<SideToFallPresenterFactory, SideToFallPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<FloorPresenterFactory, FloorPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<CrossbarPresenterFactory, CrossbarPresenter>(Lifetime.Scoped);
            builder.RegisterFactory<GoldPresenterFactory, GoldPresenter>(Lifetime.Scoped);
        }
    }
}