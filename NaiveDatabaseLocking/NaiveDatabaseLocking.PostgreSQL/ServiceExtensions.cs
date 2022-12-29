using Microsoft.Extensions.DependencyInjection;
using NaiveDatabaseLocking.LockProviders;
using NaiveDatabaseLocking.PostgreSQL;
using NaiveDatabaseLocking.PostgreSQL.Connections;

namespace NaiveDatabaseLocking.MSSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMSSqlLockProvider(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IDbConnectionProvider, PostgreSqlDbConnectionProvider>(Impl =>
        {
            return new PostgreSqlDbConnectionProvider(connectionString);
        });
        serviceCollection.AddScoped<ILockProvider, PostgreSqlLockProvider>();
        return serviceCollection;
    }
}