CREATE TABLE [config].[MachineGroupMapping] (
    [group_id]   INT NOT NULL,
    [machine_id] INT NOT NULL,
    CONSTRAINT [PK_MachineGroupMapping] PRIMARY KEY CLUSTERED ([group_id] ASC, [machine_id] ASC),
    CONSTRAINT [FK_MachineGroupMapping_MachineGroup] FOREIGN KEY ([group_id]) REFERENCES [config].[MachineGroup] ([id]),
    CONSTRAINT [FK_MachineGroupMapping_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id]),
    CONSTRAINT [IX_Unique_MachineGroups] UNIQUE NONCLUSTERED ([group_id] ASC, [machine_id] ASC)
);

