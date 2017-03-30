﻿CREATE TABLE [config].[ConfigFile] (
    [id]              INT           IDENTITY (100000, 1) NOT NULL,
    [component_id]    INT           NOT NULL,
    [file_name]       VARCHAR (256) NOT NULL,
    [environment]     VARCHAR (128) CONSTRAINT [DF_ConfigFile_environment] DEFAULT ('development') NOT NULL,
    [xml_declaration] VARCHAR (512) NULL,
    [create_date]     DATETIME      CONSTRAINT [DF_ConfigFile_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]     DATETIME      CONSTRAINT [DF_ConfigFile_modify_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ConfigFile] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigFile_Components] FOREIGN KEY ([component_id]) REFERENCES [config].[Components] ([id]),
    CONSTRAINT [FK_ConfigFile_Enum_EnvironmentType] FOREIGN KEY ([environment]) REFERENCES [config].[Enum_EnvironmentType] ([name]),
    CONSTRAINT [IX_ConfigFile] UNIQUE NONCLUSTERED ([component_id] ASC, [file_name] ASC, [environment] ASC)
);





