CREATE TABLE [config].[EnvironmentVariables] (
    [id]          INT           IDENTITY (100000, 1) NOT NULL,
    [key]         VARCHAR (256) NOT NULL,
    [value]       VARCHAR (256) NOT NULL,
    [type]        VARCHAR (128) CONSTRAINT [DF_EnvironmentVariables_type] DEFAULT (user_name()) NOT NULL,
    [path]        VARCHAR (256) NULL,
    [create_date] DATETIME      CONSTRAINT [DF_EnvironmentVariables_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      NULL,
    [active]      BIT           CONSTRAINT [DF_EnvironmentVariables_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_EnvironmentVariables] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_EnvironmentVariables_Enum_EnvironmentVariableType] FOREIGN KEY ([type]) REFERENCES [config].[Enum_EnvironmentVariableType] ([name]),
    CONSTRAINT [IX_EnvironmentVariables_key_type] UNIQUE NONCLUSTERED ([key] ASC, [type] ASC)
);

