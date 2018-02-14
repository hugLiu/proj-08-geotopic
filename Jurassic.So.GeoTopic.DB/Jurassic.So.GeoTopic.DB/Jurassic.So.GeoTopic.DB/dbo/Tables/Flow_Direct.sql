CREATE TABLE [dbo].[Flow_Direct] (
    [StepId]     INT NOT NULL,
    [NextStepId] INT NOT NULL,
    [OnResult]   INT NOT NULL,
    CONSTRAINT [PK_Flow_Direct] PRIMARY KEY CLUSTERED ([StepId] ASC, [NextStepId] ASC),
    CONSTRAINT [FK_Flow_Direct_Flow_Step] FOREIGN KEY ([StepId]) REFERENCES [dbo].[Flow_Step] ([Id]),
    CONSTRAINT [FK_Flow_Direct_Flow_Step1] FOREIGN KEY ([NextStepId]) REFERENCES [dbo].[Flow_Step] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'执行下一步的结果条件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Direct', @level2type = N'COLUMN', @level2name = N'OnResult';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下一步ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Direct', @level2type = N'COLUMN', @level2name = N'NextStepId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始步骤ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Flow_Direct', @level2type = N'COLUMN', @level2name = N'StepId';

