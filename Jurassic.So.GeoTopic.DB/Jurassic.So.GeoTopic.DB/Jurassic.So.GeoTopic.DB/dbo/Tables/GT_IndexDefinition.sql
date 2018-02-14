CREATE TABLE [dbo].[GT_IndexDefinition] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
	[Code]       NVARCHAR (100) NOT NULL,
    [Title]       NVARCHAR (128) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL DEFAULT getdate(),
	[CreatedBy] NVARCHAR(50)      NULL,
	[UpdatedDate] DATETIME      NOT NULL DEFAULT getdate(),
    [UpdatedBy]    NVARCHAR (50) NULL,
    [IsDelete]   BIT           DEFAULT ((0)) NOT NULL,
	CONSTRAINT [U_IndexDefinition_Code] UNIQUE ([Code]),
	CONSTRAINT [U_IndexDefinition_Title] UNIQUE ([Title]),
    CONSTRAINT [PK_GT_IndexDefinition] PRIMARY KEY CLUSTERED ([Id] ASC)
);

