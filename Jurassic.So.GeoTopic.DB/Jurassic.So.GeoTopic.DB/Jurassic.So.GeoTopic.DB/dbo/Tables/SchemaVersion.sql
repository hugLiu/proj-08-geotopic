CREATE TABLE [dbo].[SchemaVersion] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Version]    NVARCHAR (50)  NULL,
    [UpdateTime] DATETIME       NULL,
    [Remark]     NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

