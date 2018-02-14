CREATE TABLE [dbo].[GT_Topic] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [PId]        INT           NULL,
    [Code]      NVARCHAR (50) NULL,
    [Title]      NVARCHAR (128) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [U_Topic] UNIQUE ([Code]),
    CONSTRAINT [FK_Topic_Topic] FOREIGN KEY ([Pid]) REFERENCES [dbo].[GT_Topic] ([Id])
);

