CREATE TABLE [config].[Applications] (
    [id]               INT           IDENTITY (100000, 1) NOT NULL,
    [application_name] VARCHAR (256) NOT NULL,
    [release]          VARCHAR (128) NULL,
    [create_date]      DATETIME      CONSTRAINT [DF_Applications_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME      NULL,
    [active]           BIT           CONSTRAINT [DF_Applications_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_Applications_name_release] UNIQUE NONCLUSTERED ([application_name] ASC, [release] ASC)
);





