CREATE TABLE [config].[MachineVariablesMap] (
    [environmentVariable_id] INT NOT NULL,
    [machine_id]             INT NOT NULL,
    CONSTRAINT [PK_MachineVariables] PRIMARY KEY CLUSTERED ([environmentVariable_id] ASC, [machine_id] ASC),
    CONSTRAINT [FK_MachineVariables_EnvironmentVariables] FOREIGN KEY ([environmentVariable_id]) REFERENCES [config].[EnvironmentVariables] ([id]),
    CONSTRAINT [FK_MachineVariables_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id])
);

