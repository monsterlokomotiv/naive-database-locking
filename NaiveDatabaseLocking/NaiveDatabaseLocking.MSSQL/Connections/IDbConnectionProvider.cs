using Microsoft.Data.SqlClient;

namespace NaiveDatabaseLocking.MSSQL.Connections;

public interface IDbConnectionProvider
{
    SqlConnection CreateConnection();
}
