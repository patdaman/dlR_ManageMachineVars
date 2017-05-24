CREATE TABLE [config].[ConfigVariables] (
    [id]               INT            IDENTITY (100000, 1) NOT NULL,
    [full_element]     VARCHAR (4000) NULL,
    [parent_element]   VARCHAR (4000) CONSTRAINT [DF_ConfigVariables_parent_element] DEFAULT ('appSettings') NOT NULL,
    [configfile_id]    INT            NULL,
    [element]          VARCHAR (256)  NULL,
    [attribute]        VARCHAR (256)  NULL,
    [key]              VARCHAR (256)  NULL,
    [value_name]       VARCHAR (256)  NULL,
    [create_date]      DATETIME       CONSTRAINT [DF_ConfigVariables_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME       CONSTRAINT [DF_ConfigVariables_modify_date] DEFAULT (getdate()) NOT NULL,
    [last_modify_user] VARCHAR (128)  CONSTRAINT [DF_ConfigVariables_last_modify_user] DEFAULT ('suser_name') NOT NULL,
    [active]           BIT            CONSTRAINT [DF_ConfigVariables_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ConfigVariables] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigVariables_ConfigFile] FOREIGN KEY ([configfile_id]) REFERENCES [config].[ConfigFile] ([id])
);























