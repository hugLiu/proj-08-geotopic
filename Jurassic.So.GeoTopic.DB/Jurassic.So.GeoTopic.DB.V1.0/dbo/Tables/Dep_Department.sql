CREATE TABLE [dbo].[Dep_Department] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ParentId]         INT            NULL,
    [DepHID]           NVARCHAR (50)  NULL,
    [Ord]              INT            NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Remark]           NVARCHAR (200) NULL,
    [DepType]          NVARCHAR (20)  NULL,
    [ExamineType]      NVARCHAR (20)  NULL,
    [IsActive]         INT            CONSTRAINT [DF_Dep_Department_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDisabled]       INT            CONSTRAINT [DF_Dep_Department_IsDisabled] DEFAULT ((0)) NOT NULL,
    [IsDeleted]        INT            CONSTRAINT [DF_Dep_Department_Deleted] DEFAULT ((0)) NOT NULL,
    [CreateDatetime]   DATETIME       CONSTRAINT [DF_Dep_Department_CreateDatetime] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime] DATETIME       CONSTRAINT [DF_Dep_Department_ModifiedDateTime] DEFAULT (getdate()) NOT NULL,
    [OrgNode]          NVARCHAR (200) NULL,
    CONSTRAINT [PK_Sys_Department] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部门编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dep_Department', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父部门编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dep_Department', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部门序号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dep_Department', @level2type = N'COLUMN', @level2name = N'Ord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部门名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dep_Department', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dep_Department', @level2type = N'COLUMN', @level2name = N'Remark';

