CREATE TABLE [dbo].[Base_CatalogArticle] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [CatalogId]  INT           NOT NULL,
    [ArticleId]  INT           NOT NULL,
    [CreateTime] DATETIME2 (7) NOT NULL,
    [Ord]        INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_Article_CatalogArticle] FOREIGN KEY ([ArticleId]) REFERENCES [dbo].[Base_Article] ([Id]),
    CONSTRAINT [FK_Base_Catalog_CatalogArticle] FOREIGN KEY ([CatalogId]) REFERENCES [dbo].[Base_Catalog] ([Id]),
    UNIQUE NONCLUSTERED ([CatalogId] ASC, [ArticleId] ASC)
);

