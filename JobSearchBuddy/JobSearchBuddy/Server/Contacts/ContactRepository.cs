using JobSearchBuddy.Server.Contacts.Interfaces;

namespace JobSearchBuddy.Server.Contacts;

public class ContactRepository : IContactRepository
{
    public Task<bool> DeleteContactAsync(int contactId)
    {
        throw new NotImplementedException();
    }

    public Task<Contact?> GetContactByIdAsync(int contactId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Contact>> GetContactsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Contact> SaveContactAsync(Contact contact)
    {
        throw new NotImplementedException();
    }
}