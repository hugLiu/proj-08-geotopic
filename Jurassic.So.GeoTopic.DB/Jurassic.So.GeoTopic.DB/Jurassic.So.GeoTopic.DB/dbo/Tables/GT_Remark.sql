CREATE TABLE [dbo].[GT_Remark]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Scoap] NVARCHAR(50)      NULL,
	[NatureKey] NVARCHAR(128)      NULL,
	[Comment] NVARCHAR(MAX)      NOT NULL,
	[UserId] INT      NULL,
	[CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
	CONSTRAINT [FK_Remark_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
)
