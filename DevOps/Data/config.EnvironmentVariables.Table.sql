USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_EnvironmentVariables_Enum_EnvironmentVariableType]') AND parent_object_id = OBJECT_ID(N'[config].[EnvironmentVariables]'))
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [FK_EnvironmentVariables_Enum_EnvironmentVariableType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [DF_EnvironmentVariables_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [DF_EnvironmentVariables_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [DF_EnvironmentVariables_create_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_type]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [DF_EnvironmentVariables_type]
END

GO
/****** Object:  Index [IX_EnvironmentVariables_key_type]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[EnvironmentVariables]') AND name = N'IX_EnvironmentVariables_key_type')
ALTER TABLE [config].[EnvironmentVariables] DROP CONSTRAINT [IX_EnvironmentVariables_key_type]
GO
/****** Object:  Table [config].[EnvironmentVariables]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[EnvironmentVariables]') AND type in (N'U'))
DROP TABLE [config].[EnvironmentVariables]
GO
/****** Object:  Table [config].[EnvironmentVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[EnvironmentVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[EnvironmentVariables](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[key] [varchar](256) NOT NULL,
	[value] [varchar](256) NOT NULL,
	[type] [varchar](128) NOT NULL,
	[path] [varchar](256) NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_EnvironmentVariables] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[EnvironmentVariables] ON 

INSERT [config].[EnvironmentVariables] ([id], [key], [value], [type], [path], [create_date], [modify_date], [active]) VALUES (100000, N'test1', N'marcom', N'user', NULL, CAST(N'2017-02-23T15:57:16.227' AS DateTime), CAST(N'2017-02-23T15:57:16.227' AS DateTime), 1)
INSERT [config].[EnvironmentVariables] ([id], [key], [value], [type], [path], [create_date], [modify_date], [active]) VALUES (100001, N'test2', N'password', N'machine', NULL, CAST(N'2017-02-23T15:57:25.460' AS DateTime), CAST(N'2017-02-23T15:57:25.460' AS DateTime), 1)
SET IDENTITY_INSERT [config].[EnvironmentVariables] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_EnvironmentVariables_key_type]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[EnvironmentVariables]') AND name = N'IX_EnvironmentVariables_key_type')
ALTER TABLE [config].[EnvironmentVariables] ADD  CONSTRAINT [IX_EnvironmentVariables_key_type] UNIQUE NONCLUSTERED 
(
	[key] ASC,
	[type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_type]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] ADD  CONSTRAINT [DF_EnvironmentVariables_type]  DEFAULT (user_name()) FOR [type]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] ADD  CONSTRAINT [DF_EnvironmentVariables_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] ADD  CONSTRAINT [DF_EnvironmentVariables_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_EnvironmentVariables_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[EnvironmentVariables] ADD  CONSTRAINT [DF_EnvironmentVariables_active]  DEFAULT ((1)) FOR [active]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_EnvironmentVariables_Enum_EnvironmentVariableType]') AND parent_object_id = OBJECT_ID(N'[config].[EnvironmentVariables]'))
ALTER TABLE [config].[EnvironmentVariables]  WITH CHECK ADD  CONSTRAINT [FK_EnvironmentVariables_Enum_EnvironmentVariableType] FOREIGN KEY([type])
REFERENCES [config].[Enum_EnvironmentVariableType] ([name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_EnvironmentVariables_Enum_EnvironmentVariableType]') AND parent_object_id = OBJECT_ID(N'[config].[EnvironmentVariables]'))
ALTER TABLE [config].[EnvironmentVariables] CHECK CONSTRAINT [FK_EnvironmentVariables_Enum_EnvironmentVariableType]
GO
