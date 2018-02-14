CREATE TABLE [dbo].[Flow_Graph] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [StartStepId] INT            NOT NULL,
    CONSTRAINT [PK_Proj_Flow_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Flow_Main_Flow_Step] FOREIGN KEY ([StartStepId]) REFERENCES [dbo].[Flow_Step] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始节点', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Graph', @level2type = N'COLUMN', @level2name = N'StartStepId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Graph', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程定义主表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Graph', @level2type = N'COLUMN', @level2name = N'Id';

