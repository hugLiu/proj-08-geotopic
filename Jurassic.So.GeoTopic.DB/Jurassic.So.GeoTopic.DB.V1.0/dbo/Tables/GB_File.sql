CREATE TABLE [dbo].[GB_File] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [SubmissiomId] INT             NULL,
    [FileName]     NVARCHAR (50)   NOT NULL,
    [File]         VARBINARY (MAX) NULL,
    [CreatedDate]  DATETIME        NOT NULL,
    [CreatedBy]    NVARCHAR (50)   NULL,
    [UpdatedDate]  DATETIME        NOT NULL,
    [UpdatedBy]    NVARCHAR (50)   NULL,
    [IsDelete]     BIT             NOT NULL,
    CONSTRAINT [PK_GB_File] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GB_File_GB_Submission] FOREIGN KEY ([SubmissiomId]) REFERENCES [dbo].[GB_Submission] ([Id])
);

