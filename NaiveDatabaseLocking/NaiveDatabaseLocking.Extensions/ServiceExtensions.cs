using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NaiveDatabaseLocking.LockProviders;

namespace NaiveDatabaseLocking.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLockFactory(this IServiceCollection serviceCollection)
    {
        serviceCollection.RemoveAll(typeof(ILockProvider));
        serviceCollection.AddScoped<ILockFactory, LockFactory>();
        serviceCollection.AddScoped<ILockProvider, InMemoryLockProvider>();
        return serviceCollection;
    }
}