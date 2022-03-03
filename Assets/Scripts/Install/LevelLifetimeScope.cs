using Loderunner.Gameplay;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class LevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LadderPresenter>(Lifetime.Scoped);
            builder.Register<WallBlockPresenter>(Lifetime.Scoped);
            builder.Register<BorderPresenter>(Lifetime.Scoped);
            builder.Register<SideToFallPresenter>(Lifetime.Scoped);
            builder.Register<FloorPresenter>(Lifetime.Scoped);
        }
    }
}