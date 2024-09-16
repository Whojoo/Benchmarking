using System.Data;
using Microsoft.Data.SqlClient;

namespace Benchy.DapperVsEfCore.Database.Factories;

public static class ConnectionFactory
{
    public static IDbConnection Create()
    {
        return new SqlConnection(DataSchemaConstants.ConnectionString);
    }
}