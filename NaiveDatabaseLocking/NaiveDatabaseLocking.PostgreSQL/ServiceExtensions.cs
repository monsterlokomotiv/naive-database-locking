using Microsoft.Extensions.DependencyInjection;
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
        return serviceCollection;
    }
}