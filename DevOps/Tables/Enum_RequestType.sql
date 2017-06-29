CREATE TABLE [monitor].[Enum_RequestType] (
    [name]   VARCHAR (128) NOT NULL,
    [value]  VARCHAR (128) NOT NULL,
    [active] BIT           CONSTRAINT [DF_Enum_RequestType_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Enum_RequestType_1] PRIMARY KEY CLUSTERED ([name] ASC)
);

