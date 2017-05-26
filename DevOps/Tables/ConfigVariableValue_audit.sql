CREATE TABLE [config].[ConfigVariableValue_audit] (
    [AuditID]          INT            IDENTITY (1, 1) NOT NULL,
    [Type]             CHAR (1)       NULL,
    [id]               INT            NOT NULL,
    [configvar_id]     INT            NOT NULL,
    [environment_type] VARCHAR (128)  NOT NULL,
    [FieldName]        VARCHAR (128)  NULL,
    [OldValue]         VARCHAR (1000) NULL,
    [NewValue]         VARCHAR (1000) NULL,
    [UpdateUtcDate]    DATETIME2 (3)  DEFAULT (getutcdate()) NULL,
    [UserName]         VARCHAR (128)  NULL,
    CONSTRAINT [PK_ConfigVariableValue_audit] PRIMARY KEY CLUSTERED ([AuditID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ConfigVariableValue_audit_id]
    ON [config].[ConfigVariableValue_audit]([id] ASC, [configvar_id] ASC, [environment_type] ASC);

