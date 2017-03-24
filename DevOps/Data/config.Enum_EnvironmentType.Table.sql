USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Enum_MachineUsageType_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Enum_EnvironmentType] DROP CONSTRAINT [DF_Enum_MachineUsageType_active]
END

GO
/****** Object:  Index [IX_UsageType_Enum_usage]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Enum_EnvironmentType]') AND name = N'IX_UsageType_Enum_usage')
ALTER TABLE [config].[Enum_EnvironmentType] DROP CONSTRAINT [IX_UsageType_Enum_usage]
GO
/****** Object:  Table [config].[Enum_EnvironmentType]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Enum_EnvironmentType]') AND type in (N'U'))
DROP TABLE [config].[Enum_EnvironmentType]
GO
/****** Object:  Table [config].[Enum_EnvironmentType]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Enum_EnvironmentType]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[Enum_EnvironmentType](
	[name] [varchar](128) NOT NULL,
	[value] [varchar](128) NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_UsageType_Enum] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'development', N'development', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'dr', N'dr', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'production', N'production', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'qa', N'qa', 1)
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UsageType_Enum_usage]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Enum_EnvironmentType]') AND name = N'IX_UsageType_Enum_usage')
ALTER TABLE [config].[Enum_EnvironmentType] ADD  CONSTRAINT [IX_UsageType_Enum_usage] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Enum_MachineUsageType_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Enum_EnvironmentType] ADD  CONSTRAINT [DF_Enum_MachineUsageType_active]  DEFAULT ((1)) FOR [active]
END

GO
