CREATE TABLE [config].[MachineApplications] (
    [application_id] INT NOT NULL,
    [machine_id]     INT NOT NULL,
    CONSTRAINT [PK_MachineConfigVariables] PRIMARY KEY CLUSTERED ([application_id] ASC, [machine_id] ASC),
    CONSTRAINT [FK_MachineApplications_Applications] FOREIGN KEY ([application_id]) REFERENCES [config].[Applications] ([id]),
    CONSTRAINT [FK_MachineConfigVariables_Machines] FOREIGN KEY ([machine_id]) REFERENCES [config].[Machines] ([id])
);

