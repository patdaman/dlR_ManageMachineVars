USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Enum_Locations_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Enum_Locations] DROP CONSTRAINT [DF_Enum_Locations_active]
END

GO
/****** Object:  Table [config].[Enum_Locations]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Enum_Locations]') AND type in (N'U'))
DROP TABLE [config].[Enum_Locations]
GO
/****** Object:  Table [config].[Enum_Locations]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Enum_Locations]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[Enum_Locations](
	[name] [varchar](128) NOT NULL,
	[value] [varchar](128) NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_Enum_Locations] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[Enum_Locations] ([name], [value], [active]) VALUES (N'Azure', N'Azure', 1)
INSERT [config].[Enum_Locations] ([name], [value], [active]) VALUES (N'Las Vegas', N'Las Vegas', 1)
INSERT [config].[Enum_Locations] ([name], [value], [active]) VALUES (N'ScaleMetrix', N'ScaleMetrix', 1)
INSERT [config].[Enum_Locations] ([name], [value], [active]) VALUES (N'Solana Beach', N'Solana Beach', 1)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Enum_Locations_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Enum_Locations] ADD  CONSTRAINT [DF_Enum_Locations_active]  DEFAULT ((1)) FOR [active]
END

GO
