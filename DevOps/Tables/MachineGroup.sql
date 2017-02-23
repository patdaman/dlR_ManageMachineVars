CREATE TABLE [config].[MachineGroup] (
    [id]          INT           IDENTITY (100000, 1) NOT NULL,
    [group_name]  VARCHAR (128) NOT NULL,
    [create_date] DATETIME      CONSTRAINT [DF_MachineGroup_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      NULL,
    CONSTRAINT [PK_MachineGroup] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_MachineGroup] UNIQUE NONCLUSTERED ([group_name] ASC)
);

