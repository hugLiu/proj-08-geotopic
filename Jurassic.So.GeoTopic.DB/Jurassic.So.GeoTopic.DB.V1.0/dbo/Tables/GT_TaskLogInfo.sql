CREATE TABLE [dbo].[GT_TaskLogInfo] (
    [Id]                     INT              IDENTITY (1, 1) NOT NULL,
    [TaskLogInfoId]          UNIQUEIDENTIFIER NOT NULL,
    [SpiderScopeId]          INT              NULL,
    [AdapterId]              INT              NULL,
    [AdapterName]            NVARCHAR (50)    NULL,
    [DataSourceName]         NVARCHAR (100)   NULL,
    [SpiderScope]            NVARCHAR (255)   NULL,
    [StartDate]              DATETIME         NULL,
    [EndDate]                DATETIME         NULL,
    [Success]                BIT              NULL,
    [RecordCount]            INT              NULL,
    [FailureTime]            INT              NULL,
    [FailturReason]          TEXT             NULL,
    [IsInvalid]              BIT              CONSTRAINT [DF__ID_TaskLo__IsInv__6B99EBCE] DEFAULT ((0)) NULL,
    [OrderIndex]             BIGINT           NULL,
    [Remark]                 NVARCHAR (200)   NULL,
    [Performer]              NVARCHAR (50)    NULL,
    [AdapterTaskLogIdentity] NVARCHAR (255)   NULL,
    [Deleted]                BIT              CONSTRAINT [DF_ID_TaskLogInfo_Deleted] DEFAULT ((0)) NULL,
    [CreatedDate]            DATETIME         CONSTRAINT [DF__ID_TaskLo__SysCr__6AA5C795] DEFAULT (getdate()) NULL,
    [CreatedBy]              NVARCHAR (20)    NULL,
    [UpdatedDate]            DATETIME         NULL,
    [UpdatedBy]              NVARCHAR (20)    NULL,
    CONSTRAINT [PK_GT_TaskLogInfo_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '任务执行日志', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{任务日志编号}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'TaskLogInfoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{数据类型}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'SpiderScope';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{开始时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'StartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{结束时间}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{是否成功}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'Success';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{记录数量}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'RecordCount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{失败次数}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'FailureTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{失败原因}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'FailturReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{IsInvalid}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'IsInvalid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{OrderIndex}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'OrderIndex';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{Remark}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{SysCreateDate}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_TaskLogInfo', @level2type = N'COLUMN', @level2name = N'CreatedDate';

