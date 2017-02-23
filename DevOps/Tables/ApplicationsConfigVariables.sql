CREATE TABLE [config].[ApplicationsConfigVariables] (
    [application_id]    INT NOT NULL,
    [configvariable_id] INT NOT NULL,
    CONSTRAINT [PK_ApplicationsConfigVariables] PRIMARY KEY CLUSTERED ([application_id] ASC, [configvariable_id] ASC),
    CONSTRAINT [FK_ApplicationsConfigVariables_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]),
    CONSTRAINT [FK_ApplicationsConfigVariables_ConfigVariables] FOREIGN KEY ([configvariable_id]) REFERENCES [config].[ConfigVariables] ([id])
);

