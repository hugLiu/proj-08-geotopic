CREATE TABLE [dbo].[Dep_DepHistory] (
    [Id]    INT IDENTITY (1, 1) NOT NULL,
    [DepId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Sys_DepHistory_Sys_Department] FOREIGN KEY ([DepId]) REFERENCES [dbo].[Dep_Department] ([Id])
);

