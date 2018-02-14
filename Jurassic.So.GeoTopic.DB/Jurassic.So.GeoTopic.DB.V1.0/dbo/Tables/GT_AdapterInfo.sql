CREATE TABLE [dbo].[GT_AdapterInfo] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [AdapterName]    NVARCHAR (50)   NOT NULL,
    [SDomain]        CHAR (8)        CONSTRAINT [DF__ds_Adapte__SDoma__6C190EBB] DEFAULT ('Archive') NULL,
    [DataSourceName] NVARCHAR (100)  NULL,
    [DataSourceType] NVARCHAR (50)   NULL,
    [AdapterURL]     NVARCHAR (1024) NULL,
    [Status]         INT             NULL,
    [InvokeType]     INT             NOT NULL,
    [Hangup]         INT             NULL,
    [Desc]           NVARCHAR (1024) NULL,
    [IsInvalid]      BIT             CONSTRAINT [DF__ds_Adapte__IsInv__6EF57B66] DEFAULT ((0)) NULL,
    [OrderIndex]     BIGINT          NULL,
    [Remark]         NVARCHAR (200)  NULL,
    [ClassName]      NVARCHAR (100)  NULL,
    [ConfigAddress]  NVARCHAR (1000) NULL,
    [AdapterType]    NVARCHAR (100)  NULL,
    [SpiderSize]     INT             CONSTRAINT [DF_ds_AdapterInfo_SpiderSize] DEFAULT ((20)) NULL,
    [CreatedDate]    DATETIME        CONSTRAINT [DF__ds_Adapte__SysCr__6E01572D] DEFAULT (getdate()) NULL,
    [CreatedBy]      NVARCHAR (20)   NULL,
    [UpdatedDate]    DATETIME        NULL,
    [UpdatedBy]      NVARCHAR (20)   NULL,
    CONSTRAINT [PK_GT_AdapterInfo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CKC_SDOMAIN_DS_ADAPT] CHECK ([SDomain] IS NULL OR ([SDomain]='3GX' OR [SDomain]='Archive'))
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ds_AdapterInfo_AdapterName]
    ON [dbo].[GT_AdapterInfo]([AdapterName] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '适配器注册管理表。每条记录代表一个接入的数据源。', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{适配器名称}{}{适配器名称，来自Adapter的AdapterInfo。允许编辑}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'AdapterName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{应用域}{}{适配器的应用域，来自Adapter的AdapterInfo。目前为Archive或3GX，不允许编辑}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'SDomain';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{数据源名称}{}{适配器的所接的数据存储的业务名称，来自Adapter的AdapterInfo。不允许编辑}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'DataSourceName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{数据源类型}{}{适配器所接的数据存储的存储系统名称，如：Oracle、SQL Server、GeoFrame、GeoBank、3GXService等，来自Adapter的AdapterInfo。不允许编辑}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'DataSourceType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{服务地址}{}{适配器的服务地址及相关参数}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'AdapterURL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{工作状态}{}{适配器当前的工作状态，1表示正常，0表示异常}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{是否挂起}{}{表示适配器当前被挂起，0表示挂起，1表示工作，无论正常或异常均可挂起}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'Hangup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{IsInvalid}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'IsInvalid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{OrderIndex}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'OrderIndex';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{Remark}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一次爬取的Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'SpiderSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '{SysCreateDate}{}{}', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GT_AdapterInfo', @level2type = N'COLUMN', @level2name = N'CreatedDate';

