CREATE TABLE [dbo].[Base_ArticleRelation] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [SourceId]     INT NOT NULL,
    [TargetId]     INT NOT NULL,
    [RelationType] INT NOT NULL,
    [Ord]          INT NOT NULL,
    CONSTRAINT [PK_Base_ArticleRelation] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'源ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleRelation', @level2type = N'COLUMN', @level2name = N'SourceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目标ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleRelation', @level2type = N'COLUMN', @level2name = N'TargetId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_ArticleRelation', @level2type = N'COLUMN', @level2name = N'Ord';

