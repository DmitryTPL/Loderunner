using Loderunner.Welcome;
using UniTaskPubSub;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class WelcomeLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<AsyncMessageBus>(Lifetime.Singleton);
            builder.RegisterEntryPoint<LoadService>();
        }
    }
}
