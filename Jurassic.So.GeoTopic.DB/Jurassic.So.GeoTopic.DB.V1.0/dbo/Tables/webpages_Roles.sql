CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (256) NOT NULL,
    [Description] NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC),
    UNIQUE NONCLUSTERED ([RoleName] ASC)
);

