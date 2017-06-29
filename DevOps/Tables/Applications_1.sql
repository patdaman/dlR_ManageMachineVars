CREATE TABLE [monitor].[Applications] (
    [id]               INT           IDENTITY (100000, 1) NOT NULL,
    [app_name]         VARCHAR (128) NOT NULL,
    [app_id]           VARCHAR (256) NULL,
    [app_key]          VARCHAR (256) NULL,
    [resourcegroup_id] INT           NOT NULL,
    [create_date]      DATETIME      CONSTRAINT [DF_Applications_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME      NULL,
    [last_modify_user] VARCHAR (128) CONSTRAINT [DF_Applications_last_modify_user] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Applications_1] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Applications_ResourceGroups] FOREIGN KEY ([resourcegroup_id]) REFERENCES [monitor].[ResourceGroups] ([id]),
    CONSTRAINT [IX_Applications] UNIQUE NONCLUSTERED ([app_name] ASC)
);

