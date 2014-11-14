CREATE TABLE [dbo].[Factor]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Factor_Id] DEFAULT (newid()),
    
    [Name] NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,

    [VerySimple] TINYINT NOT NULL,
    [Simple] TINYINT NOT NULL,
    [Medium] TINYINT NOT NULL,
    [Complex] TINYINT NOT NULL,
    [VeryComplex] TINYINT NOT NULL,
    
    [ProjectId] UNIQUEIDENTIFIER NULL,
    [TemplateId] UNIQUEIDENTIFIER NULL,

    [IsActive] BIT NOT NULL CONSTRAINT [DF_Factor_IsActive] DEFAULT (1),

    [SysCreateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Factor_SysCreateDate] DEFAULT (sysdatetime()),
    [SysCreateUser] NVARCHAR(100) NULL,
    [SysUpdateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Factor_SysUpdateDate] DEFAULT (sysdatetime()),
    [SysUpdateUser] NVARCHAR(100) NULL,
    [SysVersion] ROWVERSION NOT NULL,

    CONSTRAINT [PK_Factor] PRIMARY KEY NONCLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Factor_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]), 
    CONSTRAINT [FK_Factor_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([Id]), 
)
GO

CREATE INDEX [IX_Factor_ProjectId] 
    ON [dbo].[Factor] ([ProjectId])
GO

CREATE INDEX [IX_Factor_TemplateId] 
    ON [dbo].[Factor] ([TemplateId])
GO
