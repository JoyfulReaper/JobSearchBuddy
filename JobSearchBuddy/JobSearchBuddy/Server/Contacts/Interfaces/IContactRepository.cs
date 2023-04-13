using JobSearchBuddy.Server.Jobs;
using JobSearchBuddy.Server.Notes;

namespace JobSearchBuddy.Server.Contacts.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int contactId);
    Task<int> CreateAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task<int> DeleteAsync(int contactId);

    Task<int> AddNoteAsync(int contactId, Note note);
    Task<IEnumerable<Note>> GetNotesAsync(int contactId);
    Task<int> DeleteNoteAsync(int contactId, int noteId);
}