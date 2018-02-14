CREATE TABLE [dbo].[GT_Code] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CodeTypeId]  INT            NOT NULL,
    [Code]        NVARCHAR (100) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (50)  NULL,
    [UpdatedDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_GT_Code] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GT_Code_GT_CodeType] FOREIGN KEY ([CodeTypeId]) REFERENCES [dbo].[GT_CodeType] ([Id])
);



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GT_Code]
    ON [dbo].[GT_Code]([CodeTypeId] ASC, [Code] ASC);

