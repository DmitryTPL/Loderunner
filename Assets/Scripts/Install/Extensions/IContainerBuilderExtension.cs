using System;
using Loderunner.Install;
using VContainer;

public static class IContainerBuilderExtension
{
    public static RegistrationBuilder RegisterFactory<TFactory, TEntity>(this IContainerBuilder builder, Lifetime lifetime)
        where TFactory : IFactory<TEntity>
        where TEntity : IDisposable
    {
        return builder.RegisterFactory<TEntity>(container => container.Resolve<TFactory>().Create, lifetime);
    }
}