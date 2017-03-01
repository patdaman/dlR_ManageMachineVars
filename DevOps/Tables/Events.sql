CREATE TABLE [logging].[Events] (
    [id]        INT             IDENTITY (100000, 1) NOT NULL,
    [device_id] INT             NULL,
    [Hostname]  VARCHAR (256)   NOT NULL,
    [Date]      DATE            CONSTRAINT [DF_Events_Date] DEFAULT (getdate()) NOT NULL,
    [Time]      TIME (7)        CONSTRAINT [DF_Events_Time] DEFAULT (getdate()) NOT NULL,
    [Priority]  VARCHAR (128)   NULL,
    [Message]   NVARCHAR (1024) NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Events_Devices] FOREIGN KEY ([device_id]) REFERENCES [logging].[Devices] ([id])
);

