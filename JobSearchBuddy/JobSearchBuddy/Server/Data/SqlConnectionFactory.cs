using JobSearchBuddy.Server.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobSearchBuddy.Server.Data;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _config;

    public SqlConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _config.GetConnectionString("DefaultConnection");
        return new SqlConnection(connectionString);
    }
}