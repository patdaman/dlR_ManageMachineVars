CREATE TABLE [config].[ApplicationEnvironmentVariables] (
    [application_id]         INT NOT NULL,
    [environmentvariable_id] INT NOT NULL,
    CONSTRAINT [PK_ApplicationEnvironmentVariables] PRIMARY KEY CLUSTERED ([application_id] ASC, [environmentvariable_id] ASC),
    CONSTRAINT [FK_ApplicationEnvironmentVariables_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]),
    CONSTRAINT [FK_ApplicationEnvironmentVariables_EnvironmentVariables] FOREIGN KEY ([environmentvariable_id]) REFERENCES [config].[EnvironmentVariables] ([id])
);

