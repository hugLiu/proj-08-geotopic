CREATE TABLE [dbo].[Dep_PostHistory] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DepPostId]      INT            NOT NULL,
    [ChangeType]     NVARCHAR (20)  NOT NULL,
    [ChangeExplain]  NVARCHAR (128) NULL,
    [DepId]          INT            NOT NULL,
    [PostId]         INT            NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [ExamineType]    NVARCHAR (20)  NOT NULL,
    [IsActive]       INT            DEFAULT ((1)) NOT NULL,
    [IsDisabled]     INT            DEFAULT ((0)) NOT NULL,
    [IsDeleted]      INT            DEFAULT ((0)) NOT NULL,
    [CreateDatetime] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

