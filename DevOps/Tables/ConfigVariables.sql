CREATE TABLE [config].[ConfigVariables] (
    [id]             INT           IDENTITY (100000, 1) NOT NULL,
    [parent_element] VARCHAR (256) CONSTRAINT [DF_ConfigVariables_parent_element] DEFAULT ('appSettings') NOT NULL,
    [element]        VARCHAR (256) NULL,
    [attribute]      VARCHAR (256) NULL,
    [key]            VARCHAR (256) NULL,
    [value_name]     VARCHAR (256) NULL,
    [create_date]    DATETIME      CONSTRAINT [DF_ConfigVariables_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME      CONSTRAINT [DF_ConfigVariables_modify_date] DEFAULT (getdate()) NOT NULL,
    [active]         BIT           CONSTRAINT [DF_ConfigVariables_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ConfigVariables] PRIMARY KEY CLUSTERED ([id] ASC)
);















