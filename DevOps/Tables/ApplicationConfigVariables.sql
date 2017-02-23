CREATE TABLE [config].[ApplicationConfigVariables] (
    [application_id]    INT NOT NULL,
    [configvariable_id] INT NOT NULL,
    CONSTRAINT [PK_ApplicationConfigVariables] PRIMARY KEY CLUSTERED ([application_id] ASC, [configvariable_id] ASC),
    CONSTRAINT [FK_ApplicationConfigVariables_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]),
    CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables] FOREIGN KEY ([configvariable_id]) REFERENCES [config].[ConfigVariables] ([id])
);

