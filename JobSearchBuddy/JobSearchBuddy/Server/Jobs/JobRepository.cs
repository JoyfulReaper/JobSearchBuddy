using Dapper;
using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Server.Data.Interfaces;
using JobSearchBuddy.Server.Jobs.Interfaces;
using JobSearchBuddy.Server.Notes;
using Microsoft.Data.SqlClient;
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
        //const string sql = "SELECT * FROM Jobs WHERE JobId = @JobId AND DateDeleted IS NULL";
        const string sql = @"SELECT * FROM Jobs j LEFT JOIN
                                JobsNotes jn ON j.JobId = jn.JobId LEFT JOIN
                                Notes n ON jn.NoteId = n.NoteId
                                WHERE j.JobId = @JobId AND j.DateDeleted IS NULL;
                                SELECT * FROM Notes n INNER JOIN 
                                JobsNotes jn ON n.NoteId = jn.NoteId WHERE jn.JobId = @JobId;";

        using var reader = await connection.QueryMultipleAsync(sql, new { JobId = jobId });
        var job = await reader.ReadSingleOrDefaultAsync<Job>();

        if (!reader.IsConsumed && job is not null)
        {
            job.Notes = (await reader.ReadAsync<Note>()).ToList();
        }

        return job;
    }

    public async Task<int> CreateAsync(Job job)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = @"INSERT INTO Jobs(Title, Description, Url, CompanyName, ContactId, SalaryRange, IsRemote, Address1, City, State, Zip, IsInterested, IsApplied, StatusId, DateApplied, DatePosted, DateCreated)
                                 VALUES(@Title, @Description, @Url, @CompanyName, @ContactId, @SalaryRange, @IsRemote, @Address1, @City, @State, @Zip, @IsInterested, @IsApplied, @StatusId, @DateApplied, @DatePosted, SYSDATETIME());
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

    public async Task<int> DeleteAsync(int jobId)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        const string sql = "UPDATE Jobs SET DateDeleted = SYSDATETIME() WHERE JobId = @JobId AND DateDeleted IS NULL";
        return await connection.ExecuteAsync(sql, new { JobId = jobId });
    }

    public async Task<IEnumerable<Note>> GetNotesAsync(int jobId)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();

        var notes = await connection.QueryAsync<Note>(@"
        SELECT n.*
        FROM [dbo].[Notes] n
        INNER JOIN [dbo].[JobsNotes] jn ON jn.NoteId = n.NoteId
        WHERE jn.JobId = @JobId
    ", new { JobId = jobId });

        return notes;
    }

    public async Task<int> DeleteNoteAsync(int jobId, int noteId)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        connection.Open();
        var transaction = connection.BeginTransaction();

        try
        {
            var rowsAffected = await connection.ExecuteAsync(@"
            DELETE FROM [dbo].[JobsNotes]
            WHERE [JobId] = @JobId AND [NoteId] = @NoteId
            ", new { JobId = jobId, NoteId = noteId }, transaction);
            if(rowsAffected == 0) 
            {
                throw new Exception($"Note with ID {noteId} is not associated with job with ID {jobId}");
            }


            rowsAffected = await connection.ExecuteAsync(@"
            DELETE FROM [dbo].[Notes]
            WHERE [NoteId] = @NoteId AND [RelationshipType] = 'Job'
            ", new { NoteId = noteId }, transaction);

            if (rowsAffected == 0)
            {
                throw new Exception($"Unable to delete Note with ID {noteId}");
            }

            transaction.Commit();

            return rowsAffected;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Failed to delete note from job", ex);
        }
    }

    public async Task<int> AddNoteAsync(int jobId, Note note)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateConnection();
        connection.Open();
        var transaction = connection.BeginTransaction();

        try
        {
            var noteId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO [dbo].[Notes] ([NoteText], [RelationshipType], [DateCreated])
                VALUES (@NoteText, @RelationshipType, SYSDATETIME());
                SELECT SCOPE_IDENTITY();
            ", new { NoteText = note.NoteText, RelationshipType = "Job" }, transaction);

            await connection.ExecuteAsync(@"
                INSERT INTO [dbo].[JobsNotes] ([JobId], [NoteId])
                VALUES (@JobId, @NoteId);
            ", new { JobId = jobId, NoteId = noteId }, transaction);

            transaction.Commit();

            note.NoteId = noteId;
            return noteId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Failed to add note to job", ex);
        }
    }
}