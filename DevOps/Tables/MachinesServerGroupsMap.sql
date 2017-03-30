CREATE TABLE [config].[MachinesServerGroupsMap] (
    [machine_id]     INT NOT NULL,
    [servergroup_id] INT NOT NULL,
    CONSTRAINT [PK_MachinesServerGroups] PRIMARY KEY CLUSTERED ([machine_id] ASC, [servergroup_id] ASC),
    CONSTRAINT [FK_MachinesServerGroups_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id]),
    CONSTRAINT [FK_MachinesServerGroups_ServerGroups] FOREIGN KEY ([servergroup_id]) REFERENCES [config].[ServerGroups] ([id])
);

