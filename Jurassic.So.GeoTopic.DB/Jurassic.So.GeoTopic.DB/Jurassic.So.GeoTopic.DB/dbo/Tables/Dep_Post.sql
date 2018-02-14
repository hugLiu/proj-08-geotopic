CREATE TABLE [dbo].[Dep_Post] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [PostName]       NVARCHAR (50) NOT NULL,
    [PostType]       NVARCHAR (20) NOT NULL,
    [PostLevelType]  NVARCHAR (20) NULL,
    [PostEngageType] NVARCHAR (20) NULL,
    [OperatorID]     INT           NOT NULL,
    [CreateDatetime] DATETIME      CONSTRAINT [DF_Dep_Post_CreateDatetime] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]      INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

