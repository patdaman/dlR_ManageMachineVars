CREATE TABLE [config].[ComponentConfigVariablesMap] (
    [component_id]      INT NOT NULL,
    [configvariable_id] INT NOT NULL,
    CONSTRAINT [PK_ComponentConfigVariables] PRIMARY KEY CLUSTERED ([component_id] ASC, [configvariable_id] ASC),
    CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables] FOREIGN KEY ([configvariable_id]) REFERENCES [config].[ConfigVariables] ([id]),
    CONSTRAINT [FK_ComponentConfigVariables_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id])
);

