CREATE TABLE [monitor].[ApplicationInsightMachineMap] (
    [applicationId] INT NOT NULL,
    [machineId]     INT NOT NULL,
    CONSTRAINT [PK_ApplicationInsightMachineMap] PRIMARY KEY CLUSTERED ([applicationId] ASC, [machineId] ASC),
    CONSTRAINT [FK_ApplicationInsightMachineMap_Applications] FOREIGN KEY ([applicationId]) REFERENCES [monitor].[Applications] ([id]),
    CONSTRAINT [FK_ApplicationInsightMachineMap_Machines] FOREIGN KEY ([machineId]) REFERENCES [config].[Machines] ([id])
);

