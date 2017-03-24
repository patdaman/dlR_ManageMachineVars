USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] DROP CONSTRAINT [DF_Components_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] DROP CONSTRAINT [DF_Components_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] DROP CONSTRAINT [DF_Components_create_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_relative_path]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] DROP CONSTRAINT [DF_Components_relative_path]
END

GO
/****** Object:  Index [IX_Components]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Components]') AND name = N'IX_Components')
ALTER TABLE [config].[Components] DROP CONSTRAINT [IX_Components]
GO
/****** Object:  Table [config].[Components]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Components]') AND type in (N'U'))
DROP TABLE [config].[Components]
GO
/****** Object:  Table [config].[Components]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Components]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[Components](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[component_name] [varchar](256) NOT NULL,
	[relative_path] [varchar](256) NOT NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_Components] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[Components] ON 

INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100000, N'Commerce', N'Commerce\Commerce.config', CAST(N'2017-03-02T12:17:26.673' AS DateTime), CAST(N'2017-03-02T12:17:26.673' AS DateTime), 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100001, N'DAL', N'DAL\DAL.config', CAST(N'2017-03-02T12:18:00.380' AS DateTime), CAST(N'2017-03-02T12:18:00.380' AS DateTime), 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100002, N'ManagerI18N', N'ManagerI18N\App.Config', CAST(N'2017-03-02T12:19:07.447' AS DateTime), CAST(N'2017-03-02T12:19:07.447' AS DateTime), 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100003, N'ManagerI18N', N'ManagerI18N\log4net.config', CAST(N'2017-03-02T12:20:29.953' AS DateTime), CAST(N'2017-03-02T12:20:29.953' AS DateTime), 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100004, N'Services', N'Services\Manager\app.config', CAST(N'2017-03-02T12:21:28.840' AS DateTime), CAST(N'2017-03-02T12:21:28.840' AS DateTime), 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100005, N'Services', N'Services\Manager\log4net.config', CAST(N'2017-03-02T12:21:48.693' AS DateTime), CAST(N'2017-03-02T12:21:48.693' AS DateTime), 1)
SET IDENTITY_INSERT [config].[Components] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Components]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Components]') AND name = N'IX_Components')
ALTER TABLE [config].[Components] ADD  CONSTRAINT [IX_Components] UNIQUE NONCLUSTERED 
(
	[component_name] ASC,
	[relative_path] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_relative_path]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] ADD  CONSTRAINT [DF_Components_relative_path]  DEFAULT ('component_name + ''app.config''') FOR [relative_path]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] ADD  CONSTRAINT [DF_Components_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] ADD  CONSTRAINT [DF_Components_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Components_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Components] ADD  CONSTRAINT [DF_Components_active]  DEFAULT ((1)) FOR [active]
END

GO
