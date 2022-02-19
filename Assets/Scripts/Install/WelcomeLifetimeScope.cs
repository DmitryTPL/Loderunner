using Loderunner.Welcome;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class WelcomeLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoadService>();
        }
    }
}
