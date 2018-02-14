CREATE TABLE [dbo].[Base_ArticleText] (
    [Id]   INT            NOT NULL,
    [TEXT] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Base_ArticleText] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_ArticleText_Base_Article] FOREIGN KEY ([Id]) REFERENCES [dbo].[Base_Article] ([Id])
);

