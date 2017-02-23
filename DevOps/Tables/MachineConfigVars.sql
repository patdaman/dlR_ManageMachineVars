CREATE TABLE [config].[MachineConfigVars] (
    [appConfigVariable_id] INT NOT NULL,
    [machine_id]           INT NOT NULL,
    CONSTRAINT [PK_MachineConfigVars] PRIMARY KEY CLUSTERED ([appConfigVariable_id] ASC, [machine_id] ASC),
    CONSTRAINT [FK_MachineConfigVars_ConfigVariables] FOREIGN KEY ([appConfigVariable_id]) REFERENCES [config].[ConfigVariables] ([id]),
    CONSTRAINT [FK_MachineConfigVars_MachineConfigVars] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id])
);

