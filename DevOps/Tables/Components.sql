CREATE TABLE [config].[Components] (
    [id]               INT           IDENTITY (100000, 1) NOT NULL,
    [component_name]   VARCHAR (256) NOT NULL,
    [relative_path]    VARCHAR (256) CONSTRAINT [DF_Components_relative_path] DEFAULT ('component_name + ''app.config''') NOT NULL,
    [create_date]      DATETIME      CONSTRAINT [DF_Components_create_date] DEFAULT (getdate()) NOT NULL,
    [last_modify_user] VARCHAR (128) CONSTRAINT [DF_Components_last_modify_user] DEFAULT ('suser_name') NOT NULL,
    [modify_date]      DATETIME      CONSTRAINT [DF_Components_modify_date] DEFAULT (getdate()) NOT NULL,
    [active]           BIT           CONSTRAINT [DF_Components_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Components] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_Components] UNIQUE NONCLUSTERED ([component_name] ASC, [relative_path] ASC)
);





