CREATE TABLE [dbo].[UserProfile] (
    [UserId]      INT           IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (56) NOT NULL,
    [Email]       VARCHAR (200) NULL,
    [PhoneNumber] VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

