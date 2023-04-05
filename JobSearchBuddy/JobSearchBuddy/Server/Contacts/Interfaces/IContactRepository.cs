namespace JobSearchBuddy.Server.Contacts.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetContactByIdAsync(int contactId);
    Task<List<Contact>> GetContactsAsync();
    Task<Contact> SaveContactAsync(Contact contact);
    Task<bool> DeleteContactAsync(int contactId);
}