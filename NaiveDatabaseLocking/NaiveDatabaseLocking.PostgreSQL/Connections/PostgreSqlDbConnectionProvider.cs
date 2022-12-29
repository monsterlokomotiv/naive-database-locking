using Npgsql;

namespace NaiveDatabaseLocking.PostgreSQL.Connections;

public class PostgreSqlDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;
    public PostgreSqlDbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
