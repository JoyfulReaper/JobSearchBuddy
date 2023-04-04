CREATE DATABASE [JobSearchBuddy]


---- Tables ----
CREATE TABLE [dbo].[Contacts]
(
	[ContactId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [PhoneNumber] NVARCHAR(12) NULL UNIQUE, 
    [EmailAddress] NVARCHAR(100) NULL UNIQUE, 
    [CompanyName] NVARCHAR(50) NULL, 
    [JobTitle] NVARCHAR(50) NULL, 
    [IsExternalRecruiter] BIT NOT NULL DEFAULT 1,
    [DateAdded] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [DateUpdated] DATETIME2 NULL
)
GO

CREATE TABLE [dbo].[Notes]
(
	[NoteId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NoteText] NVARCHAR(MAX) NOT NULL, 
    [RelationshipType] VARCHAR(10) NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [DateUpdated] DATETIME2 NULL
)
GO

CREATE TABLE [dbo].[ContactsNotes]
(
	[ContactsNotesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_ContactsNotes_Jobs] FOREIGN KEY ([ContactId]) REFERENCES [Contacts]([ContactId]), 
    CONSTRAINT [FK_ContactsNotes_Notes] FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)
GO

CREATE TABLE [dbo].[Jobs]
(
	[JobId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [Url] NVARCHAR(150) NULL,
    [CompanyName] NVARCHAR(50) NOT NULL, 
    [ContactId] INT NULL, 
    [SalaryRange] NVARCHAR(50) NULL, 
    [IsRemote] BIT NOT NULL DEFAULT 0, 
    [Address1] NVARCHAR(100) NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] VARCHAR(2) NULL DEFAULT 'PA', 
    [Zip] NCHAR(10) NULL, 
    [IsInterested] BIT NOT NULL DEFAULT 0, 
    [IsApplied] BIT NOT NULL DEFAULT 0, 
    [StatusId] INT NOT NULL DEFAULT 1,
    [DateApplied] DATETIME2 NULL, 
    [DatePosted] DATETIME2 NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [DateUpdated] DATETIME2 NULL, 
    CONSTRAINT [FK_Jobs_Contacts] FOREIGN KEY ([ContactId]) REFERENCES [Contacts]([ContactId]), 
    CONSTRAINT [FK_Jobs_Statuses] FOREIGN KEY ([StatusId]) REFERENCES [Statuses]([StatusId]),
    CONSTRAINT [CK_Jobs_IsApplied_DateApplied] CHECK ((IsApplied = 0 AND DateApplied IS NULL) OR (IsApplied = 1 AND DateApplied IS NOT NULL))
)
GO

CREATE TABLE [dbo].[JobsContacts]
(
	[JobsContactsId] INT NOT NULL PRIMARY KEY IDENTITY,
	[JobId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	CONSTRAINT [FK_JobsContacts_Jobs] FOREIGN KEY ([JobId]) REFERENCES Jobs,
	CONSTRAINT [FK_JobsContacts_Contacts] FOREIGN KEY ([ContactId]) REFERENCES Contacts
)
GO

CREATE TABLE [dbo].[JobsNotes]
(
	[JobsNotesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [JobId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_JobsNotes_Jobs] FOREIGN KEY ([JobId]) REFERENCES [Jobs]([JobId]), 
    CONSTRAINT [FK_JobsNotes_Notes] FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)
GO

CREATE TABLE [dbo].[Statuses]
(
	[StatusId] INT NOT NULL PRIMARY KEY IDENTITY,
    [StatusName] NVARCHAR(50) NOT NULL,
	CONSTRAINT [UQ_Statuses_StatusName] UNIQUE ([StatusName])
)
GO

------- Lookup Table Data -------

INSERT INTO [dbo].[Statuses] ([StatusName])
VALUES 
	('New'),
	('Applied'), 
	('Screening'), 
	('Interview'), 
	('Offer'),
	('Denied'),
	('Accepted'),
	('Rejected'),
	('Withdrawn'),
	('No Response'),
	('Other'),
	('Not Interested')
GO