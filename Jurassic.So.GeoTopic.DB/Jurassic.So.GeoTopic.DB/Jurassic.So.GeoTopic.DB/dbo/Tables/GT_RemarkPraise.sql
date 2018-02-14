CREATE TABLE [dbo].[GT_RemarkPraise]
(
	[RemarkId] INT NOT NULL,
	[UserId] INT NOT NULL,
    CONSTRAINT [PK_RemarkPraise] PRIMARY KEY ([UserId], [RemarkId]),
	CONSTRAINT [FK_Praise_Remark] FOREIGN KEY ([RemarkId]) REFERENCES [dbo].[GT_Remark] ([Id]),
	CONSTRAINT [FK_Praise_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId]), 
)
