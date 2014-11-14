CREATE TABLE [dbo].[Project]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Project_Id] DEFAULT (newid()),
    
    [Name] NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,
    
    [HoursPerWeek] INT NOT NULL, 
    [ContingencyRate] DECIMAL(10, 4) NOT NULL,

    -- computed
    [TotalTasks] INT NULL, 
    [TotalHours] INT NULL, 
    [TotalWeeks] DECIMAL(10, 4) NULL, 

    -- computed
    [ContingencyHours] INT NULL, 
    [ContingencyWeeks] DECIMAL(10, 4) NULL, 

    [IsActive] BIT NOT NULL CONSTRAINT [DF_Project_IsActive] DEFAULT (1),

    [SysCreateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Project_SysCreateDate] DEFAULT (sysdatetime()),
    [SysCreateUser] NVARCHAR(100) NULL,
    [SysUpdateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Project_SysUpdateDate] DEFAULT (sysdatetime()),
    [SysUpdateUser] NVARCHAR(100) NULL,
    [SysVersion] ROWVERSION NOT NULL,

    CONSTRAINT [PK_Project] PRIMARY KEY NONCLUSTERED ([Id] ASC), 
)
GO