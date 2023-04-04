CREATE TABLE [dbo].[ContactsNotes]
(
	[ContactsNotesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_ContactsNotes_Jobs] FOREIGN KEY ([ContactId]) REFERENCES [Contacts]([ContactId]), 
    CONSTRAINT [FK_ContactsNotes_Notes] FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)