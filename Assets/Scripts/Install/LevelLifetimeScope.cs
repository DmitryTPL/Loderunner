using Loderunner.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private GoldView _goldViewPrefab;
        [SerializeField] private GuardianView _guardianViewPrefab;
        [SerializeField] private Transform _pool;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterTypes(builder);
            RegisterFactories(builder);
        }

        private void RegisterTypes(IContainerBuilder builder)
        {
            builder.Register<IPathFinder, AStarPathFinder>(Lifetime.Singleton);
            builder.Register<IGuardiansIdPool, GuardiansIdPool>(Lifetime.Singleton);
            
            builder.Register<GoldCreator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GuardianCreator>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter("pool", _pool);

            builder.Register<LadderPresenter>(Lifetime.Transient);
            builder.Register<WallBlockPresenter>(Lifetime.Transient);
            builder.Register<RemovedWallPresenter>(Lifetime.Transient);
            builder.Register<BorderPresenter>(Lifetime.Transient);
            builder.Register<SideToFallPresenter>(Lifetime.Transient);
            builder.Register<FloorPresenter>(Lifetime.Transient);
            builder.Register<CrossbarPresenter>(Lifetime.Transient);
            builder.Register<GoldPresenter>(Lifetime.Transient);
            builder.Register<FinalLadderPresenter>(Lifetime.Transient);
            builder.Register<LevelExitPresenter>(Lifetime.Transient);
            builder.Register<GuardianSpawnerPresenter>(Lifetime.Transient);
            builder.Register<GuardianPresenter>(Lifetime.Transient);
            builder.Register<GuardianSpawnerPresenter>(Lifetime.Transient);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterFactory<Transform, GoldView>(container =>
                parent => container.Instantiate(_goldViewPrefab, parent), Lifetime.Singleton);
            
            builder.RegisterFactory<Transform, GuardianView>(container =>
                parent => container.Instantiate(_guardianViewPrefab, parent), Lifetime.Singleton);
        }
    }
}