using Microsoft.Data.SqlClient;

namespace NaiveDatabaseLocking.MSSQL.Connections;

public class MsSqlDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;
    public MsSqlDbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
