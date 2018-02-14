CREATE TABLE [dbo].[Base_ArticleExt] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ArticleId]   INT            NOT NULL,
    [CatlogExtId] INT            NOT NULL,
    [Value]       NVARCHAR (200) NULL,
    CONSTRAINT [PK_Base_ArticleAttribute] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_ArticleExt_Base_Article] FOREIGN KEY ([ArticleId]) REFERENCES [dbo].[Base_Article] ([Id]),
    CONSTRAINT [FK_Base_ArticleExt_Base_CatlogExt] FOREIGN KEY ([CatlogExtId]) REFERENCES [dbo].[Base_CatalogExt] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'文章扩展表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleExt', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'文章ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleExt', @level2type = N'COLUMN', @level2name = N'ArticleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扩展项ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleExt', @level2type = N'COLUMN', @level2name = N'CatlogExtId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleExt', @level2type = N'COLUMN', @level2name = N'Value';

