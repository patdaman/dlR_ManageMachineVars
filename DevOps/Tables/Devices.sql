CREATE TABLE [logging].[Devices] (
    [id]          INT           IDENTITY (100000, 1) NOT NULL,
    [device_name] VARCHAR (256) NOT NULL,
    [ip_address]  VARCHAR (256) NULL,
    [create_date] DATETIME      CONSTRAINT [DF_Devices_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      NULL,
    [active]      BIT           CONSTRAINT [DF_Devices_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_Devices] UNIQUE NONCLUSTERED ([device_name] ASC)
);



