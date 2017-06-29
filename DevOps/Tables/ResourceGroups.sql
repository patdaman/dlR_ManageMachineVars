CREATE TABLE [monitor].[ResourceGroups] (
    [id]          INT           IDENTITY (100000, 1) NOT NULL,
    [group_name]  VARCHAR (128) NOT NULL,
    [create_date] DATETIME      CONSTRAINT [DF_ResourceGroups_create_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ResourceGroups] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_ResourceGroups] UNIQUE NONCLUSTERED ([group_name] ASC)
);

