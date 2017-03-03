CREATE TABLE [config].[AppComponents] (
    [application_id] INT NOT NULL,
    [component_id]   INT NOT NULL,
    CONSTRAINT [PK_AppComponents] PRIMARY KEY CLUSTERED ([application_id] ASC, [component_id] ASC),
    CONSTRAINT [FK_AppComponents_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]),
    CONSTRAINT [FK_AppComponents_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id])
);

