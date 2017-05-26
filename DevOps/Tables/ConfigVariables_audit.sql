CREATE TABLE [config].[ConfigVariables_audit] (
    [AuditID]       INT            IDENTITY (1, 1) NOT NULL,
    [Type]          CHAR (1)       NULL,
    [id]            INT            NOT NULL,
    [FieldName]     VARCHAR (128)  NULL,
    [OldValue]      VARCHAR (1000) NULL,
    [NewValue]      VARCHAR (1000) NULL,
    [UpdateUtcDate] DATETIME2 (3)  DEFAULT (getutcdate()) NULL,
    [UserName]      VARCHAR (128)  NULL,
    CONSTRAINT [PK_ConfigVariables_audit] PRIMARY KEY CLUSTERED ([AuditID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ConfigVariables_audit_id]
    ON [config].[ConfigVariables_audit]([id] ASC);

