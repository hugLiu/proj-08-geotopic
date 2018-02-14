CREATE TABLE [dbo].[GT_TopicInRoles] (
    [RoleId]  INT NOT NULL,
    [TopicId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [TopicId] ASC),
    CONSTRAINT [fk_Id] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[GT_Topic] ([Id]),
    CONSTRAINT [fk_RoleId2] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
);

