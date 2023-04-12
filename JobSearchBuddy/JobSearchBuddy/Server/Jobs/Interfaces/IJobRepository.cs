namespace JobSearchBuddy.Server.Jobs.Interfaces;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(int jobId);
    Task<int> CreateAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(int jobId);
}