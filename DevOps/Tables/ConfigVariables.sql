CREATE TABLE [config].[ConfigVariables] (
    [id]             INT            IDENTITY (100000, 1) NOT NULL,
    [parent_element] VARCHAR (256)  NULL,
    [element]        VARCHAR (256)  NULL,
    [attribute]      VARCHAR (256)  NOT NULL,
    [key]            VARCHAR (256)  NOT NULL,
    [value_name]     VARCHAR (256)  NOT NULL,
    [value]          VARCHAR (4000) NOT NULL,
    [config_path]    VARCHAR (256)  NOT NULL,
    [create_date]    DATETIME       CONSTRAINT [DF_ConfigVariables_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME       NULL,
    [active]         BIT            CONSTRAINT [DF_ConfigVariables_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ConfigVariables] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigVariables_Enum_ParentElement] FOREIGN KEY ([parent_element]) REFERENCES [config].[Enum_ParentElement] ([name])
);



