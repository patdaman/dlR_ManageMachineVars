CREATE TABLE [config].[ConfigFileAttributes] (
    [id]              INT           IDENTITY (100000, 1) NOT NULL,
    [element_id]      INT           NOT NULL,
    [attribute_name]  VARCHAR (256) NOT NULL,
    [attribute_value] VARCHAR (256) NULL,
    [create_date]     DATETIME      CONSTRAINT [DF_ConfigFileAttributes_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]     DATETIME      CONSTRAINT [DF_ConfigFileAttributes_modify_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ConfigFileAttributes] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ConfigFileAttributes_ConfigFileElements] FOREIGN KEY ([element_id]) REFERENCES [config].[ConfigFileElements] ([id]),
    CONSTRAINT [IX_ConfigFileAttributes] UNIQUE NONCLUSTERED ([element_id] ASC, [attribute_name] ASC)
);

