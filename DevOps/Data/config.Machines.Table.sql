USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_Machines_Enum_Locations]') AND parent_object_id = OBJECT_ID(N'[config].[Machines]'))
ALTER TABLE [config].[Machines] DROP CONSTRAINT [FK_Machines_Enum_Locations]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] DROP CONSTRAINT [DF_Machines_active]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] DROP CONSTRAINT [DF_Machines_modify_date]
END

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] DROP CONSTRAINT [DF_Machines_create_date]
END

GO
/****** Object:  Index [IX_Machines_MachineName]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Machines]') AND name = N'IX_Machines_MachineName')
ALTER TABLE [config].[Machines] DROP CONSTRAINT [IX_Machines_MachineName]
GO
/****** Object:  Table [config].[Machines]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Machines]') AND type in (N'U'))
DROP TABLE [config].[Machines]
GO
/****** Object:  Table [config].[Machines]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[Machines]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[Machines](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[machine_name] [varchar](128) NOT NULL,
	[ip_address] [varchar](128) NULL,
	[location] [varchar](128) NOT NULL,
	[usage] [varchar](128) NOT NULL,
	[create_date] [datetime] NOT NULL,
	[modify_date] [datetime] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_Machines] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [config].[Machines] ON 

INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100000, N'hqdev04', NULL, N'Las Vegas', N'development', CAST(N'2017-02-10T15:49:46.100' AS DateTime), CAST(N'2017-02-10T15:49:46.100' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100001, N'hqdev07', NULL, N'Las Vegas', N'development', CAST(N'2017-02-10T15:50:13.743' AS DateTime), CAST(N'2017-02-10T15:50:13.743' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100002, N'168LYR1', NULL, N'Solana Beach', N'development', CAST(N'2017-02-22T14:38:56.070' AS DateTime), CAST(N'2017-02-22T14:38:56.070' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100003, N'bender', NULL, N'Solana Beach', N'development', CAST(N'2017-02-23T09:18:06.763' AS DateTime), CAST(N'2017-02-23T09:18:06.763' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100004, N'lvmgr01', NULL, N'Las Vegas', N'QA', CAST(N'2017-03-01T15:40:51.587' AS DateTime), CAST(N'2017-03-01T15:40:51.587' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100005, N'hqdev01', NULL, N'Las Vegas', N'development', CAST(N'2017-03-03T16:18:23.770' AS DateTime), CAST(N'2017-03-03T16:18:23.770' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100007, N'eumgr01', NULL, N'Azure', N'production', CAST(N'2017-03-14T16:49:43.297' AS DateTime), CAST(N'2017-03-14T16:49:43.297' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100008, N'eumgr02', NULL, N'Azure', N'production', CAST(N'2017-03-14T16:49:53.090' AS DateTime), CAST(N'2017-03-14T16:49:53.090' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100009, N'hqdev08', NULL, N'Solana Beach', N'development', CAST(N'2017-03-14T16:50:35.443' AS DateTime), CAST(N'2017-03-14T16:50:35.443' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100010, N'sdapi02.dc.pti.com', NULL, N'Solana Beach', N'QA', CAST(N'2017-03-20T14:02:48.230' AS DateTime), CAST(N'2017-03-20T14:02:48.230' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100012, N'NULLsdapi01.dc.pti.com', NULL, N'Solana Beach', N'QA', CAST(N'2017-03-20T14:03:07.330' AS DateTime), CAST(N'2017-03-20T14:03:07.330' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100013, N'sdapi03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100014, N'sdapi04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100015, N'sddmn01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100016, N'sddmn02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100017, N'sdfpr01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100018, N'sdfpr02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100019, N'sdfpx01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100020, N'sdfpx02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100021, N'sdfpx03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100022, N'sdfpx04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100023, N'sdglb01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100024, N'sdglb02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100025, N'sdimw01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100026, N'sdimw03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100027, N'sdimw04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100028, N'sdint01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100029, N'sdint02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100030, N'sdint03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100031, N'sdint04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100032, N'sdjd01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100033, N'sdjd02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100034, N'sdlnk01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100035, N'sdlnk03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100036, N'sdmgr01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100037, N'sdmgr02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100038, N'sdmgr03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100039, N'sdpfd01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100040, N'sdpfd02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100041, N'sdpfd03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100042, N'sdpfd04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100043, N'sdpfe02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100044, N'sdpfe03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100045, N'sdpfe04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100046, N'sdpfe21.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100047, N'sdpfe22.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100048, N'sdpfi02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100049, N'sdpfi03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100050, N'sdpfi04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100051, N'sdpfi05.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100052, N'sdpfi06.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100053, N'sdpfi07.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100054, N'sdpfi08.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100055, N'sdpfi09.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100056, N'sdpfi10.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100057, N'sdpfi11.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100058, N'sdpfi12.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100059, N'sdpfi13.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100060, N'sdpfi14.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100061, N'sdstr01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100062, N'sdstr02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100063, N'sdstr03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100064, N'sdsvc01.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100065, N'sdsvc02.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100066, N'sdsvc03.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100067, N'sdsvc04.dc.pti.com', N'', N'Solana Beach', N'Production', CAST(N'2017-03-20T14:05:37.823' AS DateTime), CAST(N'2017-03-20T14:05:37.823' AS DateTime), 1)
SET IDENTITY_INSERT [config].[Machines] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Machines_MachineName]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[config].[Machines]') AND name = N'IX_Machines_MachineName')
ALTER TABLE [config].[Machines] ADD  CONSTRAINT [IX_Machines_MachineName] UNIQUE NONCLUSTERED 
(
	[machine_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_create_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] ADD  CONSTRAINT [DF_Machines_create_date]  DEFAULT (getdate()) FOR [create_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_modify_date]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] ADD  CONSTRAINT [DF_Machines_modify_date]  DEFAULT (getdate()) FOR [modify_date]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_Machines_active]') AND type = 'D')
BEGIN
ALTER TABLE [config].[Machines] ADD  CONSTRAINT [DF_Machines_active]  DEFAULT ((1)) FOR [active]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_Machines_Enum_Locations]') AND parent_object_id = OBJECT_ID(N'[config].[Machines]'))
ALTER TABLE [config].[Machines]  WITH CHECK ADD  CONSTRAINT [FK_Machines_Enum_Locations] FOREIGN KEY([location])
REFERENCES [config].[Enum_Locations] ([name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_Machines_Enum_Locations]') AND parent_object_id = OBJECT_ID(N'[config].[Machines]'))
ALTER TABLE [config].[Machines] CHECK CONSTRAINT [FK_Machines_Enum_Locations]
GO
