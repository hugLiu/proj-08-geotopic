CREATE TABLE [dbo].[GT_TopicIndex] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [TopicId]   INT            NOT NULL,
    [IndexDefinitionId] INT            NOT NULL,
    [Value]     NVARCHAR (128) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__GT_TopicIndex] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [U_TopicIndex] UNIQUE ([TopicId],[IndexDefinitionId],[Value]),
	CONSTRAINT [FK_Index_Definition] FOREIGN KEY ([IndexDefinitionId]) REFERENCES [dbo].[GT_IndexDefinition] ([Id]),
    CONSTRAINT [FK_Index_Topic] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[GT_Topic] ([Id])
);

