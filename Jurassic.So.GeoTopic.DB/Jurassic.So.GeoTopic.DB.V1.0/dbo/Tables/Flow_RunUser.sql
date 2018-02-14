CREATE TABLE [dbo].[Flow_RunUser] (
    [RunId]  INT NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [PK_Flow_RunUser] PRIMARY KEY CLUSTERED ([RunId] ASC, [UserId] ASC),
    CONSTRAINT [FK_Flow_RunUser_Flow_Run] FOREIGN KEY ([RunId]) REFERENCES [dbo].[Flow_Run] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实例步骤ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_RunUser', @level2type = N'COLUMN', @level2name = N'RunId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_RunUser', @level2type = N'COLUMN', @level2name = N'UserId';

