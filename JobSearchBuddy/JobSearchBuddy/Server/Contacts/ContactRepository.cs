using Dapper;
using JobSearchBuddy.Server.Contacts.Interfaces;
using JobSearchBuddy.Server.Data.Interfaces;
using JobSearchBuddy.Server.Notes;
using System.Data;

namespace JobSearchBuddy.Server.Contacts
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ContactRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(Contact contact)
        {
            const string sql = @"INSERT INTO Contacts (FirstName, LastName, PhoneNumber, EmailAddress, CompanyName, JobTitle, IsExternalRecruiter)
                                VALUES (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @CompanyName, @JobTitle, @IsExternalRecruiter);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _connectionFactory.CreateConnection();
            var id = await connection.QuerySingleOrDefaultAsync<int>(sql, contact);

            contact.ContactId = id;
            return id;
        }

        public async Task<int> DeleteAsync(int contactId)
        {
            const string sql = @"UPDATE Contacts SET DateDeleted = SYSDATETIME() WHERE ContactId = @ContactId;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { ContactId = contactId });
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM Contacts WHERE DateDeleted IS NULL;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Contact>(sql);
        }

        public async Task<Contact?> GetByIdAsync(int contactId)
        {
            const string sql = @"SELECT c.ContactId, c.FirstName, c.LastName, c.PhoneNumber, 
                                 c.EmailAddress, c.CompanyName, c.JobTitle, c.IsExternalRecruiter,
                                 c.DateAdded, c.DateUpdated FROM Contacts c LEFT JOIN
                                 ContactsNotes cn ON c.ContactId = cn.ContactId LEFT JOIN
                                 Notes n ON cn.NoteId = n.NoteId
                                 WHERE c.ContactId = @ContactId AND c.DateDeleted IS NULL;";


            using var connection = _connectionFactory.CreateConnection();
            using var reader = await connection.QueryMultipleAsync(sql, new { ContactId = contactId });
            var contact = await reader.ReadSingleOrDefaultAsync<Contact>();

            if(!reader.IsConsumed)
            {
                contact.Notes = (await reader.ReadAsync<Note>()).ToList();
            }

            return contact;
        }

        public async Task UpdateAsync(Contact contact)
        {
            const string sql = @"UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, 
                                PhoneNumber = @PhoneNumber, EmailAddress = @EmailAddress, 
                                CompanyName = @CompanyName, JobTitle = @JobTitle, IsExternalRecruiter = @IsExternalRecruiter, 
                                DateUpdated = SYSDATETIME() WHERE ContactId = @ContactId AND DateDeleted IS NULL";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, contact);
        }

        public async Task<int> AddNoteAsync(int contactId, Note note)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            var transaction = connection.BeginTransaction();

            try
            {
                var noteId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO [dbo].[Notes] ([NoteText], [RelationshipType], [DateCreated])
                VALUES (@NoteText, @RelationshipType, SYSDATETIME());
                SELECT SCOPE_IDENTITY();
            ", new { NoteText = note.NoteText, RelationshipType = "Contact" }, transaction);

                await connection.ExecuteAsync(@"
                INSERT INTO [dbo].[ContactsNotes] ([ContactId], [NoteId])
                VALUES (@ContactId, @NoteId);
            ", new { ContactId = contactId, NoteId = noteId }, transaction);

                transaction.Commit();

                note.NoteId = noteId;

                return noteId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to add note to contact", ex);
            }
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(int contactId)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            var notes = await connection.QueryAsync<Note>(@"
                SELECT n.*
                FROM [dbo].[Notes] n INNER JOIN 
                [dbo].[ContactsNotes] cn ON cn.NoteId = n.NoteId INNER JOIN
                [dbo].[Contacts] c ON c.ContactId = cn.ContactId
                WHERE cn.ContactId = @ContactId AND c.DateDeleted IS NULL
            ", new { ContactId = contactId });

            return notes;
        }

        public async Task<int> DeleteNoteAsync(int contactId, int noteId)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            var transaction = connection.BeginTransaction();

            try
            {
                var rowsAffected = await connection.ExecuteAsync(@"
                DELETE FROM [dbo].[ContactsNotes]
                WHERE [ContactId] = @ContactId AND [NoteId] = @NoteId
                ", new { NoteId = noteId, ContactId = contactId }, transaction);
                if (rowsAffected == 0)
                {
                    throw new Exception($"Note with ID {noteId} is not associated with contact with ID {contactId}");
                }


                rowsAffected = await connection.ExecuteAsync(@"
                DELETE FROM [dbo].[Notes]
                WHERE [NoteId] = @NoteId AND [RelationshipType] = 'Contact'
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
                throw new Exception("Failed to delete note from contact", ex);
            }
        }
    }
}