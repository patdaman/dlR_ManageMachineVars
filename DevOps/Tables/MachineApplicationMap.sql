CREATE TABLE [config].[MachineApplicationMap] (
    [machine_id]     INT NOT NULL,
    [application_id] INT NOT NULL,
    CONSTRAINT [PK_MachineApplicationMap] PRIMARY KEY CLUSTERED ([machine_id] ASC, [application_id] ASC),
    CONSTRAINT [FK_MachineApplicationMap_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_MachineApplicationMap_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id])
);

