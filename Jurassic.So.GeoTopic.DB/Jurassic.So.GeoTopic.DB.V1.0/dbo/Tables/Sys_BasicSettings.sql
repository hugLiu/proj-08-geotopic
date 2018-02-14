CREATE TABLE [dbo].[Sys_BasicSettings] (
    [Id]                      INT            IDENTITY (1, 1) NOT NULL,
    [Pid]                     INT            NOT NULL,
    [Scope]                   NVARCHAR (50)  NULL,
    [PreferencesItemName]     NVARCHAR (50)  NOT NULL,
    [Sequence]                INT            NOT NULL,
    [Value1]                  NVARCHAR (128) NOT NULL,
    [Value2]                  NVARCHAR (128) NULL,
    [Description]             NVARCHAR (512) NULL,
    [EffectivedStartDateTime] DATETIME       NULL,
    [EffectivedEndDateTime]   DATETIME       NULL,
    [CreateDatetime]          DATETIME       NOT NULL,
    [ModifiedDateTime]        DATETIME       CONSTRAINT [DF__Sys_Basic__Modif__160F4887] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Sys_Basi__3214EC070F624AF8] PRIMARY KEY CLUSTERED ([Id] ASC)
);

