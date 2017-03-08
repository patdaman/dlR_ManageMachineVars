CREATE TABLE [config].[ConfigVariableValue] (
    [id]               INT             IDENTITY (100000, 1) NOT NULL,
    [configvar_id]     INT             NOT NULL,
    [environment_type] VARCHAR (128)   NOT NULL,
    [machine_id]       INT             NULL,
    [value]            NVARCHAR (4000) NULL,
    [create_date]      DATETIME        CONSTRAINT [DF_ConfigVariableValue_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME        NULL,
    CONSTRAINT [PK_ConfigVariableValue] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigVariableValue_ConfigVariables] FOREIGN KEY ([configvar_id]) REFERENCES [config].[ConfigVariables] ([id]),
    CONSTRAINT [FK_ConfigVariableValue_Enum_EnvironmentType] FOREIGN KEY ([environment_type]) REFERENCES [config].[Enum_EnvironmentType] ([name]),
    CONSTRAINT [FK_ConfigVariableValue_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id]),
    CONSTRAINT [IX_ConfigVariableValue] UNIQUE NONCLUSTERED ([configvar_id] ASC, [environment_type] ASC, [machine_id] ASC)
);











