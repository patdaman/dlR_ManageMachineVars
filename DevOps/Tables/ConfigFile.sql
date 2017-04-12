CREATE TABLE [config].[ConfigFile] (
    [id]              INT           IDENTITY (100000, 1) NOT NULL,
    [component_id]    INT           NOT NULL,
    [file_name]       VARCHAR (256) NOT NULL,
    [xml_declaration] VARCHAR (512) NULL,
    [root_element]    VARCHAR (256) CONSTRAINT [DF_ConfigFile_root_element] DEFAULT ('appSettings') NOT NULL,
    [create_date]     DATETIME      CONSTRAINT [DF_ConfigFile_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]     DATETIME      CONSTRAINT [DF_ConfigFile_modify_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ConfigFile] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigFile_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id]),
    CONSTRAINT [IX_ConfigFile] UNIQUE NONCLUSTERED ([component_id] ASC)
);











