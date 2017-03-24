USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] DROP CONSTRAINT [DF_Applications_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] DROP CONSTRAINT [DF_Applications_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] DROP CONSTRAINT [DF_Applications_create_date]
END

GO
/****** Object:  Index [IX_Applications_name_release]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Applications]') AND name = N'IX_Applications_name_release')
ALTER TABLE [config].[Applications] DROP CONSTRAINT [IX_Applications_name_release]
GO
/****** Object:  Table [config].[Applications]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Applications]') AND type in (N'U'))
DROP TABLE [config].[Applications]
GO
/****** Object:  Table [config].[Applications]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Applications]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[Applications](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[application_name] [varchar](256) NOT NULL,
	[release] [varchar](128) NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[Applications] ON 

INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100000, N'app1', N'.42', CAST(N'2017-02-23T09:09:44.067' AS DateTime), CAST(N'2017-02-23T09:09:44.067' AS DateTime), 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100001, N'app2', N'.42', CAST(N'2017-02-23T09:09:52.487' AS DateTime), CAST(N'2017-02-23T09:09:52.487' AS DateTime), 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100002, N'app3', N'.42', CAST(N'2017-02-23T09:10:00.620' AS DateTime), CAST(N'2017-02-23T09:10:00.620' AS DateTime), 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100003, N'Admin', NULL, CAST(N'2017-03-01T15:41:26.057' AS DateTime), CAST(N'2017-03-01T15:41:26.057' AS DateTime), 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100004, N'ManagerServices', NULL, CAST(N'2017-03-01T15:41:32.763' AS DateTime), CAST(N'2017-03-01T15:41:32.763' AS DateTime), 1)
SET IDENTITY_INSERT [config].[Applications] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Applications_name_release]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Applications]') AND name = N'IX_Applications_name_release')
ALTER TABLE [config].[Applications] ADD  CONSTRAINT [IX_Applications_name_release] UNIQUE NONCLUSTERED 
(
	[application_name] ASC,
	[release] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] ADD  CONSTRAINT [DF_Applications_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] ADD  CONSTRAINT [DF_Applications_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Applications_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Applications] ADD  CONSTRAINT [DF_Applications_active]  DEFAULT ((1)) FOR [active]
END

GO
