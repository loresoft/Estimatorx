CREATE TABLE [dbo].[Estimate]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Estimate_Id] DEFAULT (newid()),
    
    [Name] NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,

    [VerySimple] TINYINT NOT NULL,
    [Simple] TINYINT NOT NULL,
    [Medium] TINYINT NOT NULL,
    [Complex] TINYINT NOT NULL,
    [VeryComplex] TINYINT NOT NULL,

    -- computed
    [TotalTasks] INT NULL,
    [TotalHours] INT NULL,

    [ProjectId] UNIQUEIDENTIFIER NOT NULL,
    [SectionId] UNIQUEIDENTIFIER NOT NULL,
    [FactorId] UNIQUEIDENTIFIER NOT NULL,

    [IsActive] BIT NOT NULL CONSTRAINT [DF_Estimate_IsActive] DEFAULT (1),

    [SysCreateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Estimate_SysCreateDate] DEFAULT (sysdatetime()),
    [SysCreateUser] NVARCHAR(100) NULL,
    [SysUpdateDate] DATETIME2 NOT NULL CONSTRAINT [DF_Estimate_SysUpdateDate] DEFAULT (sysdatetime()),
    [SysUpdateUser] NVARCHAR(100) NULL,
    [SysVersion] ROWVERSION NOT NULL,

    CONSTRAINT [PK_Estimate] PRIMARY KEY NONCLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Estimate_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]), 
    CONSTRAINT [FK_Estimate_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Section]([Id]), 
    CONSTRAINT [FK_Estimate_FactorId] FOREIGN KEY ([FactorId]) REFERENCES [Factor]([Id]), 
)
GO



CREATE CLUSTERED INDEX [IX_Estimate_ProjectId] 
    ON [dbo].[Estimate] ([ProjectId])
GO

CREATE INDEX [IX_Estimate_SectionId] 
    ON [dbo].[Estimate] ([SectionId])
GO
