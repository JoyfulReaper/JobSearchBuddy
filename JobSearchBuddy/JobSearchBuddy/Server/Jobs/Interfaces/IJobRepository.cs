using JobSearchBuddy.Server.Notes;

namespace JobSearchBuddy.Server.Jobs.Interfaces;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(int jobId);
    Task<int> CreateAsync(Job job);
    Task UpdateAsync(Job job);
    Task<int> DeleteAsync(int jobId);

    Task<int> AddNoteAsync(int contactId, Note note);
    Task<IEnumerable<Note>> GetNotesAsync(int contactId);
    Task<int> DeleteNoteAsync(int contactId, int noteId);
}