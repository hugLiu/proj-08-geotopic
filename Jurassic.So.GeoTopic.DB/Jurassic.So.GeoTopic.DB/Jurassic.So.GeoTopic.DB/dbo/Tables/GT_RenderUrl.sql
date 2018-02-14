CREATE TABLE [dbo].[GT_RenderUrl] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [RenderTypeId]      INT            NOT NULL,
    [Url]         NVARCHAR (128)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
	[CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
	CONSTRAINT [U_RenderUrl] UNIQUE ([Url]),
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Url_Type] FOREIGN KEY ([RenderTypeId]) REFERENCES [dbo].[GT_RenderType] ([Id])
);

