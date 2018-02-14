CREATE TABLE [dbo].[Base_Catalog] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ParentId]     INT            NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Abstract]     NVARCHAR (200) NULL,
    [Location]     VARCHAR (200)  NULL,
    [IconLocation] VARCHAR (200)  NULL,
    [Language]     VARCHAR (20)   NULL,
    [Ord]          INT            NOT NULL,
    [CreateTime]   DATETIME2 (7)  NULL,
    [EditTime]     DATETIME2 (7)  NULL,
    [EditorId]     INT            NULL,
    [OwnerId]      INT            DEFAULT ((0)) NOT NULL,
    [OwnerType]    INT            DEFAULT ((0)) NOT NULL,
    [State]        INT            NOT NULL,
    CONSTRAINT [PK_CT_Catalog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_Catalog_Base_Catalog] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Base_Catalog] ([Id]),
    UNIQUE NONCLUSTERED ([ParentId] ASC, [Name] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目录基本表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目录父ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'简介', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'Abstract';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'语言', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'Language';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'Ord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Catalog', @level2type = N'COLUMN', @level2name = N'State';

