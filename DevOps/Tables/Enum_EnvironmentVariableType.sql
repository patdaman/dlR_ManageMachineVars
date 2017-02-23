CREATE TABLE [config].[Enum_EnvironmentVariableType] (
    [name]   VARCHAR (128) NOT NULL,
    [value]  VARCHAR (128) NOT NULL,
    [active] BIT           CONSTRAINT [DF_Enum_EnvironmentVariableType_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Enum_EnvironmentVariableType] PRIMARY KEY CLUSTERED ([name] ASC)
);

