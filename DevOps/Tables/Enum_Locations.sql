CREATE TABLE [config].[Enum_Locations] (
    [name]   VARCHAR (128) NOT NULL,
    [value]  VARCHAR (128) NOT NULL,
    [active] BIT           CONSTRAINT [DF_Enum_Locations_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Enum_Locations] PRIMARY KEY CLUSTERED ([name] ASC)
);

