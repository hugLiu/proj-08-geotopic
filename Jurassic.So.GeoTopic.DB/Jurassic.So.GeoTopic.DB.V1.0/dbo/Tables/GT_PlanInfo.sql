CREATE TABLE [dbo].[GT_PlanInfo] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [AdapterId]       INT            NULL,
    [PlanName]        NVARCHAR (255) NULL,
    [ValidDate]       DATETIME       NULL,
    [NextRunDate]     DATETIME       NULL,
    [PlanWillInvalid] BIT            NULL,
    [RunRule]         NVARCHAR (255) NULL,
    [DayRunTime]      NVARCHAR (255) NULL,
    [InvalidDate]     DATETIME       NULL,
    [InvalidTimes]    INT            NULL,
    [InvalidType]     INT            NULL,
    [MonthRunMonths]  NVARCHAR (255) NULL,
    [MonthRunDay]     NVARCHAR (255) NULL,
    [MonthRunTime]    NVARCHAR (255) NULL,
    [RunTimes]        INT            NULL,
    [SelfInterval]    INT            NULL,
    [SelfRunTime]     NVARCHAR (255) NULL,
    [WeekRunDays]     NVARCHAR (255) NULL,
    [WeekRunTime]     NVARCHAR (255) NULL,
    [Diabled]         BIT            NULL,
    [IsInvalid]       BIT            CONSTRAINT [DF__ID_PlanIn__IsInv__04659998] DEFAULT ((0)) NULL,
    [OrderIndex]      BIGINT         NULL,
    [Remark]          NVARCHAR (255) NULL,
    [CreatedDate]     DATETIME       NULL,
    [CreatedBy]       NVARCHAR (20)  NULL,
    [UpdatedDate]     DATETIME       NULL,
    [UpdatedBy]       NVARCHAR (20)  NULL,
    CONSTRAINT [PK_GT_PlanInfo_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GT_PlanInfo_GT_AdapterInfo] FOREIGN KEY ([AdapterId]) REFERENCES [dbo].[GT_AdapterInfo] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '计划信息表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{计划名称}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'PlanName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{生效日期}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'ValidDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{下次执行日期}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'NextRunDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{失效日期}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'PlanWillInvalid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{天执行时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'DayRunTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{过期时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'InvalidDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{过期次数}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'InvalidTimes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{过期类型}{}{表示过期的方式，分为按按次数过期和按日期过期，0表示按次数过期，1表示按照日期过期}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'InvalidType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{执行月份}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'MonthRunMonths';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{月执行天}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'MonthRunDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{月执行时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'MonthRunTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{已执行次数}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'RunTimes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{自定义执行间隔}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'SelfInterval';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{自定义执行时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'SelfRunTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{周执行天}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'WeekRunDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{周执行时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'WeekRunTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{IsInvalid}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'IsInvalid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{OrderIndex}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'OrderIndex';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{Remark}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{创建时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'CreatedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{创建者}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_PlanInfo', @level2type = N'COLUMN', @level2name = N'CreatedBy';

