CREATE TABLE [dbo].[GT_UserBehavior]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [BehaviorType] NVARCHAR(50) NOT NULL, 
    [Type] NVARCHAR(50) NULL, 
	[Title] NVARCHAR(128) NOT NULL, 
    [Content] NVARCHAR(1024) NOT NULL,
	[CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
	CONSTRAINT [FK_Behavior_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
)
