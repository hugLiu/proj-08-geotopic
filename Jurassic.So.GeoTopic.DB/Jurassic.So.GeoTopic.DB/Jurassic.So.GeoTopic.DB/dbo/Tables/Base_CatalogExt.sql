CREATE TABLE [dbo].[Base_CatalogExt] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CatalogId]      INT            NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [DataType]       INT            NOT NULL,
    [DefaultValue]   NVARCHAR (200) NULL,
    [DataSourceType] INT            NOT NULL,
    [DataSource]     NVARCHAR (MAX) NULL,
    [Ord]            INT            NOT NULL,
    [State]          INT            NOT NULL,
    [AllowNull]      BIT            NULL,
    [MaxLength]      INT            NULL,
    CONSTRAINT [PK_Base_CatlogExt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_CatlogExt_Base_Catalog] FOREIGN KEY ([CatalogId]) REFERENCES [dbo].[Base_Catalog] ([Id]),
    UNIQUE NONCLUSTERED ([CatalogId] ASC, [Name] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'字段最大长度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'MaxLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否可为空', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'AllowNull';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'State';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'Ord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'数据源类型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'DataSource';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'数据源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'DataSourceType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'默认值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'DefaultValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'数据类型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'DataType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'数据名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目录ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'CatalogId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目录扩展表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_CatalogExt', @level2type = N'COLUMN', @level2name = N'Id';

