CREATE TABLE [config].[ServerGroups] (
    [id]          INT           NOT NULL,
    [group_name]  VARCHAR (128) NOT NULL,
    [create_date] DATETIME      CONSTRAINT [DF_ServerGroups_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      NULL,
    [active]      BIT           CONSTRAINT [DF_ServerGroups_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ServerGroups] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_ServerGroups] UNIQUE NONCLUSTERED ([group_name] ASC)
);



