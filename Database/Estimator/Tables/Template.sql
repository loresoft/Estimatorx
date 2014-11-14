CREATE TABLE [dbo].[Template]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Template_Id] DEFAULT (newid()),
    
    [Name] NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,

    [IsActive] BIT NOT NULL CONSTRAINT [DF_Template_IsActive] DEFAULT (1),

    [SysCreateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Template_SysCreateDate] DEFAULT (sysdatetime()),
    [SysCreateUser] NVARCHAR(100) NULL,
    [SysUpdateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Template_SysUpdateDate] DEFAULT (sysdatetime()),
    [SysUpdateUser] NVARCHAR(100) NULL,
    [SysVersion] ROWVERSION NOT NULL,

    CONSTRAINT [PK_Template] PRIMARY KEY NONCLUSTERED ([Id] ASC), 
)
GO
