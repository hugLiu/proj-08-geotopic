CREATE TABLE [dbo].[GT_UrlTemplate] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [RenderUrlId] INT            NOT NULL,
    [Defintion]   NVARCHAR (MAX) NOT NULL,
    [Type]        NVARCHAR (128) NOT NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (50)  NULL,
    [UpdatedDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   NVARCHAR (50)  NULL,
    [IsDelete]    BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GT_UrlTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Temp_URL] FOREIGN KEY ([RenderUrlId]) REFERENCES [dbo].[GT_RenderUrl] ([Id])
);



