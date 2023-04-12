using Dapper;
using JobSearchBuddy.Server.Data.Interfaces;
using JobSearchBuddy.Server.Jobs.Interfaces;
using System.Data;


namespace JobSearchBuddy.Server.Jobs;

public class JobRepository : IJobRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public JobRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Jobs WHERE DateDeleted IS NULL";
        return await connection.QueryAsync<Job>(sql);
    }

    public async Task<Job?> GetByIdAsync(int jobId)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Jobs WHERE JobId = @JobId AND DateDeleted IS NULL";
        return await connection.QuerySingleOrDefaultAsync<Job>(sql, new { JobId = jobId });
    }

    public async Task<int> CreateAsync(Job job)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = @"INSERT INTO Jobs(Title, Description, Url, CompanyName, ContactId, SalaryRange, IsRemote, Address1, City, State, Zip, IsInterested, IsApplied, StatusId, DateApplied, DatePosted, DateCreated)
                                 VALUES(@Title, @Description, @Url, @CompanyName, @ContactId, @SalaryRange, @IsRemote, @Address1, @City, @State, @Zip, @IsInterested, @IsApplied, @StatusId, @DateApplied, @DatePosted, @DateCreated);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";

        var id = await connection.QuerySingleAsync<int>(sql, job);
        job.JobId = id;

        return id;
    }

    public async Task UpdateAsync(Job job)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = @"UPDATE Jobs SET Title = @Title, Description = @Description, Url = @Url, CompanyName = @CompanyName, ContactId = @ContactId, SalaryRange = @SalaryRange, IsRemote = @IsRemote, Address1 = @Address1,
                                City = @City, State = @State, Zip = @Zip, IsInterested = @IsInterested, IsApplied = @IsApplied, StatusId = @StatusId, DateApplied = @DateApplied, DatePosted = @DatePosted, DateUpdated = GETDATE() WHERE JobId = @JobId AND DateDeleted IS NULL";
        await connection.ExecuteAsync(sql, job);
    }

    public async Task DeleteAsync(int jobId)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = "UPDATE Jobs SET DateDeleted = SYSDATETIME() WHERE JobId = @JobId AND DateDeleted IS NULL";
        await connection.ExecuteAsync(sql, new { JobId = jobId });
    }
}