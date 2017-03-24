USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ServerGroups_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ServerGroups] DROP CONSTRAINT [DF_ServerGroups_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ServerGroups_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ServerGroups] DROP CONSTRAINT [DF_ServerGroups_create_date]
END

GO
/****** Object:  Index [IX_ServerGroups]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[ServerGroups]') AND name = N'IX_ServerGroups')
ALTER TABLE [config].[ServerGroups] DROP CONSTRAINT [IX_ServerGroups]
GO
/****** Object:  Table [config].[ServerGroups]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ServerGroups]') AND type in (N'U'))
DROP TABLE [config].[ServerGroups]
GO
/****** Object:  Table [config].[ServerGroups]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ServerGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[ServerGroups](
	[id] [int] NOT NULL,
	[group_name] [varchar](128) NOT NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_ServerGroups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ServerGroups]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[ServerGroups]') AND name = N'IX_ServerGroups')
ALTER TABLE [config].[ServerGroups] ADD  CONSTRAINT [IX_ServerGroups] UNIQUE NONCLUSTERED 
(
	[group_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ServerGroups_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ServerGroups] ADD  CONSTRAINT [DF_ServerGroups_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_ServerGroups_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[ServerGroups] ADD  CONSTRAINT [DF_ServerGroups_active]  DEFAULT ((1)) FOR [active]
END

GO
