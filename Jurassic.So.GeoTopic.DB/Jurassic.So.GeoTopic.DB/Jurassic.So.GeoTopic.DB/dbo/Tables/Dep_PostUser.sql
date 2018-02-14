CREATE TABLE [dbo].[Dep_PostUser] (
    [DepPostId]        INT      NOT NULL,
    [DepUserId]        INT      NOT NULL,
    [OperationId]      INT      NOT NULL,
    [CreateDatetime]   DATETIME CONSTRAINT [DF_Dep_PostUser_CreateDatetime] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime] DATETIME CONSTRAINT [DF_Dep_PostUser_ModifiedDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Dep_PostUser] PRIMARY KEY CLUSTERED ([DepPostId] ASC, [DepUserId] ASC),
    CONSTRAINT [FK_Dep_PostUser_Dep_DepPost] FOREIGN KEY ([OperationId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_Dep_PostUser_Dep_DepPost1] FOREIGN KEY ([DepPostId]) REFERENCES [dbo].[Dep_DepPost] ([Id]),
    CONSTRAINT [FK_Dep_PostUser_Dep_DepUser] FOREIGN KEY ([DepUserId]) REFERENCES [dbo].[Dep_DepUser] ([Id])
);

