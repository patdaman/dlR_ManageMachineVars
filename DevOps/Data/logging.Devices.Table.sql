USE [DevOps]
GO
SET IDENTITY_INSERT [logging].[Devices] ON 

INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100000, N'hqdev04', N'10.0.10.24', CAST(N'2017-03-14T16:51:35.270' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100001, N'hqdev07', N'10.0.10.27', CAST(N'2017-03-14T16:51:45.210' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100002, N'hqdev08', N'10.0.10.28', CAST(N'2017-03-14T16:51:51.430' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100003, N'168LYR1', N'10.0.0.120', CAST(N'2017-03-14T16:52:22.620' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100004, N'lvmgr01', N'10.0.4.120', CAST(N'2017-03-14T16:53:49.850' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100005, N'hqdev01', N'10.0.7.21', CAST(N'2017-03-14T16:54:08.380' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100006, N'eumgr01', N'10.0.11.21', CAST(N'2017-03-14T16:54:26.773' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100007, N'eumgr02', N'10.0.11.22', CAST(N'2017-03-14T16:54:32.697' AS DateTime), NULL, 1)
INSERT [logging].[Devices] ([id], [device_name], [ip_address], [create_date], [modify_date], [active]) VALUES (100008, N'bender', N'192.168.2.74', CAST(N'2017-03-14T16:56:43.383' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [logging].[Devices] OFF
