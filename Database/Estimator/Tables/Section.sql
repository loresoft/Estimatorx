CREATE TABLE [dbo].[Section]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Section_Id] DEFAULT (newid()),
    
    [Name] NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,

    [ProjectId] UNIQUEIDENTIFIER NOT NULL,

    -- computed
    [TotalTasks] INT NULL, 
    [TotalHours] INT NULL, 
    [TotalWeeks] DECIMAL(10, 4) NULL, 

    [IsActive] BIT NOT NULL CONSTRAINT [DF_Section_IsActive] DEFAULT (1),

    [SysCreateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Section_SysCreateDate] DEFAULT (sysdatetime()),
    [SysCreateUser] NVARCHAR(100) NULL,
    [SysUpdateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Section_SysUpdateDate] DEFAULT (sysdatetime()),
    [SysUpdateUser] NVARCHAR(100) NULL,
    [SysVersion] ROWVERSION NOT NULL,

    
    CONSTRAINT [PK_Section] PRIMARY KEY NONCLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Section_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]), 
)
GO