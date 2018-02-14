CREATE TABLE [dbo].[GT_CodeType] (
    [Id]          INT            NOT NULL,
    [Code]        NVARCHAR (100) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF__GT_CodeTy__Creat__50C5FA01] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (50)  NULL,
    [UpdatedDate] DATETIME       CONSTRAINT [DF__GT_CodeTy__Updat__51BA1E3A] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_GT_CodeType_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);



--GO
--CREATE unique INDEX [IX_GT_CodeType_Code] ON [dbo].[GT_CodeType] ([Code])

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GT_CodeType]
    ON [dbo].[GT_CodeType]([Code] ASC);

