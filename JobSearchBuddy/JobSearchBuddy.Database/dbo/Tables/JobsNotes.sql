CREATE TABLE [dbo].[JobsNotes]
(
	[JobsNotesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [JobId] INT NOT NULL, 
    [NoteId] INT NOT NULL, 
    CONSTRAINT [FK_JobsNotes_Jobs] FOREIGN KEY ([JobId]) REFERENCES [Jobs]([JobId]), 
    CONSTRAINT [FK_JobsNotes_Notes] FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)