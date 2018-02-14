CREATE TABLE [dbo].[Flow_Run] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (200) NULL,
    [InstanceId]  INT            NOT NULL,
    [RunUrl]      NVARCHAR (200) NULL,
    [StepId]      INT            NOT NULL,
    [Result]      INT            NOT NULL,
    [OperatorId]  INT            NULL,
    [OperateTime] DATETIME2 (7)  NOT NULL,
    [Remark]      NVARCHAR (200) NULL,
    CONSTRAINT [PK_Plan_Approval] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Flow_Run_Flow_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Flow_Instance] ([Id]),
    CONSTRAINT [FK_Flow_Run_Flow_Instance1] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Flow_Instance] ([Id]),
    CONSTRAINT [FK_Flow_Run_Flow_Step] FOREIGN KEY ([StepId]) REFERENCES [dbo].[Flow_Step] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'说明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'执行时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'OperateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'执行人ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'OperatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'状态值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'步骤ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'StepId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实例步骤要执行的URL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'RunUrl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程实例ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'InstanceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实例步骤名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程实例步骤表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Run', @level2type = N'COLUMN', @level2name = N'Id';

