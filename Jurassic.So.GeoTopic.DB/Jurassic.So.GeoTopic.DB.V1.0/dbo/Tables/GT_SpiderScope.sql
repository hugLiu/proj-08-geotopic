CREATE TABLE [dbo].[GT_SpiderScope] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [AdapterId]        INT            NOT NULL,
    [SpiderScope]      NVARCHAR (255) NOT NULL,
    [Priority]         INT            NULL,
    [Desc]             VARCHAR (1024) NULL,
    [IsInvalid]        BIT            CONSTRAINT [DF__ds_Adapte__IsInv__68487DD7] DEFAULT ((0)) NULL,
    [OrderIndex]       BIGINT         NULL,
    [Remark]           NVARCHAR (200) NULL,
    [IncrementType]    NVARCHAR (50)  NULL,
    [AsTask]           BIT            NULL,
    [LastSpiderStatus] INT            CONSTRAINT [DF_GT_SpiderScope_LastSpiderStatus] DEFAULT ((0)) NULL,
    [TaskMode]         INT            NULL,
    [IncrementValue]   NVARCHAR (50)  NULL,
    [CreatedDate]      DATETIME       CONSTRAINT [DF__ds_Adapte__SysCr__6754599E] DEFAULT (getdate()) NULL,
    [CreatedBy]        NVARCHAR (20)  NULL,
    [UpdatedDate]      DATETIME       NULL,
    [UpdatedBy]        NVARCHAR (20)  NULL,
    [ExecuteTimes]     INT            CONSTRAINT [DF_GT_SpiderScope_ExecuteTimes] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_GT_SpiderScope] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GT_SpiderScope_GT_AdapterInfo] FOREIGN KEY ([AdapterId]) REFERENCES [dbo].[GT_AdapterInfo] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GT_AdapterDataType]
    ON [dbo].[GT_SpiderScope]([AdapterId] ASC, [SpiderScope] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '适配器支持的数据类型或者成果类型注册表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{数据类型}{}{适配器支持的数据类型名称或者成果类型}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'SpiderScope';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{适配器优先级}{}{支持某个数据类型的每个适配器的优先顺序}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'Priority';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{IsInvalid}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'IsInvalid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{OrderIndex}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'OrderIndex';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{Remark}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{SysCreateDate}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_SpiderScope', @level2type = N'COLUMN', @level2name = N'CreatedDate';

