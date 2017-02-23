CREATE TABLE [config].[Enum_MachineUsageType] (
    [name]   VARCHAR (128) NOT NULL,
    [value]  VARCHAR (128) NOT NULL,
    [active] BIT           CONSTRAINT [DF_Enum_MachineUsageType_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UsageType_Enum] PRIMARY KEY CLUSTERED ([name] ASC),
    CONSTRAINT [IX_UsageType_Enum_usage] UNIQUE NONCLUSTERED ([name] ASC)
);

