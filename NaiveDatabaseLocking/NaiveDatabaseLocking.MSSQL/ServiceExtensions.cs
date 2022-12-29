using Microsoft.Extensions.DependencyInjection;
using NaiveDatabaseLocking.MSSQL.Connections;

namespace NaiveDatabaseLocking.MSSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMSSqlLockProvider(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IDbConnectionProvider, MsSqlDbConnectionProvider>(Impl =>
        {
            return new MsSqlDbConnectionProvider(connectionString);
        });
        return serviceCollection;
    }
}