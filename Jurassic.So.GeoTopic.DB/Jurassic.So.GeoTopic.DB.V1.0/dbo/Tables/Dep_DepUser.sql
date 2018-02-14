CREATE TABLE [dbo].[Dep_DepUser] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [DepId]          INT           NOT NULL,
    [UserId]         INT           NOT NULL,
    [UserName]       NVARCHAR (10) NOT NULL,
    [ExamineType]    NVARCHAR (20) NOT NULL,
    [ContractType]   NVARCHAR (20) NULL,
    [ContractLenght] INT           NULL,
    [JoinDateTime]   DATE          NOT NULL,
    [OutDateTime]    DATE          NULL,
    [CreateDatetime] DATETIME      CONSTRAINT [DF_Dep_DepUser_CreateDatetime] DEFAULT (getdate()) NOT NULL,
    [IsSuspension]   INT           CONSTRAINT [DF_Dep_DepUser_IsSuspension] DEFAULT ((0)) NOT NULL,
    [IsDeleted]      INT           CONSTRAINT [DF_Dep_DepUser_IsDeleted] DEFAULT ((0)) NOT NULL,
    [PostId]         INT           NULL,
    CONSTRAINT [PK_Dep_DepUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Dep_DepUser_Dep_Department] FOREIGN KEY ([DepId]) REFERENCES [dbo].[Dep_Department] ([Id]),
    CONSTRAINT [FK_Dep_DepUser_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

