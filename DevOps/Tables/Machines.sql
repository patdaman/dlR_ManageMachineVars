CREATE TABLE [config].[Machines] (
    [id]           INT           IDENTITY (100000, 1) NOT NULL,
    [machine_name] VARCHAR (128) NOT NULL,
    [location]     VARCHAR (128) NOT NULL,
    [usage]        VARCHAR (128) NOT NULL,
    [create_date]  DATETIME      CONSTRAINT [DF_Machines_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]  DATETIME      NULL,
    [active]       BIT           CONSTRAINT [DF_Machines_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Machines] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Machines_Enum_Locations] FOREIGN KEY ([location]) REFERENCES [config].[Enum_Locations] ([name]),
    CONSTRAINT [IX_Machines_MachineName] UNIQUE NONCLUSTERED ([machine_name] ASC)
);



