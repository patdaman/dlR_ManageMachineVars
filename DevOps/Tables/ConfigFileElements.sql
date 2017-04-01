CREATE TABLE [config].[ConfigFileElements] (
    [id]             INT           IDENTITY (100000, 1) NOT NULL,
    [configfile_id]  INT           NOT NULL,
    [element_name]   VARCHAR (256) NOT NULL,
    [parent_element] INT           CONSTRAINT [DF_ConfigFileElements_ParentElementId] DEFAULT ((0)) NOT NULL,
    [create_date]    DATETIME      CONSTRAINT [DF_ConfigFileElements_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME      CONSTRAINT [DF_ConfigFileElements_modify_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ConfigFileElements] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigFileElements_ConfigFile] FOREIGN KEY ([configfile_id]) REFERENCES [config].[ConfigFile] ([id]),
    CONSTRAINT [IX_ConfigFileElements] UNIQUE NONCLUSTERED ([configfile_id] ASC, [element_name] ASC, [parent_element] ASC)
);



