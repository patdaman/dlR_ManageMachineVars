CREATE TABLE [config].[MachineComponentPathMap] (
    [machine_id]   INT           NOT NULL,
    [component_id] INT           NOT NULL,
    [config_path]  VARCHAR (256) CONSTRAINT [DF_MachineComponentPath_root_path] DEFAULT ('D:\US\printableConfig\') NOT NULL,
    CONSTRAINT [PK_MachineComponentPathMap] PRIMARY KEY CLUSTERED ([machine_id] ASC, [component_id] ASC, [config_path] ASC),
    CONSTRAINT [FK_MachineComponentPath_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id]),
    CONSTRAINT [FK_MachineComponentPath_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id])
);

