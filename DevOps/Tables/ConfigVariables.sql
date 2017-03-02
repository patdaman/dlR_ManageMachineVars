CREATE TABLE [config].[ConfigVariables] (
    [id]             INT           IDENTITY (100000, 1) NOT NULL,
    [parent_element] VARCHAR (256) CONSTRAINT [DF_ConfigVariables_parent_element] DEFAULT ('appSettings') NOT NULL,
    [element]        VARCHAR (256) CONSTRAINT [DF_ConfigVariables_element] DEFAULT ('add') NOT NULL,
    [attribute]      VARCHAR (256) CONSTRAINT [DF_ConfigVariables_attribute] DEFAULT ('key') NOT NULL,
    [key]            VARCHAR (256) NOT NULL,
    [value_name]     VARCHAR (256) CONSTRAINT [DF_ConfigVariables_value_name] DEFAULT ('value') NOT NULL,
    [config_path]    VARCHAR (256) NOT NULL,
    [create_date]    DATETIME      CONSTRAINT [DF_ConfigVariables_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME      NULL,
    [active]         BIT           CONSTRAINT [DF_ConfigVariables_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ConfigVariables] PRIMARY KEY CLUSTERED ([id] ASC)
);





