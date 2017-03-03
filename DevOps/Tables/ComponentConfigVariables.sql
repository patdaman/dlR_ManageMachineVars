CREATE TABLE [config].[ComponentConfigVariables] (
    [component_id]      INT NOT NULL,
    [configvariable_id] INT NOT NULL,
    CONSTRAINT [PK_ApplicationConfigVariables] PRIMARY KEY CLUSTERED ([component_id] ASC, [configvariable_id] ASC),
    CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables] FOREIGN KEY ([configvariable_id]) REFERENCES [config].[ConfigVariables] ([id]),
    CONSTRAINT [FK_ComponentConfigVariables_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id])
);

