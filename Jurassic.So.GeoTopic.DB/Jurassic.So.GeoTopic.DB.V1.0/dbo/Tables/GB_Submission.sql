CREATE TABLE [dbo].[GB_Submission] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [NatureKey]      UNIQUEIDENTIFIER NOT NULL,
    [Authentic]      BIT              NULL,
    [AutoComplement] BIT              NULL,
    [UploadedBy]     NVARCHAR (100)   NULL,
    [UploadedDate]   DATETIME         NULL,
    [Contact]        NVARCHAR (500)   NULL,
    [Application]    NVARCHAR (500)   NULL,
    [Task]           NVARCHAR (500)   NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [CreatedBy]      NVARCHAR (100)   NULL,
    [UpdatedDate]    DATETIME         NOT NULL,
    [UpdatedBy]      NVARCHAR (100)   NULL,
    [IsDelete]       BIT              NOT NULL,
    [Tag1]           NVARCHAR (500)   NULL,
    [Tag2]           NVARCHAR (500)   NULL,
    [Tag3]           NVARCHAR (500)   NULL,
    [Tag4]           NVARCHAR (500)   NULL,
    [Tag5]           NVARCHAR (500)   NULL,
    [Tag6]           NVARCHAR (500)   NULL,
    [Tag7]           NVARCHAR (500)   NULL,
    [Tag8]           NVARCHAR (500)   NULL,
    [Tag9]           NVARCHAR (500)   NULL,
    [Tag10]          NVARCHAR (500)   NULL,
    [Tag11]          NVARCHAR (500)   NULL,
    [Tag12]          NVARCHAR (500)   NULL,
    [Tag13]          NVARCHAR (500)   NULL,
    [Tag14]          NVARCHAR (500)   NULL,
    [Tag15]          NVARCHAR (500)   NULL,
    [Tag16]          NVARCHAR (500)   NULL,
    [Tag17]          NVARCHAR (500)   NULL,
    [Tag18]          NVARCHAR (500)   NULL,
    [Tag19]          NVARCHAR (500)   NULL,
    [Tag20]          NVARCHAR (500)   NULL,
    [Tag21]          DATETIME         NULL,
    [Tag22]          DATETIME         NULL,
    [Tag23]          DATETIME         NULL,
    [Tag24]          DATETIME         NULL,
    [Tag25]          DATETIME         NULL,
    [Tag26]          DATETIME         NULL,
    [Tag27]          DATETIME         NULL,
    [Tag28]          DATETIME         NULL,
    [Tag29]          DATETIME         NULL,
    [Tag30]          DATETIME         NULL,
    [Tag31]          NVARCHAR (500)   NULL,
    [Tag32]          NVARCHAR (500)   NULL,
    CONSTRAINT [PK_GB_Submission] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否为审核状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'Authentic';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否需要调用元数据自动补全功能', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'AutoComplement';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上传人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'UploadedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上传时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'UploadedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'联系方式 可以提供多个联系方式，使用“|”分割。如”email:z3@a.com | qq:12345 | wc:3214 | tel: 18601234567”', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'Contact';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交源系统名称。（从哪个系统提交的，如油田规划应用）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'Application';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交的任务名称。（做什么任务的时候提交的，如资源建设、评价总结）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GB_Submission', @level2type = N'COLUMN', @level2name = N'Task';

