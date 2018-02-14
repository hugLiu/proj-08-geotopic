CREATE TABLE [dbo].[Flow_Step] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)  NOT NULL,
    [Url]           NVARCHAR (200) NOT NULL,
    [UserFilter]    VARCHAR (1000) NULL,
    [DefaultStatus] INT            NOT NULL,
    CONSTRAINT [PK_Proj_Flow] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程步骤表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Step', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'步骤名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Step', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表单地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Step', @level2type = N'COLUMN', @level2name = N'Url';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户筛选条件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Step', @level2type = N'COLUMN', @level2name = N'UserFilter';

