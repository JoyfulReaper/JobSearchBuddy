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