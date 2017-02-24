CREATE TABLE [config].[Enum_ParentElement] (
    [name]   VARCHAR (256) NOT NULL,
    [value]  VARCHAR (256) NOT NULL,
    [active] BIT           CONSTRAINT [DF_Enum_ParentElement_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Enum_ParentElement] PRIMARY KEY CLUSTERED ([name] ASC)
);

