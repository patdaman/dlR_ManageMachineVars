CREATE TABLE [config].[ConfigVariableValue] (
    [configvar_id]     INT             NOT NULL,
    [environment_type] VARCHAR (128)   NOT NULL,
    [value]            NVARCHAR (4000) NULL,
    CONSTRAINT [PK_ConfigVariableValue] PRIMARY KEY CLUSTERED ([configvar_id] ASC, [environment_type] ASC),
    CONSTRAINT [FK_ConfigVariableValue_ConfigVariables] FOREIGN KEY ([configvar_id]) REFERENCES [config].[ConfigVariables] ([id]),
    CONSTRAINT [FK_ConfigVariableValue_Enum_EnvironmentType] FOREIGN KEY ([environment_type]) REFERENCES [config].[Enum_EnvironmentType] ([name])
);





