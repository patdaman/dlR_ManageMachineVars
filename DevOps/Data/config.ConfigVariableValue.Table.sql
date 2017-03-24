USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_Enum_EnvironmentType]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue] DROP CONSTRAINT [FK_ConfigVariableValue_Enum_EnvironmentType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue] DROP CONSTRAINT [FK_ConfigVariableValue_ConfigVariables]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariableValue_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariableValue] DROP CONSTRAINT [DF_ConfigVariableValue_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariableValue_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariableValue] DROP CONSTRAINT [DF_ConfigVariableValue_create_date]
END

GO
/****** Object:  Index [IX_ConfigVariableValue]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[ConfigVariableValue]') AND name = N'IX_ConfigVariableValue')
ALTER TABLE [config].[ConfigVariableValue] DROP CONSTRAINT [IX_ConfigVariableValue]
GO
/****** Object:  Table [config].[ConfigVariableValue]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ConfigVariableValue]') AND type in (N'U'))
DROP TABLE [config].[ConfigVariableValue]
GO
/****** Object:  Table [config].[ConfigVariableValue]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ConfigVariableValue]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[ConfigVariableValue](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[configvar_id] [int] NOT NULL,
	[environment_type] [varchar](128) NOT NULL,
	[value] [nvarchar](4000) NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[published_date] [datetime] NULL,
 CONSTRAINT [PK_ConfigVariableValue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[ConfigVariableValue] ON 

INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100129, 100125, N'development', N'D:\US\PrintableConfig\Services\Manager\log4net.config', CAST(N'2017-03-07T16:33:19.827' AS DateTime), CAST(N'2017-03-07T16:33:19.827' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100131, 100126, N'development', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-07T16:33:19.850' AS DateTime), CAST(N'2017-03-07T16:33:19.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100133, 100127, N'development', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-07T16:33:19.857' AS DateTime), CAST(N'2017-03-07T16:33:19.857' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100135, 100128, N'development', N'log4net.Appender.RollingFileAppender', CAST(N'2017-03-07T16:33:55.913' AS DateTime), CAST(N'2017-03-07T16:33:55.913' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100137, 100129, N'development', N'd:\\Logs\\US\\ManagerI18n\\ManagerI18n.log', CAST(N'2017-03-07T16:33:55.940' AS DateTime), CAST(N'2017-03-07T16:33:55.940' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100139, 100130, N'development', N'true', CAST(N'2017-03-07T16:33:55.947' AS DateTime), CAST(N'2017-03-07T16:33:55.947' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100141, 100131, N'development', N'5120KB', CAST(N'2017-03-07T16:33:55.957' AS DateTime), CAST(N'2017-03-07T16:33:55.957' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100143, 100132, N'development', N'40', CAST(N'2017-03-07T16:33:55.977' AS DateTime), CAST(N'2017-03-07T16:33:55.977' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100145, 100133, N'development', N'log4net.Layout.PatternLayout', CAST(N'2017-03-07T16:33:55.983' AS DateTime), CAST(N'2017-03-07T16:33:55.983' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100147, 100134, N'development', N'%d [%t] %-5p %c [%x] - %m%n', CAST(N'2017-03-07T16:33:55.990' AS DateTime), CAST(N'2017-03-07T16:33:55.990' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100149, 100135, N'development', N'log4net.Filter.LoggerMatchFilter', CAST(N'2017-03-07T16:33:55.997' AS DateTime), CAST(N'2017-03-07T16:33:55.997' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100151, 100136, N'development', N'NHibernate.', CAST(N'2017-03-07T16:33:56.007' AS DateTime), CAST(N'2017-03-07T16:33:56.007' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100153, 100137, N'development', N'false', CAST(N'2017-03-07T16:33:56.013' AS DateTime), CAST(N'2017-03-07T16:33:56.013' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100155, 100138, N'development', N'INFO', CAST(N'2017-03-07T16:33:56.020' AS DateTime), CAST(N'2017-03-07T16:33:56.020' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100157, 100139, N'development', N'FileLog', CAST(N'2017-03-07T16:33:56.037' AS DateTime), CAST(N'2017-03-07T16:33:56.037' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100159, 100140, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.507' AS DateTime), CAST(N'2017-03-07T16:38:13.507' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100161, 100141, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.533' AS DateTime), CAST(N'2017-03-07T16:38:13.533' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100163, 100142, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=ImageServer;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.543' AS DateTime), CAST(N'2017-03-07T16:38:13.543' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100165, 100143, N'development', N'server=lvdbc01.dc.pti.com;uid=ui_writer;pwd=writeright;database=UIPublishing;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.567' AS DateTime), CAST(N'2017-03-07T16:38:13.567' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100167, 100144, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.573' AS DateTime), CAST(N'2017-03-07T16:38:13.573' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100169, 100145, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.583' AS DateTime), CAST(N'2017-03-07T16:38:13.583' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100171, 100146, N'development', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-07T16:38:13.590' AS DateTime), CAST(N'2017-03-07T16:38:13.590' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100173, 100147, N'development', N'ics2.ic3.com', CAST(N'2017-03-07T16:38:35.790' AS DateTime), CAST(N'2017-03-07T16:38:35.790' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100175, 100148, N'development', N'80', CAST(N'2017-03-07T16:38:35.807' AS DateTime), CAST(N'2017-03-07T16:38:35.807' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100177, 100149, N'development', N'CyberSource_SJC_US', CAST(N'2017-03-07T16:38:35.817' AS DateTime), CAST(N'2017-03-07T16:38:35.817' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100179, 100150, N'development', N'D:\opt\Keys', CAST(N'2017-03-07T16:38:35.823' AS DateTime), CAST(N'2017-03-07T16:38:35.823' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100181, 100151, N'development', N'90', CAST(N'2017-03-07T16:38:35.830' AS DateTime), CAST(N'2017-03-07T16:38:35.830' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100183, 100152, N'development', N'false', CAST(N'2017-03-07T16:38:35.837' AS DateTime), CAST(N'2017-03-07T16:38:35.837' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100185, 100153, N'development', N'30', CAST(N'2017-03-07T16:38:35.843' AS DateTime), CAST(N'2017-03-07T16:38:35.843' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100187, 100154, N'development', N'LOG_TRANSACTIONS', CAST(N'2017-03-07T16:38:35.863' AS DateTime), CAST(N'2017-03-07T16:38:35.863' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100189, 100155, N'development', N'D:\\logs\\US\\ics\\icstest.log', CAST(N'2017-03-07T16:38:35.873' AS DateTime), CAST(N'2017-03-07T16:38:35.873' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100191, 100156, N'development', N'3', CAST(N'2017-03-07T16:38:35.880' AS DateTime), CAST(N'2017-03-07T16:38:35.880' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100193, 100157, N'development', N'false', CAST(N'2017-03-07T16:38:35.890' AS DateTime), CAST(N'2017-03-07T16:38:35.890' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100194, 100125, N'qa', N'D:\US\PrintableConfig\Services\Manager\log4net.config', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100195, 100126, N'qa', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100196, 100127, N'qa', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100197, 100128, N'qa', N'log4net.Appender.RollingFileAppender', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100198, 100129, N'qa', N'd:\\Logs\\US\\ManagerI18n\\ManagerI18n.log', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100199, 100130, N'qa', N'true', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100200, 100131, N'qa', N'5120KB', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100201, 100132, N'qa', N'40', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100202, 100133, N'qa', N'log4net.Layout.PatternLayout', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100203, 100134, N'qa', N'%d [%t] %-5p %c [%x] - %m%n', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100204, 100135, N'qa', N'log4net.Filter.LoggerMatchFilter', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100205, 100136, N'qa', N'NHibernate.', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100206, 100137, N'qa', N'false', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100207, 100138, N'qa', N'INFO', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100208, 100139, N'qa', N'FileLog', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100209, 100140, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100210, 100141, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100211, 100142, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=ImageServer;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100212, 100143, N'qa', N'server=lvdbc01.dc.pti.com;uid=ui_writer;pwd=writeright;database=UIPublishing;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100213, 100144, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100214, 100145, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100215, 100146, N'qa', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100216, 100147, N'qa', N'ics2.ic3.com', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100217, 100148, N'qa', N'80', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100218, 100149, N'qa', N'CyberSource_SJC_US', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100219, 100150, N'qa', N'D:\opt\Keys', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100220, 100151, N'qa', N'90', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100221, 100152, N'qa', N'false', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100222, 100153, N'qa', N'30', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100223, 100154, N'qa', N'LOG_TRANSACTIONS', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100224, 100155, N'qa', N'D:\\logs\\US\\ics\\icstest.log', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100225, 100156, N'qa', N'3', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100226, 100157, N'qa', N'false', CAST(N'2017-03-23T14:22:18.850' AS DateTime), CAST(N'2017-03-23T14:22:18.850' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100227, 100125, N'production', N'D:\US\PrintableConfig\Services\Manager\log4net.config', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100228, 100126, N'production', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100229, 100127, N'production', N'\\lvfsc02.dc.pti.com\PortalAssets\PrintOneLogos\temp', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100230, 100128, N'production', N'log4net.Appender.RollingFileAppender', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100231, 100129, N'production', N'd:\\Logs\\US\\ManagerI18n\\ManagerI18n.log', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100232, 100130, N'production', N'true', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100233, 100131, N'production', N'5120KB', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100234, 100132, N'production', N'40', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100235, 100133, N'production', N'log4net.Layout.PatternLayout', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100236, 100134, N'production', N'%d [%t] %-5p %c [%x] - %m%n', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100237, 100135, N'production', N'log4net.Filter.LoggerMatchFilter', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100238, 100136, N'production', N'NHibernate.', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100239, 100137, N'production', N'false', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100240, 100138, N'production', N'INFO', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100241, 100139, N'production', N'FileLog', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100242, 100140, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100243, 100141, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100244, 100142, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=ImageServer;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100245, 100143, N'production', N'server=lvdbc01.dc.pti.com;uid=ui_writer;pwd=writeright;database=UIPublishing;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100246, 100144, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100247, 100145, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_us_bg;pwd=56BL7x8QU;database=BGateway;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100248, 100146, N'production', N'server=lvdbc01.dc.pti.com;uid=pti_app;pwd=56BL7x8QU;database=pt;multipleactiveresultsets=True', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100249, 100147, N'production', N'ics2.ic3.com', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100250, 100148, N'production', N'80', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100251, 100149, N'production', N'CyberSource_SJC_US', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100252, 100150, N'production', N'D:\opt\Keys', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100253, 100151, N'production', N'90', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100254, 100152, N'production', N'false', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100255, 100153, N'production', N'30', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100256, 100154, N'production', N'LOG_TRANSACTIONS', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100257, 100155, N'production', N'D:\\logs\\US\\ics\\icstest.log', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100258, 100156, N'production', N'3', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
INSERT [config].[ConfigVariableValue] ([id], [configvar_id], [environment_type], [value], [create_date], [modify_date], [published_date]) VALUES (100259, 100157, N'production', N'false', CAST(N'2017-03-23T14:22:24.980' AS DateTime), CAST(N'2017-03-23T14:22:24.980' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [config].[ConfigVariableValue] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ConfigVariableValue]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[ConfigVariableValue]') AND name = N'IX_ConfigVariableValue')
ALTER TABLE [config].[ConfigVariableValue] ADD  CONSTRAINT [IX_ConfigVariableValue] UNIQUE NONCLUSTERED 
(
	[environment_type] ASC,
	[configvar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariableValue_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariableValue] ADD  CONSTRAINT [DF_ConfigVariableValue_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ConfigVariableValue_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ConfigVariableValue] ADD  CONSTRAINT [DF_ConfigVariableValue_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue]  WITH CHECK ADD  CONSTRAINT [FK_ConfigVariableValue_ConfigVariables] FOREIGN KEY([configvar_id])
REFERENCES [config].[ConfigVariables] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue] CHECK CONSTRAINT [FK_ConfigVariableValue_ConfigVariables]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_Enum_EnvironmentType]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue]  WITH CHECK ADD  CONSTRAINT [FK_ConfigVariableValue_Enum_EnvironmentType] FOREIGN KEY([environment_type])
REFERENCES [config].[Enum_EnvironmentType] ([name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ConfigVariableValue_Enum_EnvironmentType]') AND parent_object_id = OBJECT_ID(N'[config].[ConfigVariableValue]'))
ALTER TABLE [config].[ConfigVariableValue] CHECK CONSTRAINT [FK_ConfigVariableValue_Enum_EnvironmentType]
GO
