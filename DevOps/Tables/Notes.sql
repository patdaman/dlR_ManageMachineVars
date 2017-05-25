CREATE TABLE [config].[Notes] (
    [id]               INT            IDENTITY (100000, 1) NOT NULL,
    [note_id]          INT            NOT NULL,
    [note_type]        VARCHAR (256)  CONSTRAINT [DF_Notes_note_type] DEFAULT ('ConfigVariables') NOT NULL,
    [text]             VARCHAR (4000) NOT NULL,
    [create_date]      DATETIME       CONSTRAINT [DF_Notes_create_date] DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME       CONSTRAINT [DF_Notes_modify_date] DEFAULT (getdate()) NOT NULL,
    [last_modify_user] VARCHAR (256)  CONSTRAINT [DF_Notes_last_modify_user] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_Notes] UNIQUE NONCLUSTERED ([note_id] ASC, [note_type] ASC, [create_date] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Table that note relates to', @level0type = N'SCHEMA', @level0name = N'config', @level1type = N'TABLE', @level1name = N'Notes', @level2type = N'COLUMN', @level2name = N'note_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id from the table where note is applicable', @level0type = N'SCHEMA', @level0name = N'config', @level1type = N'TABLE', @level1name = N'Notes', @level2type = N'COLUMN', @level2name = N'note_id';

