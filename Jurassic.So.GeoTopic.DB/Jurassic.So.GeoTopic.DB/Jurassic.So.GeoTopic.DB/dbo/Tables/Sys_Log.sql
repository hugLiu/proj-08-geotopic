CREATE TABLE [dbo].[Sys_Log] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ModuleName]     NVARCHAR (50)   NOT NULL,
    [ActionName]     NVARCHAR (50)   NOT NULL,
    [ClientIP]       VARCHAR (20)    NULL,
    [UserName]       VARCHAR (50)    NULL,
    [OpTime]         DATETIME2 (7)   NULL,
    [CatalogId]      INT             NULL,
    [ObjectId]       INT             NULL,
    [LogType]        VARCHAR (20)    NULL,
    [Request]        VARCHAR (2000)  NULL,
    [Costs]          FLOAT (53)      NULL,
    [Message]        NVARCHAR (2000) NULL,
    [Browser]        VARCHAR (50)    NULL,
    [BrowserVersion] NUMERIC (18, 4) NULL,
    [Platform]       VARCHAR (50)    NULL,
    CONSTRAINT [PK_Sys_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
);

