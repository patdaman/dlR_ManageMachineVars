CREATE TABLE [shell].[Scripts] (
    [id]          INT           IDENTITY (100000, 1) NOT NULL,
    [script_name] VARCHAR (256) NOT NULL,
    [script_text] VARCHAR (MAX) NOT NULL,
    [create_date] DATETIME      CONSTRAINT [DF_Scripts_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      NULL,
    [is_active]   BIT           CONSTRAINT [DF_Scripts_is_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Scripts] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_Scripts] UNIQUE NONCLUSTERED ([script_name] ASC)
);



