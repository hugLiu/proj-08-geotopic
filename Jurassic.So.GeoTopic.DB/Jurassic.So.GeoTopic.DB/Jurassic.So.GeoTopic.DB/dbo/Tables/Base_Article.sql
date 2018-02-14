CREATE TABLE [dbo].[Base_Article] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (200) NOT NULL,
    [Keywords]   NVARCHAR (200) NULL,
    [Abstract]   NVARCHAR (200) NULL,
    [UrlTitle]   VARCHAR (50)   NULL,
    [Author]     NVARCHAR (50)  NULL,
    [EditorId]   INT            NOT NULL,
    [CreateTime] DATETIME2 (7)  NOT NULL,
    [EditTime]   DATETIME2 (7)  NOT NULL,
    [State]      INT            NOT NULL,
    [Clicks]     INT            CONSTRAINT [DF_Base_Article_Clicks] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CT_Article] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Base_Article_UserProfile] FOREIGN KEY ([EditorId]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'State';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'EditTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'CreateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'编辑者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'EditorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'Author';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'URL上的标题', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'UrlTitle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'摘要', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'Abstract';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'关键词', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'Keywords';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标题', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'文章基本表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Base_Article', @level2type = N'COLUMN', @level2name = N'Id';

