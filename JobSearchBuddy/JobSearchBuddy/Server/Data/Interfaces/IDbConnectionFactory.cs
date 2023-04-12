using System.Data;

namespace JobSearchBuddy.Server.Data.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}