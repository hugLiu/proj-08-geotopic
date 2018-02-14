CREATE TABLE [dbo].[GT_TopicCard] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [TopicId]    INT            NOT NULL,
	[OrderId]    INT            NULL,
    [Title]      NVARCHAR (128) NOT NULL,
    [Definition] NVARCHAR (MAX) NULL,
    [CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Card_Topic] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[GT_Topic] ([Id])
);

