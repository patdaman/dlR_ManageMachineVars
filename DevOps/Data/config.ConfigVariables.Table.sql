USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_create_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_value_name]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_value_name]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_attribute]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_attribute]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_element]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_element]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_parent_element]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] DROP CONSTRAINT [DF_ConfigVariables_parent_element]
END

GO
/****** Object:  Table [config].[ConfigVariables]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ConfigVariables]') AND type in (N'U'))
DROP TABLE [config].[ConfigVariables]
GO
/****** Object:  Table [config].[ConfigVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ConfigVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[ConfigVariables](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[parent_element] [varchar](256) NOT NULL,
	[element] [varchar](256) NOT NULL,
	[key_name] [varchar](256) NOT NULL,
	[key] [varchar](256) NOT NULL,
	[value_name] [varchar](256) NOT NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_ConfigVariables] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[ConfigVariables] ON 

INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100125, N'appSettings', N'add', N'key', N'log4net.config', N'value', CAST(N'2017-03-08T00:33:19.830' AS DateTime), CAST(N'2017-03-08T00:33:19.830' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100126, N'appSettings', N'add', N'key', N'ProductImageAssociationWS.UploadDirectory', N'value', CAST(N'2017-03-08T00:33:19.850' AS DateTime), CAST(N'2017-03-08T00:33:19.850' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100127, N'appSettings', N'add', N'key', N'CustomSearchImageAssociationWS.UploadDirectory', N'value', CAST(N'2017-03-08T00:33:19.857' AS DateTime), CAST(N'2017-03-08T00:33:19.857' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100128, N'log4net', N'appender', N'name', N'FileLog', N'type', CAST(N'2017-03-08T00:33:55.913' AS DateTime), CAST(N'2017-03-08T00:33:55.913' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100129, N'appender', N'file', N'value', N'd:\\Logs\\US\\ManagerI18n\\ManagerI18n.log', N'value', CAST(N'2017-03-08T00:33:55.940' AS DateTime), CAST(N'2017-03-08T00:33:55.940' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100130, N'appender', N'appendToFile', N'value', N'true', N'value', CAST(N'2017-03-08T00:33:55.947' AS DateTime), CAST(N'2017-03-08T00:33:55.947' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100131, N'appender', N'maximumFileSize', N'value', N'5120KB', N'value', CAST(N'2017-03-08T00:33:55.957' AS DateTime), CAST(N'2017-03-08T00:33:55.957' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100132, N'appender', N'maxSizeRollBackups', N'value', N'40', N'value', CAST(N'2017-03-08T00:33:55.977' AS DateTime), CAST(N'2017-03-08T00:33:55.977' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100133, N'appender', N'layout', N'type', N'log4net.Layout.PatternLayout', N'type', CAST(N'2017-03-08T00:33:55.983' AS DateTime), CAST(N'2017-03-08T00:33:55.983' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100134, N'layout', N'conversionPattern', N'value', N'%d [%t] %-5p %c [%x] - %m%n', N'value', CAST(N'2017-03-08T00:33:55.990' AS DateTime), CAST(N'2017-03-08T00:33:55.990' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100135, N'appender', N'filter', N'type', N'log4net.Filter.LoggerMatchFilter', N'type', CAST(N'2017-03-08T00:33:55.997' AS DateTime), CAST(N'2017-03-08T00:33:55.997' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100136, N'filter', N'loggerToMatch', N'', N'', N'loggerToMatch', CAST(N'2017-03-08T00:33:56.007' AS DateTime), CAST(N'2017-03-08T00:33:56.007' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100137, N'filter', N'acceptOnMatch', N'', N'', N'acceptOnMatch', CAST(N'2017-03-08T00:33:56.013' AS DateTime), CAST(N'2017-03-08T00:33:56.013' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100138, N'root', N'level', N'value', N'INFO', N'value', CAST(N'2017-03-08T00:33:56.020' AS DateTime), CAST(N'2017-03-08T00:33:56.020' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100139, N'root', N'appender-ref', N'ref', N'FileLog', N'ref', CAST(N'2017-03-08T00:33:56.037' AS DateTime), CAST(N'2017-03-08T00:33:56.037' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100140, N'databases', N'db', N'name', N'pt', N'connection_string', CAST(N'2017-03-08T00:38:13.507' AS DateTime), CAST(N'2017-03-22T13:23:54.067' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100141, N'databases', N'db', N'name', N'usergroups', N'connection_string', CAST(N'2017-03-08T00:38:13.533' AS DateTime), CAST(N'2017-03-08T00:38:13.533' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100142, N'databases', N'db', N'name', N'is', N'connection_string', CAST(N'2017-03-08T00:38:13.543' AS DateTime), CAST(N'2017-03-08T00:38:13.543' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100143, N'databases', N'db', N'name', N'ui', N'connection_string', CAST(N'2017-03-08T00:38:13.567' AS DateTime), CAST(N'2017-03-08T00:38:13.567' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100144, N'databases', N'db', N'name', N'gw', N'connection_string', CAST(N'2017-03-08T00:38:13.573' AS DateTime), CAST(N'2017-03-08T00:38:13.573' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100145, N'databases', N'db', N'name', N'bg', N'connection_string', CAST(N'2017-03-08T00:38:13.583' AS DateTime), CAST(N'2017-03-08T00:38:13.583' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100146, N'databases', N'db', N'name', N'urlrewrite', N'connection_string', CAST(N'2017-03-08T00:38:13.590' AS DateTime), CAST(N'2017-03-08T00:38:13.590' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100147, N'ICSClientConfig', N'ServerHost', N'', N'', N'ServerHost', CAST(N'2017-03-08T00:38:35.790' AS DateTime), CAST(N'2017-03-22T14:08:58.743' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100148, N'ICSClientConfig', N'ServerPort', N'', N'', N'ServerPort', CAST(N'2017-03-08T00:38:35.807' AS DateTime), CAST(N'2017-03-22T13:06:05.807' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100149, N'ICSClientConfig', N'ServerId', N'', N'', N'ServerId', CAST(N'2017-03-08T00:38:35.817' AS DateTime), CAST(N'2017-03-08T00:38:35.817' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100150, N'ICSClientConfig', N'KeysDir', N'', N'', N'KeysDir', CAST(N'2017-03-08T00:38:35.823' AS DateTime), CAST(N'2017-03-22T14:09:56.310' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100151, N'ICSClientConfig', N'Timeout', N'', N'', N'Timeout', CAST(N'2017-03-08T00:38:35.830' AS DateTime), CAST(N'2017-03-08T00:38:35.830' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100152, N'ICSClientConfig', N'RetryEnabled', N'', N'', N'RetryEnabled', CAST(N'2017-03-08T00:38:35.837' AS DateTime), CAST(N'2017-03-08T00:38:35.837' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100153, N'ICSClientConfig', N'RetryStart', N'', N'', N'RetryStart', CAST(N'2017-03-08T00:38:35.843' AS DateTime), CAST(N'2017-03-08T00:38:35.843' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100154, N'ICSClientConfig', N'LogLevel', N'', N'', N'LogLevel', CAST(N'2017-03-08T00:38:35.863' AS DateTime), CAST(N'2017-03-22T14:26:55.657' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100155, N'ICSClientConfig', N'LogFile', N'', N'', N'LogFile', CAST(N'2017-03-08T00:38:35.873' AS DateTime), CAST(N'2017-03-22T13:04:37.853' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100156, N'ICSClientConfig', N'LogMaxSize', N'', N'', N'LogMaxSize', CAST(N'2017-03-08T00:38:35.880' AS DateTime), CAST(N'2017-03-08T00:38:35.880' AS DateTime), 1)
INSERT [config].[ConfigVariables] ([id], [parent_element], [element], [key_name], [key], [value_name], [create_date], [modify_date], [active]) VALUES (100157, N'ICSClientConfig', N'ThrowLogException', N'', N'', N'ThrowLogException', CAST(N'2017-03-08T00:38:35.890' AS DateTime), CAST(N'2017-03-08T00:38:35.890' AS DateTime), 1)
SET IDENTITY_INSERT [config].[ConfigVariables] OFF
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_parent_element]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_parent_element]  DEFAULT ('appSettings') FOR [parent_element]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_element]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_element]  DEFAULT ('add') FOR [element]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_attribute]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_attribute]  DEFAULT ('key') FOR [key_name]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_value_name]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_value_name]  DEFAULT ('value') FOR [value_name]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariables_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariables] ADD  CONSTRAINT [DF_ConfigVariables_active]  DEFAULT ((1)) FOR [active]
END

GO
