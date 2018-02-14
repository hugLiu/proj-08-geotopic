CREATE TABLE [dbo].[Dep_DepPost] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DepId]          INT            NOT NULL,
    [PostId]         INT            NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [PlanNumber]     INT            NULL,
    [ExamineType]    NVARCHAR (20)  NULL,
    [Describe]       NVARCHAR (512) NULL,
    [Duty]           NVARCHAR (512) NULL,
    [Requirement]    NVARCHAR (512) NULL,
    [IsActive]       INT            CONSTRAINT [DF_Dep_DepPost_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDisabled]     INT            CONSTRAINT [DF__Sys_DepPo__IsDis__395884C4] DEFAULT ((0)) NOT NULL,
    [IsDeleted]      INT            CONSTRAINT [DF__Sys_DepPo__IsDel__3A4CA8FD] DEFAULT ((0)) NOT NULL,
    [CreateDatetime] DATETIME       CONSTRAINT [DF_Dep_DepPost_CreateDatetime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__tmp_ms_x__3214EC07282DF8C2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Dep_DepPost_Dep_Department] FOREIGN KEY ([DepId]) REFERENCES [dbo].[Dep_Department] ([Id]),
    CONSTRAINT [FK_Dep_DepPost_Dep_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Dep_Post] ([Id])
);

