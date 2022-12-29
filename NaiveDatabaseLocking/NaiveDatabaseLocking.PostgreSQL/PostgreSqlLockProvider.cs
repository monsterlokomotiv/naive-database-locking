using Microsoft.Extensions.Logging;
using NaiveDatabaseLocking.LockProviders;
using NaiveDatabaseLocking.Locks;
using NaiveDatabaseLocking.PostgreSQL.Connections;
using Npgsql;

namespace NaiveDatabaseLocking.PostgreSQL;

public class PostgreSqlLockProvider : ILockProvider
{
    private readonly IDbConnectionProvider _connectionProvider;
    private readonly ILogger _logger;
    public PostgreSqlLockProvider(IDbConnectionProvider connectionProvider,
        ILogger<PostgreSqlLockProvider> logger)
    {
        _connectionProvider = connectionProvider;
        _logger = logger;
    }

    public async Task<ILockContainer> GetLock(string key)
    {
        using var conn = _connectionProvider.CreateConnection();
        await conn.OpenAsync();

        var lockExists = await KeyIsAlreadyLocked(conn, key);
        if (lockExists)
            return new LockContainer(null, LockCreationStatus.AlreadyExists);

        try
        {
            var createdLock = new Lock(Guid.NewGuid(), key, DateTime.UtcNow.AddSeconds(5));
            await InsertLock(conn, createdLock);
            return new LockContainer(createdLock, LockCreationStatus.Created);
        }
        catch(Exception ex)
        {
            //THE RACE IS ON BABY!
            return new LockContainer(null, LockCreationStatus.AlreadyExists);
        }
    }

    public async Task ReleaseLock(ILock toRelease)
    {
        using var conn = _connectionProvider.CreateConnection();
        await conn.OpenAsync();

        const string DeleteLockSql = """
            DELETE FROM Locks
            WHERE Id = @id
            """;

        var cmd = conn.CreateCommand();
        cmd.CommandText = DeleteLockSql;
        var idParam = cmd.CreateParameter();
        idParam.ParameterName = "id";
        idParam.Value = toRelease.Id;
        idParam.DbType = System.Data.DbType.Guid;
    }

    private static async Task InsertLock(NpgsqlConnection connection, Lock createdLock)
    {
        const string InsertLockSql = 
            """
            INSERT INTO Locks
            VALUES (@id, @key, @expirationTime)
            """;

        var cmd = connection.CreateCommand();
        cmd.CommandText = InsertLockSql;
        var idParam = cmd.CreateParameter();
        idParam.ParameterName = "id";
        idParam.Value = createdLock.Key;
        idParam.DbType = System.Data.DbType.Guid;
        var keyParam = cmd.CreateParameter();
        keyParam.ParameterName = "key";
        keyParam.Value = createdLock.Key;
        keyParam.DbType = System.Data.DbType.String;
        var expParam = cmd.CreateParameter();
        expParam.ParameterName = "expirationTime";
        expParam.Value = createdLock.Key;
        expParam.DbType = System.Data.DbType.DateTime2;

        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task<bool> KeyIsAlreadyLocked(NpgsqlConnection connection, string key)
    {
        const string CheckIfLockExistsSql = 
            """
            SELECT COUNT(*)
            FROM Locks
            WHERE Key = @key
            """;

        var cmd = connection.CreateCommand();
        cmd.CommandText = CheckIfLockExistsSql;
        var param = cmd.CreateParameter();
        param.ParameterName = "key";
        param.Value = key;
        param.DbType = System.Data.DbType.String;
        var result = await cmd.ExecuteScalarAsync();

        if (result == null)
            return false;

        return ((int)result) > 0;
    }
}
