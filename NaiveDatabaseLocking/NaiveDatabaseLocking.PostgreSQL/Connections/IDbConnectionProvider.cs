namespace NaiveDatabaseLocking.PostgreSQL.Connections;

public interface IDbConnectionProvider
{
    Npgsql.NpgsqlConnection CreateConnection();
}
