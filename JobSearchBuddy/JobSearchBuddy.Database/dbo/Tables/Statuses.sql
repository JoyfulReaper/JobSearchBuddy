CREATE TABLE [dbo].[Statuses]
(
	[StatusId] INT NOT NULL PRIMARY KEY IDENTITY,
    [StatusName] NVARCHAR(50) NOT NULL,
	CONSTRAINT [UQ_Statuses_StatusName] UNIQUE ([StatusName])
)