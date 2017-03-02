CREATE TABLE [shell].[ExecutionHistory] (
    [id]              INT             IDENTITY (100000, 1) NOT NULL,
    [script_id]       INT             CONSTRAINT [DF_ExecutionHistory_script_id] DEFAULT ((0)) NOT NULL,
    [script_name]     VARCHAR (256)   NULL,
    [user_name]       VARCHAR (256)   NULL,
    [execution_dt]    DATETIME        CONSTRAINT [DF_ExecutionHistory_execution_dt] DEFAULT (getdate()) NOT NULL,
    [contains_errors] BIT             CONSTRAINT [DF_ExecutionHistory_contains_errors] DEFAULT ((0)) NOT NULL,
    [output]          NVARCHAR (4000) NULL,
    CONSTRAINT [PK_ExecutionHistory] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ExecutionHistory_Scripts] FOREIGN KEY ([script_id]) REFERENCES [shell].[Scripts] ([id])
);



