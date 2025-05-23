using System.Data;
using Microsoft.Data.SqlClient;

namespace Benchy.DapperVsEfCore.Database.Factories;

public static class ConnectionFactory
{
    public static async Task<IDbConnection> Create(string connectionString)
    {
        var connection =  new SqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}