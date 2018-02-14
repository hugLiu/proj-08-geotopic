CREATE TABLE [dbo].[Flow_Instance] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [GraphId]   INT           NOT NULL,
    [StartTime] DATETIME2 (7) NOT NULL,
    [CreaterId] INT           NOT NULL,
    CONSTRAINT [PK_Flow_Instance] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Flow_Instance_Flow_Main] FOREIGN KEY ([GraphId]) REFERENCES [dbo].[Flow_Graph] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Instance', @level2type = N'COLUMN', @level2name = N'CreaterId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'开始时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Instance', @level2type = N'COLUMN', @level2name = N'StartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程定义ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Instance', @level2type = N'COLUMN', @level2name = N'GraphId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实例名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Instance', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程实例表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Instance', @level2type = N'COLUMN', @level2name = N'Id';

