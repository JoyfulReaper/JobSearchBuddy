CREATE TABLE [dbo].[JobsContacts]
(
	[JobsContactsId] INT NOT NULL PRIMARY KEY IDENTITY,
	[JobId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	CONSTRAINT [FK_JobsContacts_Jobs] FOREIGN KEY ([JobId]) REFERENCES Jobs,
	CONSTRAINT [FK_JobsContacts_Contacts] FOREIGN KEY ([ContactId]) REFERENCES Contacts
)